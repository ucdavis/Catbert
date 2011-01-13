using System;
using System.Linq;
using System.Collections.Generic;
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
	[Authorize]
	public class UserManagementController : ApplicationControllerBase
	{
		private readonly IRepository<User> _userRepository;
		private readonly IRepository<Permission> _permissionRepository;
		private readonly IRepository<UnitAssociation> _unitAssociationRepository;
	    private readonly IUserService _userService;
		private readonly IUnitService _unitService;
		private readonly IRoleService _roleService;
		private readonly IDirectorySearchService _directorySearchService;

		public UserManagementController(IRepository<User> userRepository, 
			IRepository<Permission> permissionRepository,
			IRepository<UnitAssociation> unitAssociationRepository,
			IUserService userService, 
			IUnitService unitService, 
			IRoleService roleService, 
			IDirectorySearchService directorySearchService)
		{
			_userRepository = userRepository;
			_permissionRepository = permissionRepository;
			_unitAssociationRepository = unitAssociationRepository;
		    _userService = userService;
			_unitService = unitService;
			_roleService = roleService;
			_directorySearchService = directorySearchService;
		}

		//
		// GET: /UserManagement/Manage/app
		//Optional:  filter by ?role= and/or ?unit=
		public ActionResult Manage(string application, string role, string unit)
		{
			var model = UserManagementViewModel.Create(_permissionRepository, _unitAssociationRepository);
			model.Application = application;
			
			model.Units =
				_unitService.GetVisibleByUser(application, CurrentUser.Identity.Name).ToList().Select(
					x => new KeyValuePair<int, string>(x.Id, x.ShortName)).ToList();

			model.Roles =
				_roleService.GetVisibleByUser(application, CurrentUser.Identity.Name).Select(
					x => new KeyValuePair<int, string>(x.Id, x.Name)).ToList();
			
			var users = _userService.GetByApplication(application, CurrentUser.Identity.Name, role, unit).ToList();
			
			model.ConvertToUserShowModel(users, application);

			return View(model);
		}

		public JsonResult FindUser(string searchTerm)
		{
			ServiceUser serviceUser = null;

			var directoryUser = _directorySearchService.FindUser(searchTerm);

			if (directoryUser != null) serviceUser = new ServiceUser(directoryUser);

			return Json(serviceUser, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult InsertNewUser(string application, ServiceUser serviceUser, int roleId, int unitId)
		{
			var user = _userRepository.Queryable.Where(x => x.LoginId == serviceUser.Login).SingleOrDefault();

			if (user == null) 
			{
				user = Mapper.Map<ServiceUser, User>(serviceUser);
				InsertNewUser(user);
			}

			var app = Repository.OfType<Application>().Queryable.Where(x => x.Name == application).Single();
			var role = Repository.OfType<Role>().GetById(roleId);
			var unit = Repository.OfType<Unit>().GetById(unitId);
			
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

		public JsonResult LoadUser(string application, string login)
		{
			var user = _userRepository.Queryable.Where(x => x.LoginId == login).Single();

			var model = Mapper.Map<User, UserShowModel>(user);

			SetPermissionsAndUnitAssociations(application, login, model);

			return Json(model, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public void RemoveUnit(string application, string login, int id)
		{
            EnsureCurrentUserCanManageLogin(application, login);

            //We must check that the given unit association is indeed in this application and granted to this login
		    var unitAsoociation =
		        _unitAssociationRepository.Queryable
		            .Where(x => x.Id == id && x.Application.Name == application && x.User.LoginId == login)
		            .Single();
            
            _unitAssociationRepository.Remove(unitAsoociation);
		}
		
		[HttpPost]
		public void RemovePermission(string application, string login, int id)
		{
            EnsureCurrentUserCanManageLogin(application, login);

            //We must check that the given permission is indeed in this application and granted to this login
            var permission =
                _permissionRepository.Queryable
                    .Where(x => x.Id == id && x.Application.Name == application && x.User.LoginId == login)
                    .Single();

            _permissionRepository.Remove(permission);
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
										 RoleName = p.Role.Name.Trim()
									 }).ToList();

			model.UnitAssociations = (from ua in _unitAssociationRepository.Queryable
									  where ua.User.LoginId == login && ua.Application.Name == application
									  select
										  new UserShowModel.UnitAssociationModel
										  {
											  Id = ua.Id,
											  UnitName = ua.Unit.ShortName.Trim()
										  }).ToList();
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

		/// <summary>
		/// Insert a new user into the database
		/// </summary>
		private void InsertNewUser(User user)
		{
			//Make sure the user given in valid
			Check.Require(user.IsValid(), string.Format("User not valid: {0}", string.Join(", ", user.ValidationResults().Select(x=>x.Message))));
			
			_userRepository.EnsurePersistent(user);
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
