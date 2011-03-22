using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Routing;

namespace Catbert4
{
    public class RouteConfigurator
    {
        public virtual void RegisterRoutes()
        {
            RouteCollection routes = RouteTable.Routes;
            routes.Clear();
            
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            routes.MapRoute("UserManagement", "UserManagement/{action}/{application}",
                            defaults:
                                new
                                    {
                                        controller = "UserManagement",
                                        action = "Manage",
                                        application = UrlParameter.Optional
                                    });

            routes.MapRoute("Default", "{controller}/{action}/{id}",
                            defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional});
        }
    }
}