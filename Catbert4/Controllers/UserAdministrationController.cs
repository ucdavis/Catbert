using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Catbert4.Core.Domain;
using Catbert4.Models;
using Catbert4.Services;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Attributes;
using UCDArch.Web.Helpers;
using UCDArch.Core.Utils;

namespace Catbert4.Controllers
{
    /// <summary>
    /// Controller for the User class
    /// </summary>
    public class UserAdministrationController : ApplicationControllerBase
    {
        private readonly IDirectorySearchService _directorySearchService;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Permission> _permissionRepository;
        private readonly IRepository<UnitAssociation> _unitAssociationRepository;
        private readonly IRepository<ApplicationRole> _applicationRoleRepository;

        public UserAdministrationController(
            IDirectorySearchService directorySearchService,
            IRepository<User> userRepository, 
            IRepository<Permission> permissionRepository, 
            IRepository<UnitAssociation> unitAssociationRepository,
            IRepository<ApplicationRole> applicationRoleRepository)
        {
            _directorySearchService = directorySearchService;
            _userRepository = userRepository;
            _permissionRepository = permissionRepository;
            _unitAssociationRepository = unitAssociationRepository;
            _applicationRoleRepository = applicationRoleRepository;
        }

        //
        // GET: /User/
        [HandleTransactionsManually] //No data access required
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            var userList = _userRepository.Queryable
                .OrderBy(x => x.LastName)
                .Select(
                    x =>
                    new UserListModel { Email = x.Email, FirstName = x.FirstName, LastName = x.LastName, Login = x.LoginId });

            return View(userList.ToList());
        }

        //
        // GET: /Application/Details/login
        public ActionResult Details(string id)
        {
            var user = GetUser(id);
            
            if (user == null) return RedirectToAction("Index");

            var model = Mapper.Map<User, UserShowModel>(user);

            SetPermissionsAndUnitAssociations(id, model);

            return View(model);
        }

        //
        // GET: /User/Find
        [HandleTransactionsManually] //We are just using directory services
        public ActionResult Find(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return View();
            }
            
            var user = _directorySearchService.FindUser(searchTerm);

            if (user == null)
            {
                ViewData["searchTerm"] = searchTerm;
                Message = string.Format("No users found with email or kerberos Id = {0}", searchTerm);
                return View();
            }

            Message = string.Format("User {0} found.  Please verify the information below and then click add",
                                    user.FullName);

            return RedirectToAction("Add", new { id = user.LoginId });
        }

        //
        // GET /User/Add/login
        public ActionResult Add(string id)
        {
            //see if we already have the user in the db
            var userExists = _userRepository.Queryable.Any(x => x.LoginId == id);

            if (userExists)
            {
                Message = string.Format("You have been redirected to the edit page for {0}", id);
                return RedirectToAction("Edit", new { id = id });
            }
            
            //get the user back from directory services
            var user = _directorySearchService.FindUser(id);

            if (user == null)
            {
                Message = string.Format("No users found with kerberos Id = {0}", id);
                return RedirectToAction("Find");
            }

            var newUser = Mapper.Map<DirectoryUser, User>(user);
            
            return View(newUser);
        }

        //
        // POST: /User/Add
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add(User user)
        {
            ModelState.Remove("Id"); //clear out the Id binding error when the binder tries to use user.Id 

            var userToCreate = new User();

            Mapper.Map(user, userToCreate);
            
            userToCreate.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                _userRepository.EnsurePersistent(userToCreate);

                Message = "User Created Successfully.  You can now add permissions and unit associations";

                return RedirectToAction("Edit", new {id = user.LoginId});
            }
            else
            {
                return View(user);
            }
        }

        //
        // GET: /User/Edit/login
        public ActionResult Edit(string id)
        {
            var user = GetUser(id);
            
            if (user == null) return RedirectToAction("Index");

			var viewModel = UserViewModel.Create(Repository);
			viewModel.User = user;

            SetPermissionsAndUnitAssociations(id, viewModel.UserShowModel);
            
			return View(viewModel);
        }
        
        //
        // POST: /User/Edit/5
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(string id, User user)
        {
            var userToEdit = GetUser(id);

            if (userToEdit == null) return RedirectToAction("Index");

            Mapper.Map(user, userToEdit);
            
            userToEdit.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                _userRepository.EnsurePersistent(userToEdit);

                Message = "User Edited Successfully";

                return RedirectToAction("Index");
            }
            else
            {
				var viewModel = UserViewModel.Create(Repository);
                viewModel.User = user;

                return View(viewModel);
            }
        }
        
        //
        // GET: /User/Delete/5 
        public ActionResult Delete(string id)
        {
            var user = GetUser(id);

            if (user == null) return RedirectToAction("Index");

            var model = Mapper.Map<User, UserShowModel>(user);

            SetPermissionsAndUnitAssociations(id, model);

            return View(model);
        }

        //
        // POST: /User/Delete/5
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string id, User user)
        {
            var userToDelete = GetUser(id);
            
            if (userToDelete == null) return RedirectToAction("Index");

            userToDelete.Permissions.Clear();
            userToDelete.UnitAssociations.Clear();

            _userRepository.Remove(userToDelete);

            Message = "User Removed Successfully";

            return RedirectToAction("Index");
        }

        [HttpPost]
        [BypassAntiForgeryToken]
        public JsonResult GetRolesForApplication(int? val)
        {
            if (val.HasValue == false) return Json(new {});

            var rolesForApp = from appRole in _applicationRoleRepository.Queryable
                              where appRole.Application.Id == val
                              orderby appRole.Role.Name
                              orderby appRole.Level
                              select new {Value = appRole.Role.Id, Text = appRole.Role.Name};

            return Json(rolesForApp.ToList());
        }

        [HttpPost]
        public JsonResult RemovePermission(int id)
        {
            var permission = _permissionRepository.GetNullableById(id);

            if (permission == null) return Json(new JsonStatusModel(success: false));

            _permissionRepository.Remove(permission);

            return Json(new JsonStatusModel(success: true));
        }

        [HttpPost]
        public JsonResult RemoveAssociation(int id)
        {
            var association = _unitAssociationRepository.GetNullableById(id);

            if (association == null) return Json(new JsonStatusModel(success: false));

            _unitAssociationRepository.Remove(association);

            return Json(new JsonStatusModel(success: true));
        }        
        
        [HttpPost]
        public JsonResult AddPermission(int userId, int applicationId, int roleId)
        {
            //First verify the role can be associated with this position
            var roleIsAssociatedWithApplication = (from appRole in _applicationRoleRepository.Queryable
                                                  where appRole.Application.Id == applicationId
                                                        && appRole.Role.Id == roleId
                                                  select appRole).Any();

            //Now make sure the user doesn't already have this role
            var userHasRole = (from perm in _permissionRepository.Queryable
                              where perm.Application.Id == applicationId
                                    && perm.Role.Id == roleId
                                    && perm.User.Id == userId
                              select perm).Any();

            if (roleIsAssociatedWithApplication == false || userHasRole)
            {
                return Json(new JsonStatusModel(success: false) {Comment = "User already has this role"});
            }

            var newPermission = new Permission
                                    {
                                        Application = Repository.OfType<Application>().GetById(applicationId),
                                        Role = Repository.OfType<Role>().GetById(roleId),
                                        User = Repository.OfType<User>().GetById(userId)
                                    };
            
            _permissionRepository.EnsurePersistent(newPermission);

            return Json(new JsonStatusModel(success: true) {Identifier = newPermission.Id});
        }

        [HttpPost]
        public JsonResult AddAssociation(int userId, int applicationId, int unitId)
        {
            //Now make sure the user doesn't already have this unit association
            var userHasUnitAssociation = (from u in _unitAssociationRepository.Queryable
                                         where u.Application.Id == applicationId
                                               && u.Unit.Id == unitId
                                               && u.User.Id == userId
                                         select u).Any();

            if (userHasUnitAssociation)
            {
                return Json(new JsonStatusModel(success: false) { Comment = "User already has this unit association" });
            }

            var newUnitAssociation = new UnitAssociation()
            {
                Application = Repository.OfType<Application>().GetById(applicationId),
                Unit = Repository.OfType<Unit>().GetById(unitId),
                User = Repository.OfType<User>().GetById(userId)
            };
            
            _unitAssociationRepository.EnsurePersistent(newUnitAssociation);

            return Json(new JsonStatusModel(success: true) {Identifier = newUnitAssociation.Id});
        }

        public JsonResult SearchUsers(string term)
        {
            var users = _userRepository.Queryable
                .Where(x => x.LoginId.Contains(term) || x.FirstName.Contains(term) || x.LastName.Contains(term) || x.Email.Contains(term))
                .Select(x => new { value = x.LoginId, x.FirstName, x.LastName, x.Email });
            
            return Json(users.ToList(), JsonRequestBehavior.AllowGet);
        }
        
        private User GetUser(string login)
        {
            return _userRepository.Queryable.Where(x => x.LoginId == login).SingleOrDefault();
        }

        private void SetPermissionsAndUnitAssociations(string id, UserShowModel model)
        {
            model.Permissions = (from p in _permissionRepository.Queryable
                                 where p.User.LoginId == id
                                 select
                                     new UserShowModel.PermissionModel
                                     {
                                         Id = p.Id,
                                         ApplicationName = p.Application.Name,
                                         RoleName = p.Role.Name
                                     }).ToList();

            model.UnitAssociations = (from ua in _unitAssociationRepository.Queryable
                                      where ua.User.LoginId == id
                                      select
                                          new UserShowModel.UnitAssociationModel
                                          {
                                              Id = ua.Id,
                                              ApplicationName = ua.Application.Name,
                                              UnitName = ua.Unit.ShortName
                                          }).ToList();
        }
    }

	/// <summary>
    /// ViewModel for the User class
    /// </summary>
    public class UserViewModel
	{
		public User User { get; set; }
	    public UserShowModel UserShowModel { get; set; }
	    public UserLookupModel UserLookupModel { get; set; }

		public static UserViewModel Create(IRepository repository)
		{
			Check.Require(repository != null, "Repository must be supplied");

		    var viewModel = new UserViewModel
		                        {
		                            User = new User(),
		                            UserShowModel = new UserShowModel(),
		                            UserLookupModel = new UserLookupModel()
		                        };

		    SetLookups(viewModel.UserLookupModel, repository);
            
			return viewModel;
		}

        /// <summary>
        /// Set the lookups efficiently which are in the userLookupModel
        /// </summary>
	    private static void SetLookups(UserLookupModel model, IRepository repository)
        {
            model.Applications =
                repository.OfType<Application>().Queryable.OrderBy(x => x.Name).Select(
                    x => new KeyValuePair<int, string>(x.Id, x.Name)).ToList();
            model.Units =
                repository.OfType<Unit>().Queryable.OrderBy(x => x.ShortName).Select(
                    x => new KeyValuePair<int, string>(x.Id, x.ShortName)).ToList();
        }
	}
}
