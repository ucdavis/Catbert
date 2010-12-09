using System.Linq;
using System.Web.Mvc;
using UCDArch.Web.Attributes;
using Catbert4.Core.Domain;

namespace Catbert4.Controllers
{
    [HandleTransactionsManually]
    public class HomeController : ApplicationControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
        //
        // GET: /Home/
        public ActionResult Dev()
        {
            var apps = Repository.OfType<Application>().GetAll();
            var perm = Repository.OfType<Permission>().Queryable.Where(x => x.Inactive == false).First();
            var assoc = Repository.OfType<UnitAssociation>().Queryable.Where(x => x.Inactive == false).First();

            return Content(string.Format("There are {0} apps", apps.Count));
            //return View();
        }

    }
}
