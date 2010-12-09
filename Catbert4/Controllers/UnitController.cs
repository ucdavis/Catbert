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
using System.Diagnostics.Contracts;

namespace Catbert4.Controllers
{
    /// <summary>
    /// Controller for the Unit class
    /// </summary>
    public class UnitController : ApplicationControllerBase
    {
	    private readonly IRepository<Unit> _unitRepository;

        public UnitController(IRepository<Unit> unitRepository)
        {
            _unitRepository = unitRepository;
        }
    
        //
        // GET: /Unit/
        public ActionResult Index()
        {
            var unitList = _unitRepository.Queryable.OrderBy(x=>x.FullName).Fetch(x=>x.School);

            return View(unitList.ToList());
        }

/*
        // GET: /Unit/Create
        public ActionResult Create()
        {
			var viewModel = UnitViewModel.Create(Repository);
            
            return View(viewModel);
        } 

        //
        // POST: /Unit/Create
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Unit unit)
        {
            var unitToCreate = new Unit();

            TransferValues(unit, unitToCreate);

            unitToCreate.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                _unitRepository.EnsurePersistent(unitToCreate);

                Message = "Unit Created Successfully";

                return RedirectToAction("Index");
            }
            else
            {
				var viewModel = UnitViewModel.Create(Repository);
                viewModel.Unit = unit;

                return View(viewModel);
            }
        }
*/
        //
        // GET: /Unit/Edit/5
        public ActionResult Edit(int id)
        {
            var unit = _unitRepository.GetNullableById(id);

            if (unit == null) return RedirectToAction("Index");

			var viewModel = UnitViewModel.Create(Repository);
			viewModel.Unit = unit;

			return View(viewModel);
        }
        
        //
        // POST: /Unit/Edit/5
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, Unit unit)
        {
            var unitToEdit = _unitRepository.GetNullableById(id);

            if (unitToEdit == null) return RedirectToAction("Index");

            Mapper.Map(unit, unitToEdit);

            //TransferValues(unit, unitToEdit);

            unitToEdit.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                _unitRepository.EnsurePersistent(unitToEdit);

                Message = "Unit Edited Successfully";

                return RedirectToAction("Index");
            }
            else
            {
				var viewModel = UnitViewModel.Create(Repository);
                viewModel.Unit = unit;

                return View(viewModel);
            }
        }
        
        //
        // GET: /Unit/Delete/5 
        public ActionResult Delete(int id)
        {
			var unit = _unitRepository.GetNullableById(id);

            if (unit == null) return RedirectToAction("Index");

            return View(unit);
        }

        //
        // POST: /Unit/Delete/5
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int id, Unit unit)
        {
			var unitToDelete = _unitRepository.GetNullableById(id);

            if (unitToDelete == null) return RedirectToAction("Index");

            _unitRepository.Remove(unitToDelete);

            Message = "Unit Removed Successfully";

            return RedirectToAction("Index");
        }
        
        /// <summary>
        /// Transfer editable values from source to destination
        /// </summary>
        private static void TransferValues(Unit source, Unit destination)
        {
            destination.FisCode = source.FisCode;
            destination.PpsCode = source.PpsCode;
            destination.ShortName = source.ShortName;
            destination.FullName = source.FullName;
        }

    }

	/// <summary>
    /// ViewModel for the Unit class
    /// </summary>
    public class UnitViewModel
	{
		public Unit Unit { get; set; }
        public IList<School> Schools { get; set; }

		public static UnitViewModel Create(IRepository repository)
		{
			Check.Require(repository != null, "Repository must be supplied");
			
			var viewModel = new UnitViewModel
			                    {
			                        Unit = new Unit(),
			                        Schools = repository.OfType<School>().Queryable.OrderBy(x => x.ShortDescription).ToList()
			                    };

		    return viewModel;
		}
	}
}
