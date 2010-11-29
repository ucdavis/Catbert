using System.Diagnostics.Contracts;
using System.Linq;
using Catbert4.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using System.Security.Principal;

namespace Catbert4.Services
{
    public interface IUserService
    {
        User GetUser();
        User GetUser(string login);
        bool UserExists(string login);
    }

    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IPrincipal _principal;

        public UserService(IRepository<User> userRepository, IPrincipal principal)
        {
            _userRepository = userRepository;
            _principal = principal;
        }

        public User GetUser()
        {
            Contract.Requires(_principal.Identity.IsAuthenticated);

            return GetUser(_principal.Identity.Name);
        }

        public User GetUser(string login)
        {
            Contract.Requires(login != null);

            return _userRepository.Queryable.Where(x => x.LoginId == login).Single();
        }

        public bool UserExists(string login)
        {
            Contract.Requires(login != null);

            return _userRepository.Queryable.Any(x => x.LoginId == login);
        }
    }
}