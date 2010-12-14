using System;
using System.Linq;
using System.Web.Mvc;
using Catbert4.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Controller;
using UCDArch.Web.Helpers;
using UCDArch.Core.Utils;

namespace Catbert4.Controllers
{
    /// <summary>
    /// Controller for the Role class
    /// </summary>
    public class RoleController : ApplicationControllerBase
    {
	    private readonly IRepository<Role> _roleRepository;

        public RoleController(IRepository<Role> roleRepository)
        {
            _roleRepository = roleRepository;
        }
    
        //
        // GET: /Role/
        public ActionResult Index()
        {
            var roleList = _roleRepository.Queryable.OrderBy(x=>x.Name);

            return View(roleList.ToList());
        }

        //
        // GET: /Role/Create
        public ActionResult Create()
        {
			var viewModel = RoleViewModel.Create(Repository);
            
            return View(viewModel);
        } 

        //
        // POST: /Role/Create
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Role role)
        {
            var roleToCreate = new Role {Name = role.Name};

            //Make sure role name is not already in active use
            var existingActiveRoleName = _roleRepository
                                            .Queryable
                                            .Where(x => x.Name == role.Name && x.Inactive == false)
                                            .Any();

            if (existingActiveRoleName)
            {
                ModelState.AddModelError("Role.Name", "The rolename given already exists and is active");
            }

            roleToCreate.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                _roleRepository.EnsurePersistent(roleToCreate);

                Message = "Role Created Successfully";

                return RedirectToAction("Index");
            }
            else
            {
				var viewModel = RoleViewModel.Create(Repository);
                viewModel.Role = role;

                return View(viewModel);
            }
        }
    }

	/// <summary>
    /// ViewModel for the Role class
    /// </summary>
    public class RoleViewModel
	{
		public Role Role { get; set; }
 
		public static RoleViewModel Create(IRepository repository)
		{
			Check.Require(repository != null, "Repository must be supplied");
			
			var viewModel = new RoleViewModel {Role = new Role()};
 
			return viewModel;
		}
	}
}
