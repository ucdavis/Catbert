using System.Linq;
using System.ServiceModel;
using System.Web.Mvc;
using Catbert4.Providers;
using Catbert4.Services.UserManagement;
using Catbert4.Services.Wcf;
using NHibernate.Criterion;
using UCDArch.Web.Attributes;
using Catbert4.Core.Domain;
using System.Web.Security;
using IRoleService = Catbert4.Services.Wcf.IRoleService;
using UCDArch.Data.NHibernate;

namespace Catbert4.Controllers
{
    public class HomeController : ApplicationControllerBase
    {
        [HandleTransactionsManually]
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

        public ActionResult ServiceTests()
        {
            var messageService = new ChannelFactory<IMessageService>(new BasicHttpBinding(), GetAbsoluteUrl("~/Public/Message.svc"));
            var roleService = new ChannelFactory<IRoleService>(new BasicHttpBinding(), GetAbsoluteUrl("~/Public/Role.svc"));

            var messageProxy = messageService.CreateChannel();

            var message = messageProxy.GetMessages("AD419");
            
            messageService.Close();

            
            var provider = new CatbertServiceRoleProvider();
            provider.InitWithoutConfig("Catbert", GetAbsoluteUrl("~/Public/Role.svc"), "fake");

            var roles = provider.GetAllRoles();

            var q = Roles.GetAllRoles();
            var q2 = Roles.GetUsersInRole("Admin");
            
            return Content("");
        }

        public  ActionResult UserManagementTests()
        {
            var unitService = new UnitService();

            var units = unitService.GetVisibleByUser("postit", "HelpRequest");

            return Content("");
        }

        private string GetAbsoluteUrl(string relative)
        {
            return string.Format("http://{0}:{1}{2}", Request.Url.Host, Request.Url.Port, Url.Content(relative));
        }
    }
}
