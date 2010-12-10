using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Catbert4.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Helpers;
using UCDArch.Core.Utils;

namespace Catbert4.Controllers
{
    /// <summary>
    /// Controller for the Unit class
    /// </summary>
    public class UnitController : ApplicationControllerBase
    {
	    private readonly IRepository<Unit> _unitRepository;
        private readonly IRepository<UnitAssociation> _unitAssociationRepository;

        public UnitController(IRepository<Unit> unitRepository, IRepository<UnitAssociation> unitAssociationRepository)
        {
            _unitRepository = unitRepository;
            _unitAssociationRepository = unitAssociationRepository;
        }

        //
        // GET: /Unit/
        public ActionResult Index()
        {
            var unitList = _unitRepository.Queryable.OrderBy(x=>x.FullName).Fetch(x=>x.School);

            return View(unitList.ToList());
        }

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

            Mapper.Map(unit, unitToCreate);
            
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

            //Can not delete if there are any unit associations related to this unit
            if (_unitAssociationRepository.Queryable.Any(x => x.Unit.Id == id))
            {
                Message = string.Format("Can not remove {0} ({1}) because it is associated with existing users", unit.ShortName, unit.FisCode);
                return RedirectToAction("Index");
            }
            
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
