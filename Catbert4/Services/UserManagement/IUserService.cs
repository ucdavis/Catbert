using System.Collections.Generic;
using Catbert4.Core.Domain;

namespace Catbert4.Services.UserManagement
{
    public interface IUserService
    {
        List<User> GetByCriteria(string application, string searchToken, int page, int pageSize, string orderBy, out int totalUsers);
        List<User> GetByApplication(string application, string currentLogin, string role, string unit, string searchToken, int page, int pageSize, string orderBy, out int totalUsers);

        /// <summary>
        /// Returns true if the given current user has the proper permissions to be managing the loginToManage.
        /// </summary>
        bool CanUserManageGivenLogin(string application, string currentUserLogin, string loginToManage);
    }
}