﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAESDO.Catbert.Core.Domain;
using CAESDO.Catbert.Data;

namespace CAESDO.Catbert.BLL
{
    public class PermissionBLL : GenericBLL<Permission, int>
    {
        /// <summary>
        /// Overload of insert permission with the names of required fields instead of IDs
        /// </summary>
        public static Permission InsertPermission(string application, string role, string login, string trackingUserName)
        {
            int applicationID = ApplicationBLL.GetByName(application).ID;
            int userID = UserBLL.GetUser(login).ID;
            int roleID = RoleBLL.GetByName(role).ID;

            return InsertPermission(applicationID, roleID, userID, trackingUserName);
        }

        /// <summary>
        /// Assigns the given role to a user.  If the user already has that role, it returns null
        /// </summary>
        public static Permission InsertPermission(int applicationID, int roleID, int userID, string trackingUserName)
        {
            bool inactive = false;

            //First check to make sure there isn't an existing active permission like this
            if (PermissionExists(applicationID, roleID, userID, inactive)) return null;
            
            //Now see if there is an existing inactive permission
            Permission permission = Queryable.SingleOrDefault(perm => perm.Application.ID == applicationID &&
                                                                perm.Role.ID == roleID && perm.User.ID == userID && perm.Inactive == true);

            //If we didn't find a matching permission, make a new one
            if (permission == null)
            {
                permission = new Permission() { Inactive = false };
                permission.Application = ApplicationBLL.GetByID(applicationID);
                permission.Role = RoleBLL.GetByID(roleID);
                permission.User = UserBLL.GetByID(userID);
            }
            else //else just make the existing permission active
            {
                permission.Inactive = false;
            }

            Tracking tracking = TrackingBLL.GetTrackingInstance(trackingUserName, TrackingTypes.Permission, TrackingActions.Add);
            tracking.Comments = string.Format("Role (roleid:{0}) to application (appid:{1}) granted.", roleID, applicationID);

            using (var ts = new TransactionScope())
            {
                EnsurePersistent(ref permission);
                TrackingBLL.EnsurePersistent(ref tracking);

                ts.CommittTransaction();
            }

            return permission;
        }

        /// <summary>
        /// Overload of delete permission which takes the names of the app/role/login instead of corresponding IDs
        /// </summary>
        public static bool DeletePermission(string application, string role, string login, string trackingUserName)
        {
            //First find the associated permission
            Permission permission = Queryable.SingleOrDefault(perm => perm.Application.Name == application && perm.Role.Name == role
                                                                  && perm.User.LoginID == login);

            if (permission == null) return false;

            return DeletePermission(permission.ID, trackingUserName);
        }

        /// <summary>
        /// Sets the permission indicated by the permissionID to inactive status
        /// </summary>
        public static bool DeletePermission(int permissionID, string trackingUserName)
        {
            Permission permission = Queryable.SingleOrDefault(perm => perm.ID == permissionID);

            //Make sure we have a proper permission and that it isn't already inactive
            if (permission == null || permission.Inactive == true) return false;

            permission.Inactive = true; //Set the permission inactive

            Tracking tracking = TrackingBLL.GetTrackingInstance(trackingUserName, TrackingTypes.Permission, TrackingActions.Delete);
            tracking.Comments = string.Format("Role (roleid:{0}) to application (appid:{1}) removed.", permission.Role.ID, permission.Application.ID);

            using (var ts = new TransactionScope())
            {
                EnsurePersistent(ref permission);
                TrackingBLL.EnsurePersistent(ref tracking);

                ts.CommittTransaction();
            }

            return true;
        }

        public static bool PermissionExists(int applicationID, int roleID, int userID, bool inactive)
        {
            return Queryable.Any(perm => perm.Application.ID == applicationID && perm.Role.ID == roleID && perm.User.ID == userID && perm.Inactive == inactive);
        }
    }
}
