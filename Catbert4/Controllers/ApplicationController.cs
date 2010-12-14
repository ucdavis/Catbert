using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Catbert4.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Helpers;
using UCDArch.Core.Utils;
using AutoMapper;

namespace Catbert4.Controllers
{
    /// <summary>
    /// Controller for the Application class
    /// </summary>
    public class ApplicationController : ApplicationControllerBase
    {
	    private readonly IRepository<Application> _applicationRepository;
        private readonly IRepository<Role> _roleRepository;

        public ApplicationController(IRepository<Application> applicationRepository, IRepository<Role> roleRepository)
        {
            _applicationRepository = applicationRepository;
            _roleRepository = roleRepository;
        }

        //
        // GET: /Application/
        public ActionResult Index()
        {
            var applicationList = _applicationRepository.Queryable.OrderBy(x=>x.Name);

            return View(applicationList.ToList());
        }

        //
        // GET: /Application/Details/5
        public ActionResult Details(int id)
        {
            var application = _applicationRepository.GetNullableById(id);
            
            if (application == null) return RedirectToAction("Index");
            
            application.ApplicationRoles.ToList();

            return View(application);
        }

        //
        // GET: /Application/Create
        public ActionResult Create()
        {
			var viewModel = ApplicationViewModel.Create(Repository);
            
            return View(viewModel);
        } 

        //
        // POST: /Application/Create
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(ApplicationEditModel applicationEditModel)
        {
            var applicationToCreate = new Application();

            Mapper.Map(applicationEditModel.Application, applicationToCreate);
            
            SetApplicationRoles(applicationToCreate, applicationEditModel.OrderedRoles, applicationEditModel.UnorderedRoles);

            applicationToCreate.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                _applicationRepository.EnsurePersistent(applicationToCreate);

                Message = "Application Created Successfully";

                return Json(new { success = true });
            }
            else
            {
				var viewModel = ApplicationViewModel.Create(Repository);
                viewModel.Application = applicationEditModel.Application;

                return View(viewModel);
            }
        }

        //
        // GET: /Application/Edit/5
        public ActionResult Edit(int id)
        {
            var application = _applicationRepository.GetNullableById(id);

            if (application == null) return RedirectToAction("Index");
            application.ApplicationRoles.ToList();

			var viewModel = ApplicationViewModel.Create(Repository);
			viewModel.Application = application;

			return View(viewModel);
        }
        
        //
        // POST: /Application/Edit/5
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, ApplicationEditModel applicationEditModel)
        {
            var applicationToEdit = _applicationRepository.GetNullableById(id);

            if (applicationToEdit == null) return RedirectToAction("Index");

            Mapper.Map(applicationEditModel.Application, applicationToEdit);

            SetApplicationRoles(applicationToEdit, applicationEditModel.OrderedRoles,
                                applicationEditModel.UnorderedRoles);

            applicationToEdit.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                _applicationRepository.EnsurePersistent(applicationToEdit);

                Message = "Application Edited Successfully";

                return Json(new {success = true});
            }
            else
            {
				var viewModel = ApplicationViewModel.Create(Repository);
                viewModel.Application = applicationEditModel.Application;

                return View(viewModel);
            }
        }

        private void SetApplicationRoles(Application application, List<string> orderedRoles, List<string> unorderedRoles)
        {
            //Remove all of the current application roles
            application.ApplicationRoles.Clear();

            //Get the roles that we are going to need all at once
            var roles = (from r in _roleRepository.Queryable
                                           where (orderedRoles.Contains(r.Name) || unorderedRoles.Contains(r.Name))
                                           select r).ToList();
            
            //Now go through the leveled roles and add them in order to the applicationRoles object
            for (var i = 0; i < orderedRoles.Count; i++)
            {
                int index = i;
                application.ApplicationRoles.Add(new ApplicationRole
                {
                    Application = application,
                    Role = roles.Single(r => r.Name == orderedRoles[index]),
                    Level = i + 1
                    //The level is the current index plus one, so that they start at 1
                });
            }

            //Now add the non-leveled roles
            foreach (string role in unorderedRoles)
            {
                string roleName = role;
                application.ApplicationRoles.Add(new ApplicationRole()
                {
                    Application = application,
                    Role = roles.Single(r => r.Name == roleName),
                    Level = null //No level for these
                });
            }

            //Now we should have an application with reconciled roles
        }

    }
    
    public class ApplicationEditModel
    {
        public Application Application { get; set; }
        public List<string> OrderedRoles { get; set; }
        public List<string> UnorderedRoles { get; set; }
    }

	/// <summary>
    /// ViewModel for the Application class
    /// </summary>
    public class ApplicationViewModel
	{
		public Application Application { get; set; }
	    public List<Role> Roles { get; set; }

		public static ApplicationViewModel Create(IRepository repository)
		{
			Check.Require(repository != null, "Repository must be supplied");
			
			var viewModel = new ApplicationViewModel
			                    {
			                        Application = new Application(),
			                        Roles = repository.OfType<Role>().Queryable.OrderBy(x=>x.Name).ToList()
			                    };
            
		    return viewModel;
		}

        /// <summary>
        /// Returns all roles that are not already in the application's roles
        /// </summary>
        public IEnumerable<Role> GetAvailableRoles()
        {
            Check.Require(Application != null);

            var applicationRoleIds = Application.ApplicationRoles.Select(x => x.Role.Id);

            return Roles.Where(x => !applicationRoleIds.Contains(x.Id));
        }
	}
}
