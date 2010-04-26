using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
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
            AssociateUnit(user.LoginID, application, unit, trackingUserName); //Associate the unit

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
            return GetUserQueryableByApplication(application, null, null, null).ToList();
        }

        private static IQueryable<User> GetUserQueryableByApplication(string application, string role, string unit, string searchToken)
        {
            //Grab all permissions in this application
            var permissions = PermissionBLL.Queryable.Where(perm => perm.Application.Name == application && perm.Inactive == false);

            if (!string.IsNullOrEmpty(role))
                permissions = permissions.Where(perm => perm.Role.Name == role && perm.Role.Inactive == false);

            //Now get all users among these permissions
            var users = permissions.ToList().Select(perm => perm.User).Distinct();
            
            if (string.IsNullOrEmpty(searchToken) == false)
            {
                searchToken = searchToken.ToLower(); //search should be lowercase

                users = users.Where(u => u.Email.ToLower().Contains(searchToken) || u.FirstName.ToLower().Contains(searchToken)
                    || u.LastName.ToLower().Contains(searchToken) || u.LoginID.ToLower().Contains(searchToken));
            }

            return users.AsQueryable<User>();
        }

        public static List<User> GetByApplication(string application, string role, string unit, string searchToken, int page, int pageSize, string orderBy, ref int totalUsers)
        {
            IQueryable<User> users = GetUserQueryableByApplication(application, role, unit, searchToken).OrderBy(orderBy);

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
        /// Associate the unit identified by unitFIS -- only within the context of the given application 
        /// </summary>
        /// <remarks>Deprecated -- use the unitAssociationBLL instead</remarks>
        public static bool AssociateUnit(string login, string application, string unitFIS, string trackingUserName)
        {
            return UnitAssociationBLL.AssociateUnit(login, application, unitFIS, trackingUserName);
        }

        /// <summary>
        /// Unassociate the identified unit
        /// </summary>
        /// <remarks>Deprecated -- use the unitAssociationBLL instead</remarks>
        public static bool UnassociateUnit(string login, string application, string unitFIS, string trackingUserName)
        {
            return UnitAssociationBLL.UnassociateUnit(login, application, unitFIS, trackingUserName);
        }

        #endregion

    }
}
