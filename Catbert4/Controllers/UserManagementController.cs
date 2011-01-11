using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
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

	    public UserManagementController(IRepository<User> usermanagementRepository, 
            IUserService userService, 
            IUnitService unitService, 
            IRoleService roleService)
		{
		    _usermanagementRepository = usermanagementRepository;
		    _userService = userService;
	        _unitService = unitService;
	        _roleService = roleService;
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

		    model.Users = _userService.GetByApplication(application, CurrentUser.Identity.Name, role, unit).ToList();
    
			return View(model);
		}
	}

	/// <summary>
	/// ViewModel for the UserManagement class
	/// </summary>
	public class UserManagementViewModel
	{
	    public IList<KeyValuePair<int, string>> Units { get; set; }
	    public IList<KeyValuePair<int, string>> Roles { get; set; }

	    public List<User> Users { get; set; }

		public static UserManagementViewModel Create(IRepository repository)
		{
			Check.Require(repository != null, "Repository must be supplied");
			
			var viewModel = new UserManagementViewModel();
 
			return viewModel;
		}
	}
}
