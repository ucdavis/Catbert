using System;
using System.Linq;
using System.Web.Mvc;
using Catbert4.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Helpers;
using UCDArch.Core.Utils;

namespace Catbert4.Controllers
{
    /// <summary>
    /// Controller for the Application class
    /// </summary>
    public class ApplicationController : ApplicationControllerBase
    {
	    private readonly IRepository<Application> _applicationRepository;

        public ApplicationController(IRepository<Application> applicationRepository)
        {
            _applicationRepository = applicationRepository;
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
        public ActionResult Create(Application application)
        {
            var applicationToCreate = new Application();

            TransferValues(application, applicationToCreate);

            applicationToCreate.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                _applicationRepository.EnsurePersistent(applicationToCreate);

                Message = "Application Created Successfully";

                return RedirectToAction("Index");
            }
            else
            {
				var viewModel = ApplicationViewModel.Create(Repository);
                viewModel.Application = application;

                return View(viewModel);
            }
        }

        //
        // GET: /Application/Edit/5
        public ActionResult Edit(int id)
        {
            var application = _applicationRepository.GetNullableById(id);

            if (application == null) return RedirectToAction("Index");

			var viewModel = ApplicationViewModel.Create(Repository);
			viewModel.Application = application;

			return View(viewModel);
        }
        
        //
        // POST: /Application/Edit/5
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, Application application)
        {
            var applicationToEdit = _applicationRepository.GetNullableById(id);

            if (applicationToEdit == null) return RedirectToAction("Index");

            TransferValues(application, applicationToEdit);

            applicationToEdit.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                _applicationRepository.EnsurePersistent(applicationToEdit);

                Message = "Application Edited Successfully";

                return RedirectToAction("Index");
            }
            else
            {
				var viewModel = ApplicationViewModel.Create(Repository);
                viewModel.Application = application;

                return View(viewModel);
            }
        }
        
        //
        // GET: /Application/Delete/5 
        public ActionResult Delete(int id)
        {
			var application = _applicationRepository.GetNullableById(id);

            if (application == null) return RedirectToAction("Index");

            return View(application);
        }

        //
        // POST: /Application/Delete/5
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int id, Application application)
        {
			var applicationToDelete = _applicationRepository.GetNullableById(id);

            if (applicationToDelete == null) return RedirectToAction("Index");

            _applicationRepository.Remove(applicationToDelete);

            Message = "Application Removed Successfully";

            return RedirectToAction("Index");
        }
        
        /// <summary>
        /// Transfer editable values from source to destination
        /// </summary>
        private static void TransferValues(Application source, Application destination)
        {
            throw new NotImplementedException();
        }

    }

	/// <summary>
    /// ViewModel for the Application class
    /// </summary>
    public class ApplicationViewModel
	{
		public Application Application { get; set; }
 
		public static ApplicationViewModel Create(IRepository repository)
		{
			Check.Require(repository != null, "Repository must be supplied");
			
			var viewModel = new ApplicationViewModel {Application = new Application()};
 
			return viewModel;
		}
	}
}
