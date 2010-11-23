using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace Catbert4.Services
{
    public class WebPrincipal : IPrincipal
    {
        public bool IsInRole(string role)
        {
            return Roles.IsUserInRole(role);
        }

        public IIdentity Identity
        {
            get { return HttpContext.Current.User.Identity; }
        }
    }
}