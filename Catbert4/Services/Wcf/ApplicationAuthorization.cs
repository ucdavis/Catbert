using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Linq;
using Catbert4.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using Microsoft.Practices.ServiceLocation;

namespace Catbert4.Services.Wcf
{
    public class ApplicationAuthorizationValidator : UserNamePasswordValidator
    {
        /// <summary>
        /// Checks if the given password hash allows the user to access the given application
        /// </summary>
        /// <param name="userName">The application name</param>
        /// <param name="password">The access token password hash</param>
        public override void Validate(string userName, string password)
        {
            var appName = userName;

            var givenTokenExists = (from a in RepositoryFactory.AccessTokenRepository.Queryable
                                   where a.Active 
                                        && a.Application.Name == appName
                                         && a.Token == password
                                   select a).Any();

            if (givenTokenExists == false)
            {
                throw new SecurityTokenException(string.Format("Password token does not grant access to {0}", userName));
            }
        }

        public class RepositoryFactory
        {

            // Private constructor prevents instantiation from other classes
            private RepositoryFactory() { }

            /**
            * SingletonHolder is loaded on the first execution of RepositoryFactory.[Property] 
            * or the first access to SingletonHolder.Instance, not before.
            */
            private static class SingletonHolder
            {
                public static readonly IRepository<AccessToken> Instance = ServiceLocator.Current.GetInstance<IRepository<AccessToken>>();
            }

            public static IRepository<AccessToken> AccessTokenRepository
            {
                get
                {
                    return SingletonHolder.Instance;
                }
            }

        }
    }
}