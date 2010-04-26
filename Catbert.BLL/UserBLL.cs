using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAESDO.Catbert.Core.Domain;
using CAESDO.Catbert.Data;

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

        /// <summary>
        /// Returns a user's ID given their login
        /// </summary>
        public static int GetID(string login)
        {
            return Queryable.Where(user => user.LoginID == login).Select(user => user.ID).Single();
        }

        /// <summary>
        /// Insert a new user into the database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static int InsertNewUser(User user)
        {
            //Make sure the user given in valid according to enlib validation
            if (!ValidateBO<User>.isValid(user)) throw new ApplicationException(string.Format("User not valid: {0}", ValidateBO<User>.GetValidationResultsAsString(user)));

            //If so, make sure there are no other users in the DB with the given login
            if (Queryable.Where(u => u.LoginID == user.LoginID).Any()) throw new ApplicationException(string.Format("User creation failed: LoginID already exists"));

            //Since we have a new user, assign defaults and insert
            user.Inactive = false;
            user.UserKey = Guid.NewGuid();

            using (var ts = new TransactionScope())
            {
                EnsurePersistent(ref user);

                ts.CommittTransaction();
            }

            return user.ID; //return the newly created user's ID
        }

        /// <summary>
        /// Returns true if the user exists in the database
        /// </summary>
        public static bool VerifyUserExists(string login)
        {
            return Queryable.Where(user => user.LoginID == login).Any();
        }

        public static List<User> GetByApplicationRole(int applicationID, int roleID)
        {
            //Can't implement through LINQ because of restrictions on joins.
            throw new NotImplementedException();

            /*
            var query = from perm in PermissionBLL.Queryable
                        where perm.Application.ID == applicationID && perm.Role.ID == roleID
                        select perm.User;
            */

            /*
            var query = from user in Queryable
                        join perm in PermissionBLL.Queryable on user.ID equals perm.ID
                        where perm.Application.ID == applicationID && perm.Role.ID == roleID
                        select user;

            return query.ToList();
             */
        }
    }
}
