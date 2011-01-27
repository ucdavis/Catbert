using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web.Mvc;
using Catbert4.Models;
using Catbert4.Providers;
using Catbert4.Services.UserManagement;
using Catbert4.Services.Wcf;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Attributes;
using Catbert4.Core.Domain;
using System.Web.Security;
using IRoleService = Catbert4.Services.Wcf.IRoleService;
using Microsoft.Practices.ServiceLocation;
using UCDArch.Core.Utils;

namespace Catbert4.Controllers
{
    public class HomeController : ApplicationControllerBase
    {
        [HandleTransactionsManually]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserManagement()
        {
            var apps = Repository.OfType<Application>().Queryable
                .Where(x => x.Inactive == false)
                .OrderBy(x => x.Name);

            return View(apps.ToList());
        }

        public ActionResult MessageService(string baseUrl)
        {
            var serviceUrl = string.IsNullOrWhiteSpace(baseUrl) ? GetAbsoluteUrl("~/Public/Message.svc") : baseUrl + "/Public/Message.svc";
            ViewData["serviceUrl"] = serviceUrl;

            var binding = new BasicHttpBinding();

            if (serviceUrl.StartsWith("https://")) binding.Security.Mode = BasicHttpSecurityMode.Transport;

            var messageService = new ChannelFactory<IMessageService>(binding, serviceUrl);

            var messageProxy = messageService.CreateChannel();

            var messages = messageProxy.GetMessages(null);

            messageService.Close();

            return View(messages);
        }

        public ContentResult RoleProvider(string baseUrl, string token)
        {
            var serviceUrl = string.IsNullOrWhiteSpace(baseUrl) ? GetAbsoluteUrl("~/Public/Role.svc") : baseUrl + "/Public/Role.svc";

            var binding = new BasicHttpBinding();
            if (serviceUrl.StartsWith("https://")) binding.Security.Mode = BasicHttpSecurityMode.TransportWithMessageCredential;

            if (string.IsNullOrWhiteSpace(token))
            {
                token =
                Repository.OfType<AccessToken>().Queryable.Where(x => x.Application.Name == "Catbert" && x.Active).
                    Select(x => x.Token).FirstOrDefault();
            }

            Check.Require(token != null, "No access tokens for catbert found");

            var provider = new CatbertServiceRoleProvider();
            provider.InitWithoutConfig("Catbert", serviceUrl, token);

            string[] users = provider.GetUsersInRole("Admin");
            
            return Content("User count: " + users.Length);
        }

        public ActionResult RoleService(string baseUrl, string token)
        {
            var serviceUrl = string.IsNullOrWhiteSpace(baseUrl) ? GetAbsoluteUrl("~/Public/Role.svc") : baseUrl + "/Public/Role.svc";

            var binding = new BasicHttpBinding();
            if (serviceUrl.StartsWith("https://")) binding.Security.Mode = BasicHttpSecurityMode.TransportWithMessageCredential;

            if (string.IsNullOrWhiteSpace(token))
            {
                token =
                Repository.OfType<AccessToken>().Queryable.Where(x => x.Application.Name == "Catbert" && x.Active).
                    Select(x => x.Token).FirstOrDefault();
            }
            
            Check.Require(token != null, "No access tokens for catbert found");

            var client = new RoleServiceClient(binding, new EndpointAddress(serviceUrl));
            
            client.ClientCredentials.UserName.UserName = "Catbert";
            client.ClientCredentials.UserName.Password = token;

            string[] users;

            using (client)
            {
                users = client.GetUsersInRole("Catbert", "Admin");
            }

            return Content("User count: " + users.Length);
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
            var userService = ServiceLocator.Current.GetInstance<Services.UserManagement.IUserService>();
            
            //var units = unitService.GetVisibleByUser("HelpRequest", "postit");
            //units.ToList();
            
            //var roles = roleService.GetVisibleByUser("HelpRequest", "postit");
            //var result = roles.ToList();

            //var users = userService.GetByApplication("HelpRequest", "postit").ToList();
            
            return Content("");
        }

        private string GetAbsoluteUrl(string relative)
        {
            return string.Format("{0}://{1}:{2}{3}", Request.Url.Scheme, Request.Url.Host, Request.Url.Port, Url.Content(relative));
        }
    }
}
