using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using CAESArch.BLL;
using CAESArch.Core.Utils;
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
            if (!ValidateBusinessObject<User>.IsValid(user))
                throw new ApplicationException(string.Format("User not valid: {0}",
                                                             ValidateBusinessObject<User>.GetValidationResultsAsString(
                                                                 user)));

            //If so, make sure there are no other users in the DB with the given login
            if (Queryable.Where(u => u.LoginID == user.LoginID).Any())
                throw new ApplicationException(string.Format("User creation failed: LoginID already exists"));

            //Since we have a new user, assign defaults and insert
            user.Inactive = false;
            user.UserKey = Guid.NewGuid();

            Tracking tracking = TrackingBLL.GetTrackingInstance(trackingUserName, TrackingTypes.User,
                                                                TrackingActions.Add);
            tracking.Comments = string.Format("User {0} added", user.LoginID);

            using (var ts = new TransactionScope())
            {
                EnsurePersistent(user);
                TrackingBLL.EnsurePersistent(tracking);

                ts.CommitTransaction();
            }

            return user; //return the newly created user's ID
        }

        /// <summary>
        /// Ensures that the user given has the desired role and unit.
        /// If the user doesn't exist, it is created, and the role and units are associated if they were not already.
        /// </summary>
        /// <returns>the ID of the user</returns>
        public static User InsertNewUserWithRoleAndUnit(User userInformation, string role, string unit,
                                                        string application, string trackingUserName)
        {
            if (userInformation == null || string.IsNullOrEmpty(role) || string.IsNullOrEmpty(unit) ||
                string.IsNullOrEmpty(application)) throw new ArgumentException();

            User user;

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

            if (user.Inactive)
            {
                using (var ts = new TransactionScope())
                {
                    user.Inactive = false; //If we are inserting new assoc. for this user, make them active

                    EnsurePersistent(user);
                    ts.CommitTransaction();
                }
            }

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
            User user = Queryable.Where(usr => usr.LoginID == login).SingleOrDefault();

            if (user == null) return false;

            //Set the email
            user.Email = email;

            //Check to see if the user is valid, if not return false
            if (!ValidateBusinessObject<User>.IsValid(user)) return false;

            //We have a valid user, so get the tracking info and save
            Tracking tracking = TrackingBLL.GetTrackingInstance(login, TrackingTypes.User, TrackingActions.Change);
            tracking.Comments = string.Format("Email changed to {0} for user {1}", user.Email, user.ID);

            using (var ts = new TransactionScope())
            {
                EnsurePersistent(user);
                TrackingBLL.EnsurePersistent(tracking);

                ts.CommitTransaction();
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
            User user = Queryable.Where(usr => usr.LoginID == login).SingleOrDefault();

            if (user == null) return false;

            //Set the email
            user.Phone = phone;

            //Check to see if the user is valid, if not return false
            if (!ValidateBusinessObject<User>.IsValid(user)) return false;

            //We have a valid user, so get the tracking info and save
            Tracking tracking = TrackingBLL.GetTrackingInstance(login, TrackingTypes.User, TrackingActions.Change);
            tracking.Comments = string.Format("Phone changed to {0} for user {1}", user.Email, user.ID);

            using (var ts = new TransactionScope())
            {
                EnsurePersistent(user);
                TrackingBLL.EnsurePersistent(tracking);

                ts.CommitTransaction();
            }

            return true;
        }

        public static bool SetUserInfo(string login, string firstName, string lastName, string phone, string email, string trackingUserName)
        {
            //First get the user identified by the login
            User user = Queryable.Where(usr => usr.LoginID == login).SingleOrDefault();

            if (user == null) return false;

            //Set the userInfo
            user.FirstName = firstName;
            user.LastName = lastName;
            user.Email = email;
            user.Phone = phone;

            //Check to see if the user is valid, if not return false
            if (!ValidateBusinessObject<User>.IsValid(user)) return false;

            //We have a valid user, so get the tracking info and save
            Tracking tracking = TrackingBLL.GetTrackingInstance(trackingUserName, TrackingTypes.User, TrackingActions.Change);
            tracking.Comments = string.Format("UserInfo changed to {0} {1}, {2}, {3} for user {4}", user.FirstName, user.LastName, user.Email, user.Phone, user.ID);

            using (var ts = new TransactionScope())
            {
                EnsurePersistent(user);
                TrackingBLL.EnsurePersistent(tracking);

                ts.CommitTransaction();
            }

            return true;
        }

        #region Applications

        public static List<User> GetByApplication(string application, string searchToken, int page, int pageSize)
        {
            string currentLogin = HttpContext.Current.User.Identity.Name;
            int totalUsers;

            return GetByApplication(application, currentLogin, null, null, searchToken, page, pageSize, "LastName ASC",
                                    out totalUsers);

            //return UserBLL.daoFactory.GetUserDao().GetByApplication(application, null, null, searchToken, page, pageSize, "LastName ASC", out totalUsers);
        }

        public static List<User> GetByApplication(string application, string currentLogin, string role, string unit,
                                                  string searchToken, int page, int pageSize, string orderBy,
                                                  out int totalUsers)
        {
            return DaoFactory.GetUserDao().GetByApplication(application, currentLogin, role, unit, searchToken, page,
                                                            pageSize, orderBy, out totalUsers);
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

        public static IQueryable<User> GetAllActive()
        {
            return UserBLL.Queryable.Where(user => user.Inactive == false);
        }

        public static List<User> GetAllByCriteria(string application, string search, int page, int pagesize)
        {
            int totalUsers;

            return GetAllByCriteria(application, search, page, pagesize, "LastName ASC", out totalUsers);
        }

        public static List<User> GetAllByCriteria(string application, string search, int page, int pagesize, string orderBy, out int users)
        {
            Check.Require(Roles.IsUserInRole("Admin"), "User Must Be A Catbert Administrator To View The List Of All Users");

            return DaoFactory.GetUserDao().GetByCriteria(application, search, page, pagesize, orderBy, out users);
        }
    }
}