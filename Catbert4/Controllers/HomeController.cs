using System.Linq;
using System.ServiceModel;
using System.Web.Mvc;
using Catbert4.Providers;
using Catbert4.Services.UserManagement;
using Catbert4.Services.Wcf;
using NHibernate.Criterion;
using NHibernate.Linq;
using UCDArch.Web.Attributes;
using Catbert4.Core.Domain;
using System.Web.Security;
using IRoleService = Catbert4.Services.Wcf.IRoleService;
using UCDArch.Data.NHibernate;
using Microsoft.Practices.ServiceLocation;

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
            var unitService = ServiceLocator.Current.GetInstance<IUnitService>();
            var roleService = ServiceLocator.Current.GetInstance<Services.UserManagement.IRoleService>();
            
            var units = unitService.GetVisibleByUser("postit", "HelpRequest");
            units.ToList();

            var roles = roleService.GetVisibleByUser("HelpRequest", "postit");
            var result = roles.ToList();

            /*
            var levels = from p in Repository.OfType<Permission>().Queryable
                         join a in Repository.OfType<ApplicationRole>().Queryable on new { Role = p.Role.Id, App = p.Application.Id }
                                 equals new { Role = a.Role.Id, App = a.Application.Id }
                         where p.Application.Name == "HelpRequest" &&
                             p.User.LoginId == "postit" &&
                             a.Level != null
                         select a.Level;

            var manageableRoles = from ar in Repository.OfType<ApplicationRole>().Queryable
                                  where ar.Application.Name == "HelpRequest" &&
                                        ar.Level > (levels.Max())
                                  select ar.Role;
            */

            //var manageableRoles = from ar in Repository.OfType<ApplicationRole>().Queryable
            //                      where ar.Application.Name == "HelpRequest" &&
            //                            ar.Level > (
            //                                           (from p in Repository.OfType<Permission>().Queryable
            //                                            join a in Repository.OfType<ApplicationRole>().Queryable on
            //                                                new {Role = p.Role.Id, App = p.Application.Id}
            //                                                equals new {Role = a.Role.Id, App = a.Application.Id}
            //                                            where p.Application.Name == "HelpRequest" &&
            //                                                  p.User.LoginId == "postit" &&
            //                                                  a.Level != null
            //                                            select a.Level).Max()
            //                                       )
            //                      select ar.Role;
            
            
            //var uaRepo = Repository.OfType<UnitAssociation>().Queryable;
            //var units = Repository.OfType<Unit>().GetAll();
            //var schools = Repository.OfType<School>().GetAll();
            //schools.First().Units.Count();
            /*
            var q = (from s in Repository.OfType<School>().Queryable
                join u in Repository.OfType<Unit>().Queryable on s.Id equals u.School.Id
                where u.UnitAssociations.Any(x => x.Application.Name == "HelpRequest" && x.User.LoginId == "postit")
                select s).SelectMany(x => x.Units, (x, y) => y);

            var result = q.ToList();
            */
            return Content("");
        }

        private string GetAbsoluteUrl(string relative)
        {
            return string.Format("http://{0}:{1}{2}", Request.Url.Host, Request.Url.Port, Url.Content(relative));
        }
    }
}
