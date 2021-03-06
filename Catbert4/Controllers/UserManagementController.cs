﻿using System.Linq;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Web.Mvc;
using Catbert4.Models;
using Catbert4.Services;
using Catbert4.Services.UserManagement;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;
using Catbert4.Core.Domain;
using AutoMapper;

namespace Catbert4.Controllers
{
	/// <summary>
	/// Controller for the UserManagement class
	/// </summary>
	[HandleError(ExceptionType = typeof(AuthenticationException), View = "AuthorizationError")]
	public class UserManagementController : ApplicationControllerBase
	{
		private readonly IRepository<User> _userRepository;
		private readonly IRepository<Application> _applicationRepository;
		private readonly IRepository<Unit> _unitRepository;
		private readonly IRepository<Role> _roleRepository;
		private readonly IRepository<Permission> _permissionRepository;
		private readonly IRepository<UnitAssociation> _unitAssociationRepository;
		private readonly IUserService _userService;
		private readonly IUnitService _unitService;
		private readonly IRoleService _roleService;
		private readonly IDirectorySearchService _directorySearchService;

		public UserManagementController(IRepository<User> userRepository, 
			IRepository<Application> applicationRepository,
			IRepository<Unit> unitRepository,
			IRepository<Role> roleRepository,
			IRepository<Permission> permissionRepository,
			IRepository<UnitAssociation> unitAssociationRepository,
			IUserService userService, 
			IUnitService unitService, 
			IRoleService roleService, 
			IDirectorySearchService directorySearchService)
		{
			_userRepository = userRepository;
			_applicationRepository = applicationRepository;
			_unitRepository = unitRepository;
			_roleRepository = roleRepository;
			_permissionRepository = permissionRepository;
			_unitAssociationRepository = unitAssociationRepository;
			_userService = userService;
			_unitService = unitService;
			_roleService = roleService;
			_directorySearchService = directorySearchService;
		}

		/// <summary>
		/// GET: /UserManagement/Manage/app
		/// #1
		/// </summary>
		/// <param name="application"></param>
		/// <returns></returns>
		public ActionResult Manage(string application) //, string role, string unit)
		{
			var model = UserManagementViewModel.Create(_permissionRepository, _unitAssociationRepository);
			model.Application = application;
			
			model.Units =
				_unitService.GetVisibleByUser(application, CurrentUser.Identity.Name).ToList().Select(
					x => new KeyValuePair<int, string>(x.Id, x.ShortName)).ToList();

			model.Roles =
				_roleService.GetVisibleByUser(application, CurrentUser.Identity.Name).Select(
					x => new KeyValuePair<int, string>(x.Id, x.Name)).ToList();
			
			//var users = _userService.GetByApplication(application, CurrentUser.Identity.Name, role, unit).ToList();
			var users = _userService.GetByApplication(application, CurrentUser.Identity.Name).ToList();
			
			model.ConvertToUserShowModel(users, application);

			return View(model);
		}

		/// <summary>
		/// #2
		/// </summary>
		/// <param name="searchTerm"></param>
		/// <returns></returns>
		public JsonResult FindUser(string searchTerm)
		{
			ServiceUser serviceUser = null;

			var directoryUser = _directorySearchService.FindUser(searchTerm);

			if (directoryUser != null) serviceUser = new ServiceUser(directoryUser);

			return Json(serviceUser, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// #3
		/// </summary>
		/// <param name="application"></param>
		/// <param name="serviceUser"></param>
		/// <param name="roleId"></param>
		/// <param name="unitId"></param>
		/// <returns></returns>
		[HttpPost]
		public JsonResult InsertNewUser(string application, ServiceUser serviceUser, int roleId, int unitId)
		{
			var user = _userRepository.Queryable.Where(x => x.LoginId == serviceUser.Login).SingleOrDefault();

			if (user == null) 
			{
				user = Mapper.Map<ServiceUser, User>(serviceUser);
				InsertNewUser(user);
			}

			var app = GetApplication(application);
			var role = _roleRepository.GetById(roleId);
			var unit = _unitRepository.GetById(unitId);
			
			AssociateRole(app, role, user);
			AssociateUnit(app, unit, user);

			//Pull down all the user's roles
			var roles = (from p in _permissionRepository.Queryable
						 where p.Application.Name == application
							   && p.User.Id == user.Id
						 orderby p.Role.Name
						 select new { Role = p.Role.Name.Trim() }).ToList();

			//Now all the units for this user
			var units = (from unitAssociation in _unitAssociationRepository.Queryable
						 where unitAssociation.Application.Name == application
							   && unitAssociation.User.Id == user.Id
						 orderby unitAssociation.Unit.ShortName
						 select new { Units = unitAssociation.Unit.ShortName.Trim() }).ToList();

			serviceUser.Roles = string.Join(", ", roles.Select(x=>x.Role));
			serviceUser.Units = string.Join(", ", units.Select(x=>x.Units));

			serviceUser.FullNameAndLogin = user.FullNameAndLogin;

			return Json(serviceUser);
		}

		/// <summary>
		/// #4
		/// </summary>
		/// <param name="application"></param>
		/// <param name="login"></param>
		/// <returns></returns>
		public JsonResult LoadUser(string application, string login)
		{
			var user = _userRepository.Queryable.Where(x => x.LoginId == login).Single();

			var model = Mapper.Map<User, UserShowModel>(user);

			SetPermissionsAndUnitAssociations(application, login, model);

			return Json(model, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// #5
		/// </summary>
		/// <param name="application"></param>
		/// <param name="login"></param>
		/// <param name="id"></param>
		[HttpPost]
		public void RemoveUnit(string application, string login, int id)
		{
			EnsureCurrentUserCanManageLogin(application, login);
			EnsureCurrentUserCanModifyUnit(application, id);

			//We must check that the given unit association is indeed in this application and granted to this login
			var unitAssociation =
				_unitAssociationRepository.Queryable
					.Where(x => x.Unit.Id == id && x.Application.Name == application && x.User.LoginId == login)
					.Single();
			
			_unitAssociationRepository.Remove(unitAssociation);
		}

		/// <summary>
		/// #6
		/// </summary>
		/// <param name="application"></param>
		/// <param name="login"></param>
		/// <param name="id"></param>
		[HttpPost]
		public void AddUnit(string application, string login, int id)
		{
			EnsureCurrentUserCanManageLogin(application, login);
			EnsureCurrentUserCanModifyUnit(application, id);

			var app = GetApplication(application);
			var user = GetUser(login);
			var unit = _unitRepository.GetById(id);
			
			AssociateUnit(app, unit, user);
		}

        /// <summary>
        /// #7
        /// </summary>
        /// <param name="application"></param>
        /// <param name="login"></param>
        /// <param name="id"></param>
		[HttpPost]
		public void RemovePermission(string application, string login, int id)
		{
			EnsureCurrentUserCanManageLogin(application, login);
			EnsureCurrentUserCanModifyRole(application, id);

			//We must check that the given permission is indeed in this application and granted to this login
			var permission =
				_permissionRepository.Queryable
					.Where(x => x.Role.Id == id && x.Application.Name == application && x.User.LoginId == login)
					.Single();

			_permissionRepository.Remove(permission);
		}

        /// <summary>
        /// #8
        /// </summary>
        /// <param name="application"></param>
        /// <param name="login"></param>
        /// <param name="id"></param>
		[HttpPost]
		public void AddPermission(string application, string login, int id)
		{
			EnsureCurrentUserCanManageLogin(application, login);
			EnsureCurrentUserCanModifyRole(application, id);

			var app = GetApplication(application);
			var role = _roleRepository.GetById(id);
			var user = GetUser(login);
			
			AssociateRole(app, role, user);
		}

		private void AssociateUnit(Application application, Unit unit, User user)
		{
			var unitAssociationExists =
				_unitAssociationRepository.Queryable.Any(
					x => x.Application.Id == application.Id && x.Unit.Id == unit.Id && x.User.Id == user.Id);

			if (unitAssociationExists) return;

			var unitAssociation = new UnitAssociation
									  {
										  Inactive = false,
										  Application = application,
										  Unit = unit,
										  User = user
									  };
			
			_unitAssociationRepository.EnsurePersistent(unitAssociation);
		}

		private void AssociateRole(Application application, Role role, User user)
		{
			var permissionExists =
				_permissionRepository.Queryable.Any(
					x => x.Application.Id == application.Id && x.Role.Id == role.Id && x.User.Id == user.Id);

			if (permissionExists) return;
			
			var permission = new Permission
								 {
									 Inactive = false, 
									 Application = application, 
									 Role = role, 
									 User = user
								 };

			_permissionRepository.EnsurePersistent(permission);
		}

		private void EnsureCurrentUserCanModifyRole(string application, int roleId)
		{
			var manageableRoles = _roleService.GetVisibleByUser(application, CurrentUser.Identity.Name);

			Check.Require(manageableRoles.Where(x => x.Id == roleId).Count() > 0,
						  string.Format("{0} does not have access to manage the given role",
										CurrentUser.Identity.Name));
		}

		private void EnsureCurrentUserCanModifyUnit(string application, int unitId)
		{
			var manageableUnits = _unitService.GetVisibleByUser(application, CurrentUser.Identity.Name);

			Check.Require(manageableUnits.Where(x => x.Id == unitId).Count() > 0,
						  string.Format("{0} does not have access to manage the given unit",
										CurrentUser.Identity.Name));
		}

		private void EnsureCurrentUserCanManageLogin(string application, string loginToManage)
		{
			Check.Require(_userService.CanUserManageGivenLogin(application, CurrentUser.Identity.Name, loginToManage),
						  string.Format("{0} does not have access to manage {1} within the {2} application",
										CurrentUser.Identity.Name, loginToManage, application));
		}

		private void SetPermissionsAndUnitAssociations(string application, string login, UserShowModel model)
		{
			model.Permissions = (from p in _permissionRepository.Queryable
								 where p.User.LoginId == login && p.Application.Name == application
								 select
									 new UserShowModel.PermissionModel
									 {
										 Id = p.Id,
										 RoleId = p.Role.Id,
										 RoleName = p.Role.Name.Trim()
									 }).ToList();

			model.UnitAssociations = (from ua in _unitAssociationRepository.Queryable
									  where ua.User.LoginId == login && ua.Application.Name == application
									  select
										  new UserShowModel.UnitAssociationModel
										  {
											  Id = ua.Id,
											  UnitId = ua.Unit.Id,
											  UnitName = ua.Unit.ShortName.Trim()
										  }).ToList();
		}

		/// <summary>
		/// Insert a new user into the database
		/// </summary>
		private void InsertNewUser(User user)
		{
			//Make sure the user given in valid
			Check.Require(user.IsValid(), string.Format("User not valid: {0}", string.Join(", ", user.ValidationResults().Select(x=>x.Message))));
			
			_userRepository.EnsurePersistent(user);
		}

		private User GetUser(string login)
		{
			return _userRepository.Queryable.Where(x => x.LoginId == login).Single();
		}

		private Application GetApplication(string application)
		{
			return _applicationRepository.Queryable.Where(x => x.Name == application).Single();
		}
	}

	/// <summary>
	/// ViewModel for the UserManagement class
	/// </summary>
	public class UserManagementViewModel
	{
		private static IRepository<Permission> _permissionRepository;
		private static IRepository<UnitAssociation> _unitAssociationRepository;

		public IList<KeyValuePair<int, string>> Units { get; set; }
		public IList<KeyValuePair<int, string>> Roles { get; set; }

		public List<UserShowModel> UserShowModel { get; set; }

		public string Application { get; set; }

		public static UserManagementViewModel Create(IRepository<Permission> permissionRepository, IRepository<UnitAssociation> unitAssociationRepository)
		{
			Check.Require(permissionRepository != null, "Repository must be supplied");
			Check.Require(unitAssociationRepository != null, "Repository must be supplied");

			_permissionRepository = permissionRepository;
			_unitAssociationRepository = unitAssociationRepository;
	
			var viewModel = new UserManagementViewModel();
 
			return viewModel;
		}

		public void ConvertToUserShowModel(List<User> users, string application)
		{
			var userIds = users.Select(x => x.Id).ToArray();

			//Pull down all the role names for these users
			var roles = (from p in _permissionRepository.Queryable
						where p.Application.Name == application
							  && userIds.Contains(p.User.Id)
						select new { UserId = p.User.Id, RoleName = p.Role.Name }).ToList();

			//Now all the units for these users
			var units = (from p in _unitAssociationRepository.Queryable
						 where p.Application.Name == application
							   && userIds.Contains(p.User.Id)
						 select new { UserId = p.User.Id, UnitName = p.Unit.ShortName }).ToList();

			//Pull them all together
			var result = from u in users
					select
						new UserShowModel
							{
								Login = u.LoginId,
								FirstName = u.FirstName,
								LastName = u.LastName,
								Email = u.Email,
								FullNameAndLogin = u.FullNameAndLogin,
								Permissions =
									roles.Where(x => x.UserId == u.Id).Select(
										x => new UserShowModel.PermissionModel {RoleName = x.RoleName}).ToList(),
								UnitAssociations =
									units.Where(x => x.UserId == u.Id).Select(
										x => new UserShowModel.UnitAssociationModel {UnitName = x.UnitName}).ToList()
							};

			UserShowModel = result.ToList();
		}
	}
}
