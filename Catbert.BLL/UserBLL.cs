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
        public static int InsertNewUser(User user, string trackingUserName)
        {
            //Make sure the user given in valid according to enlib validation
            if (!ValidateBO<User>.isValid(user)) throw new ApplicationException(string.Format("User not valid: {0}", ValidateBO<User>.GetValidationResultsAsString(user)));

            //If so, make sure there are no other users in the DB with the given login
            if (Queryable.Where(u => u.LoginID == user.LoginID).Any()) throw new ApplicationException(string.Format("User creation failed: LoginID already exists"));

            //Since we have a new user, assign defaults and insert
            user.Inactive = false;
            user.UserKey = Guid.NewGuid();

            Tracking tracking = TrackingBLL.GetTrackingInstance(trackingUserName, TrackingTypes.User, TrackingActions.Add);
            tracking.Comments = string.Format("User {0} added", user.LoginID);

            using (var ts = new TransactionScope())
            {
                EnsurePersistent(ref user);
                TrackingBLL.EnsurePersistent(ref tracking);

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

        public static List<User> GetByApplication(string application)
        {
            //Grab all permissions in this application
            var permissions = PermissionBLL.Queryable.Where(perm => perm.Application.Name == application && perm.Inactive == false).ToList();

            //Now get all users among these permissions
            var users = permissions.Select(perm => perm.User).Distinct();

            return users.ToList();
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

        #region Units

        /// <summary>
        /// Associate the unit identified by unitFIS 
        /// </summary>
        public static bool AssociateUnit(string login, string unitFIS, string trackingUserName)
        {
            //Get the user and unit and make sure they exist
            User user = UserBLL.GetUser(login);
            Unit unit = UnitBLL.GetByFIS(unitFIS);

            if (user == null || unit == null) return false;

            //Check to see if there is already an association between this unit and this unit
            bool userUnitExists = user.Units.Contains(unit);

            if (userUnitExists) return false;

            //Add the unit to the user
            user.Units.Add(unit);

            Tracking tracking = TrackingBLL.GetTrackingInstance(trackingUserName, TrackingTypes.User, TrackingActions.Change);
            tracking.Comments = string.Format("Unit {0} associated with user {1}", unit.ID, user.ID);

            using (var ts = new TransactionScope())
            {
                EnsurePersistent(ref user);
                TrackingBLL.EnsurePersistent(ref tracking);

                ts.CommittTransaction();
            }

            return true;
        }

        /// <summary>
        /// Unassociate the identified unit
        /// </summary>
        public static bool UnassociateUnit(string login, string unitFIS, string trackingUserName)
        {
            //Get the user and unit and make sure they exist
            User user = UserBLL.GetUser(login);
            Unit unit = UnitBLL.GetByFIS(unitFIS);

            if (user == null || unit == null) return false;

            //Check to see if there is already an association between this unit and this unit
            bool userUnitExists = user.Units.Contains(unit);

            if (!userUnitExists) return false; //If there isn't, return false

            user.Units.Remove(unit); //remove the unit association

            Tracking tracking = TrackingBLL.GetTrackingInstance(trackingUserName, TrackingTypes.User, TrackingActions.Change);
            tracking.Comments = string.Format("Unit {0} unassociated from user {1}", unit.ID, user.ID);

            using (var ts = new TransactionScope())
            {
                EnsurePersistent(ref user);
                TrackingBLL.EnsurePersistent(ref tracking);

                ts.CommittTransaction();
            }

            return true;
        }

        #endregion

    }
}
