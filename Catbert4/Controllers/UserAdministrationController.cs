using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Catbert4.Core.Domain;
using Catbert4.Models;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Helpers;
using UCDArch.Core.Utils;

namespace Catbert4.Controllers
{
    /// <summary>
    /// Controller for the User class
    /// </summary>
    public class UserAdministrationController : ApplicationControllerBase
    {
	    private readonly IRepository<User> _userRepository;
        private readonly IRepository<Permission> _permissionRepository;
        private readonly IRepository<UnitAssociation> _unitAssociationRepository;

        public UserAdministrationController(
            IRepository<User> userRepository, 
            IRepository<Permission> permissionRepository, 
            IRepository<UnitAssociation> unitAssociationRepository)
        {
            _userRepository = userRepository;
            _permissionRepository = permissionRepository;
            _unitAssociationRepository = unitAssociationRepository;
        }

        //
        // GET: /User/
        public ActionResult Index()
        {
            var userList = _userRepository.Queryable
                .OrderBy(x => x.LastName)
                .Select(
                    x =>
                    new UserListModel
                        {Email = x.Email, FirstName = x.FirstName, LastName = x.LastName, Login = x.LoginId});
            
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
        // GET: /User/Create
        public ActionResult Create()
        {
			var viewModel = UserViewModel.Create(Repository);
            
            return View(viewModel);
        } 

        //
        // POST: /User/Create
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(User user)
        {
            var userToCreate = new User();

            TransferValues(user, userToCreate);

            userToCreate.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                _userRepository.EnsurePersistent(userToCreate);

                Message = "User Created Successfully";

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
        public ActionResult Delete(int id)
        {
			var user = _userRepository.GetNullableById(id);

            if (user == null) return RedirectToAction("Index");

            return View(user);
        }

        //
        // POST: /User/Delete/5
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int id, User user)
        {
			var userToDelete = _userRepository.GetNullableById(id);

            if (userToDelete == null) return RedirectToAction("Index");

            _userRepository.Remove(userToDelete);

            Message = "User Removed Successfully";

            return RedirectToAction("Index");
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
        
        /// <summary>
        /// Transfer editable values from source to destination
        /// </summary>
        private static void TransferValues(User source, User destination)
        {
            throw new NotImplementedException();
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
            //model.Applications = repository.OfType<Application>().Queryable.Select(x => new KeyValuePair<int,string>(x.Id, x.Name)).ToList();
        }
	}
}
