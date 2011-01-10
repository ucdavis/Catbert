using System.Collections.Generic;
using System.Linq;
using Catbert4.Core.Domain;

namespace Catbert4.Services.UserManagement
{
    public interface IUserService
    {
        /// <summary>
        /// Return all of the users that match the given criteria, and who are visible to the current login
        /// </summary>
        IQueryable<User> GetByApplication(string application, string currentLogin, string role = null, string unit = null);

        /// <summary>
        /// Returns true if the given current user has the proper permissions to be managing the loginToManage.
        /// </summary>
        bool CanUserManageGivenLogin(string application, string currentUserLogin, string loginToManage);
    }
}