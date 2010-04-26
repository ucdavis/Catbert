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
        public static User InsertNewUser(User user, string trackingUserName)
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

            return user; //return the newly created user's ID
        }

        /// <summary>
        /// Ensures that the user given has the desired role and unit.
        /// If the user doesn't exist, it is created, and the role and units are associated if they were not already.
        /// </summary>
        /// <returns>the ID of the user</returns>
        public static User InsertNewUserWithRoleAndUnit(User userInformation, string role, string unit, string application, string trackingUserName)
        {
            if (userInformation == null || string.IsNullOrEmpty(role) || string.IsNullOrEmpty(unit) || string.IsNullOrEmpty(application) ) throw new ArgumentException();

            User user = new User();

            if (VerifyUserExists(userInformation.LoginID)) //Either get the existing user or create a new one
            {
                user = GetUser(userInformation.LoginID);
            }
            else
            {
                user = InsertNewUser(userInformation, trackingUserName); 
            }

            PermissionBLL.InsertPermission(application, role, user.LoginID, trackingUserName); //Insert the permission
            AssociateUnit(user.LoginID, unit, trackingUserName); //Associate the unit

            return user;
        }

        /// <summary>
        /// Returns true if the user exists in the database
        /// </summary>
        public static bool VerifyUserExists(string login)
        {
            return Queryable.Where(user => user.LoginID == login).Any();
        }

        /// <summary>
        /// Set the email address of the user indicated by login
        /// </summary>
        /// <returns>False if the user doesn't exist or the phone number is poorly formatted</returns>
        public static bool SetEmail(string login, string email, string trackingUserName)
        {
            //First get the user identified by the login
            var user = Queryable.Where(usr => usr.LoginID == login).SingleOrDefault();

            if (user == null) return false;

            //Set the email
            user.Email = email;

            //Check to see if the user is valid, if not return false
            if (!ValidateBO<User>.isValid(user)) return false;

            //We have a valid user, so get the tracking info and save
            Tracking tracking = TrackingBLL.GetTrackingInstance(login, TrackingTypes.User, TrackingActions.Change);
            tracking.Comments = string.Format("Email changed to {0} for user {1}", user.Email, user.ID);

            using (var ts = new TransactionScope())
            {
                EnsurePersistent(ref user);
                TrackingBLL.EnsurePersistent(ref tracking);

                ts.CommittTransaction();
            }

            return true;
        }

        /// <summary>
        /// Set the phone number of the user indicated by login.
        /// </summary>
        /// <returns>False if the user doesn't exist or the phone number is poorly formatted</returns>
        public static bool SetPhone(string login, string phone, string trackingUserName)
        {
            //First get the user identified by the login
            var user = Queryable.Where(usr => usr.LoginID == login).SingleOrDefault();

            if (user == null) return false;

            //Set the email
            user.Phone = phone;

            //Check to see if the user is valid, if not return false
            if (!ValidateBO<User>.isValid(user)) return false;

            //We have a valid user, so get the tracking info and save
            Tracking tracking = TrackingBLL.GetTrackingInstance(login, TrackingTypes.User, TrackingActions.Change);
            tracking.Comments = string.Format("Phone changed to {0} for user {1}", user.Email, user.ID);

            using (var ts = new TransactionScope())
            {
                EnsurePersistent(ref user);
                TrackingBLL.EnsurePersistent(ref tracking);

                ts.CommittTransaction();
            }

            return true;
        }

        #region Applications
        
        public static List<User> GetByApplication(string application)
        {
            return GetUserQueryableByApplication(application).ToList();
        }

        private static IEnumerable<User> GetUserQueryableByApplication(string application)
        {
            //Grab all permissions in this application
            var permissions = PermissionBLL.Queryable.Where(perm => perm.Application.Name == application && perm.Inactive == false).ToList();

            //Now get all users among these permissions
            return permissions.Select(perm => perm.User).Distinct();
        }

        public static List<User> GetByApplication(string application, int page, int pageSize, ref int totalUsers)
        {
            IEnumerable<User> users = GetUserQueryableByApplication(application);

            totalUsers = users.Count();

            //Now take/skip to get the correct users
            users = users.Skip((page - 1) * pageSize).Take(pageSize);

            return users.ToList();
        }

        /// <summary>
        /// Gets all users who are in the given application and role.
        /// </summary>
        /// <remarks>Can be improved once joins are possible</remarks>
        public static List<User> GetByApplicationRole(string application, string role)
        {
            //Grab all 'role' permissions in this application
            var permissions = PermissionBLL.Queryable.Where(perm => perm.Application.Name == application && perm.Role.Name == role && perm.Inactive == false).ToList();

            //Now get all users among these perms
            var users = permissions.Select(perm => perm.User).Distinct();

            return users.ToList();
        }
 
        #endregion

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
