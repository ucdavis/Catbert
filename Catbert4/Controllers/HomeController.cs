using System.Web.Mvc;
using UCDArch.Web.Attributes;
using Catbert4.Core.Domain;

namespace Catbert4.Controllers
{
    [HandleTransactionsManually]
    public class HomeController : ApplicationController
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            var apps = Repository.OfType<Application>().GetAll();

            return Content(string.Format("There are {0} apps", apps.Count));
            //return View();
        }

    }
}
