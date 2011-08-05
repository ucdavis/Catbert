using System.Linq;
using System.Web.Mvc;
using Catbert4.Core.Domain;
using Catbert4.Services;
using UCDArch.Core.PersistanceSupport;
using Catbert4.Helpers;
using System;
using System.Web.Configuration;

namespace Catbert4.Controllers
{
    /// <summary>
    /// Controller for Handle Single SignOn (SSO) responsibilities
    /// </summary>
    public class SsoController : ApplicationControllerBase
    {
	    private readonly IRepository<User> _userRepository;
        private readonly IDirectorySearchService _directorySearchService;

        public SsoController(IRepository<User> userRepository, IDirectorySearchService directorySearchService)
        {
            _userRepository = userRepository;
            _directorySearchService = directorySearchService;
        }

        private static string UvRoot
        {
            get { return WebConfigurationManager.AppSettings["UserVoiceRoot"]; }
        }

        /// <summary>
        /// SSO for uservoice login.
        /// </summary>
        /// <param name="return">
        /// Page to be returned to after signin is successfull, relative to uservoice root
        /// </param>
        /// <returns>
        /// Return redirection with valid SSO token
        /// </returns>
        [Authorize] //Must be kerb authorized
        public ActionResult UvSignIn(string @return)
        {
            string encodedToken;

            var user = _userRepository.Queryable.Where(x=>x.LoginId == User.Identity.Name).SingleOrDefault();

            if (user == null)
            {
                //Can't find the user in catbert, use directory services to lookup info
                var directoryUser = _directorySearchService.FindUser(User.Identity.Name);

                if (directoryUser == null)
                {
                    //They aren't in ldap either?
                    throw new NotImplementedException("You are not found in Catbert or LDAP...");
                }
                else
                {
                    //TODO: What to do when email address is not found in ldap
                    encodedToken = UserVoiceTokenGenerator.Create(directoryUser.FullName ?? directoryUser.LoginId, directoryUser.EmailAddress,
                                                                  directoryUser.LoginId);
                }
            }
            else
            {
                //User is in catbert, generate sso token
                encodedToken = UserVoiceTokenGenerator.Create(user.FullName, user.Email, user.LoginId);
            }

            return Redirect(string.Format("{0}{1}?sso={2}", UvRoot, @return, encodedToken));
        }
    }
}
