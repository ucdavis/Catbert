using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAESDO.Catbert.Core.Domain;

namespace CAESDO.Catbert.BLL
{
    public class UserBLL : GenericBLL<User, int>
    {
        /// <summary>
        /// Get a user by login
        /// </summary>
        public static User GetUser(string login)
        {
            return GetByProperty("LoginID", login);
        }

        public static List<User> GetByApplicationRole(int applicationID, int roleID)
        {
            var query = from user in Queryable
                        join perm in PermissionBLL.Queryable on user.ID equals perm.ID
                        where perm.Application.ID == applicationID && perm.Role.ID == roleID
                        select user;

            return query.ToList();
        }
    }
}
