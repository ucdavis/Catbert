using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Catbert4.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Controller;
using UCDArch.Web.Helpers;
using UCDArch.Core.Utils;

namespace Catbert4.Controllers
{
    /// <summary>
    /// Controller for the School class
    /// </summary>
    public class SchoolController : ApplicationControllerBase
    {
        private readonly IRepositoryWithTypedId<School, string> _schoolRepository;
        private readonly IRepository<Unit> _unitRepository;

        public SchoolController(IRepositoryWithTypedId<School, string> schoolRepository, IRepository<Unit> unitRepository)
        {
            _schoolRepository = schoolRepository;
            _unitRepository = unitRepository;
        }

        //
        // GET: /School/
        public ActionResult Index()
        {
            var schoolList = _schoolRepository.Queryable;

            return View(schoolList.ToList());
        }

        //
        // GET: /School/Details/5
        public ActionResult Details(string id)
        {
            var school = _schoolRepository.GetNullableById(id);

            if (school == null) return RedirectToAction("Index");

            var unitsForSchool = _unitRepository.Queryable.Where(x => x.School.Id == id).ToList();

            var model = SchoolViewModel.Create(Repository);
            model.School = school;
            model.Units = unitsForSchool;
            
            return View(model);
        }

        //
        // GET: /School/Create
        public ActionResult Create()
        {
			var viewModel = SchoolViewModel.Create(Repository);
            
            return View(viewModel);
        } 

        //
        // POST: /School/Create
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(School school)
        {
            var schoolToCreate = new School();

            Mapper.Map(school, schoolToCreate);

            schoolToCreate.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                _schoolRepository.EnsurePersistent(schoolToCreate);

                Message = "School Created Successfully";

                return RedirectToAction("Index");
            }
            else
            {
				var viewModel = SchoolViewModel.Create(Repository);
                viewModel.School = school;

                return View(viewModel);
            }
        }

        //
        // GET: /School/Edit/5
        public ActionResult Edit(string id)
        {
            var school = _schoolRepository.GetNullableById(id);

            if (school == null) return RedirectToAction("Index");

			var viewModel = SchoolViewModel.Create(Repository);
			viewModel.School = school;

			return View(viewModel);
        }
        
        //
        // POST: /School/Edit/5
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(string id, School school)
        {
            var schoolToEdit = _schoolRepository.GetNullableById(id);

            if (schoolToEdit == null) return RedirectToAction("Index");

            Mapper.Map(school, schoolToEdit);
            schoolToEdit.SetId(id);
            
            schoolToEdit.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                _schoolRepository.EnsurePersistent(schoolToEdit);

                Message = "School Edited Successfully";

                return RedirectToAction("Index");
            }
            else
            {
				var viewModel = SchoolViewModel.Create(Repository);
                viewModel.School = school;

                return View(viewModel);
            }
        }
        
        //
        // GET: /School/Delete/5 
        public ActionResult Delete(string id)
        {
			var school = _schoolRepository.GetNullableById(id);

            if (school == null) return RedirectToAction("Index");

            //Can't delete school if any units are assocaited with it
            if (_unitRepository.Queryable.Any(x => x.School.Id == id))
            {
                Message = string.Format("Can't remove {0} ({1}) because there are units associated with it", school.ShortDescription, school.Id);
                return RedirectToAction("Index");
            }

            return View(school);
        }

        //
        // POST: /School/Delete/5
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string id, School school)
        {
			var schoolToDelete = _schoolRepository.GetNullableById(id);

            if (schoolToDelete == null) return RedirectToAction("Index");

            _schoolRepository.Remove(schoolToDelete);

            Message = "School Removed Successfully";

            return RedirectToAction("Index");
        }
    }

	/// <summary>
    /// ViewModel for the School class
    /// </summary>
    public class SchoolViewModel
	{
		public School School { get; set; }
	    public List<Unit> Units { get; set; }
 
		public static SchoolViewModel Create(IRepository repository)
		{
			Check.Require(repository != null, "Repository must be supplied");
			
			var viewModel = new SchoolViewModel {School = new School()};
 
			return viewModel;
		}
	}
}
