using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using Catbert4.Models;
using Catbert4.Services;
using Catbert4.Services.UserManagement;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Controller;
using UCDArch.Web.Helpers;
using UCDArch.Core.Utils;
using Catbert4.Core.Domain;

namespace Catbert4.Controllers
{
	/// <summary>
	/// Controller for the UserManagement class
	/// </summary>
	[Authorize]
	public class UserManagementController : ApplicationControllerBase
	{
	    private readonly IRepository<User> _usermanagementRepository;
	    private readonly IUserService _userService;
	    private readonly IUnitService _unitService;
	    private readonly IRoleService _roleService;
	    private readonly IDirectorySearchService _directorySearchService;

	    public UserManagementController(IRepository<User> usermanagementRepository, 
            IUserService userService, 
            IUnitService unitService, 
            IRoleService roleService, 
            IDirectorySearchService directorySearchService)
		{
		    _usermanagementRepository = usermanagementRepository;
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
		    var model = UserManagementViewModel.Create(Repository);
            
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
        public JsonResult InsertNewUser(ServiceUser serviceUser)
        {
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));
            return Json(serviceUser);
        }
	}

	/// <summary>
	/// ViewModel for the UserManagement class
	/// </summary>
	public class UserManagementViewModel
	{
	    private static IRepository _repository;

	    public IList<KeyValuePair<int, string>> Units { get; set; }
	    public IList<KeyValuePair<int, string>> Roles { get; set; }

	    public List<UserShowModel> UserShowModel { get; set; }

		public static UserManagementViewModel Create(IRepository repository)
		{
			Check.Require(repository != null, "Repository must be supplied");
            _repository = repository;
	
			var viewModel = new UserManagementViewModel();
 
			return viewModel;
		}

	    public void ConvertToUserShowModel(List<User> users, string application)
	    {
            var userIds = users.Select(x => x.Id).ToArray();

	        //Pull down all the role names for these users
            var roles = (from p in _repository.OfType<Permission>().Queryable
                        where p.Application.Name == application
                              && userIds.Contains(p.User.Id)
                        select new { UserId = p.User.Id, RoleName = p.Role.Name }).ToList();

            //Now all the units for these users
            var units = (from p in _repository.OfType<UnitAssociation>().Queryable
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
