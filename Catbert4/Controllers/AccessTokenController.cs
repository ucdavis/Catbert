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
    /// Controller for the AccessToken class
    /// </summary>
    public class AccessTokenController : ApplicationControllerBase
    {
	    private readonly IRepository<AccessToken> _accessTokenRepository;

        public AccessTokenController(IRepository<AccessToken> accessTokenRepository)
        {
            _accessTokenRepository = accessTokenRepository;
        }
    
        //
        // GET: /AccessToken/
        public ActionResult Index()
        {
            var applicationtokenList = _accessTokenRepository.Queryable
                .OrderBy(x=>x.Active)
                .ThenBy(x=>x.Application.Name)
                .ThenBy(x=>x.ContactEmail)
                .Fetch(x=>x.Application);

            return View(applicationtokenList.ToList());
        }

        //
        // GET: /School/Details/5
        public ActionResult Details(int id)
        {
            var accessToken =
                _accessTokenRepository.Queryable
                .Where(x => x.Id == id)
                .Fetch(x => x.Application).SingleOrDefault();
            
            if (accessToken == null) return RedirectToAction("Index");
            
            return View(accessToken);
        }

        //
        // GET: /AccessToken/Create
        public ActionResult Create()
        {
			var viewModel = AccessTokenViewModel.Create(Repository);
            
            return View(viewModel);
        } 

        //
        // POST: /AccessToken/Create
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(AccessToken accessToken)
        {
            var accessTokenToCreate = new AccessToken();

            Mapper.Map(accessToken, accessTokenToCreate);

            accessTokenToCreate.Application = accessToken.Application;
            accessTokenToCreate.SetNewToken();

            accessTokenToCreate.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                _accessTokenRepository.EnsurePersistent(accessTokenToCreate);

                Message = "AccessToken Created Successfully";
                
                return RedirectToAction("Details", new { id = accessTokenToCreate.Id });
            }
            else
            {
				var viewModel = AccessTokenViewModel.Create(Repository);
                viewModel.AccessToken = accessToken;

                return View(viewModel);
            }
        }
        
        //
        // GET: /AccessToken/Delete/5 
        public ActionResult Delete(int id)
        {
			var applicationtoken = _accessTokenRepository.GetNullableById(id);

            if (applicationtoken == null) return RedirectToAction("Index");

            return View(applicationtoken);
        }

        //
        // POST: /AccessToken/Delete/5
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int id, AccessToken applicationtoken)
        {
			var applicationtokenToDelete = _accessTokenRepository.GetNullableById(id);

            if (applicationtokenToDelete == null) return RedirectToAction("Index");

            _accessTokenRepository.Remove(applicationtokenToDelete);

            Message = "AccessToken Removed Successfully";

            return RedirectToAction("Index");
        }
        
        /// <summary>
        /// Transfer editable values from source to destination
        /// </summary>
        private static void TransferValues(AccessToken source, AccessToken destination)
        {
            throw new NotImplementedException();
        }

    }

	/// <summary>
    /// ViewModel for the AccessToken class
    /// </summary>
    public class AccessTokenViewModel
	{
		public AccessToken AccessToken { get; set; }
	    public List<Application> Applications { get; set; }

		public static AccessTokenViewModel Create(IRepository repository)
		{
			Check.Require(repository != null, "Repository must be supplied");
			
			var viewModel = new AccessTokenViewModel
			                    {
			                        AccessToken = new AccessToken(),
			                        Applications = repository.OfType<Application>().Queryable.OrderBy(x => x.Name).ToList()
			                    };

		    return viewModel;
		}
	}
}
