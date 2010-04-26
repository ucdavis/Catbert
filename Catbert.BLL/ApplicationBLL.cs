﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAESDO.Catbert.Core.Domain;
using CAESDO.Catbert.Data;

namespace CAESDO.Catbert.BLL
{
    public class ApplicationBLL : GenericBLL<Application, int>
    {
        public static List<Application> GetAll(bool inactive)
        {
            var query = from app in Queryable
                           where app.Inactive == inactive
                           orderby app.Name
                            select app;

            return query.ToList<Application>();
        }

        /// <summary>
        /// Set the roles in this application to be the roles
        /// </summary>
        public static void SetRoles(Application application, List<string> roles)
        {
            List<Role> rolesToRemove = new List<Role>();

            //Go through all of the active roles in the current application
            foreach (var role in application.Roles.Where(r => r.Inactive == false))
            {
                //If this role is in the roles list, it stays
                if (roles.Contains(role.Name))
                {
                    //Get rid of it in the roles list since we've already seen it
                    roles.Remove(role.Name);
                }
                else
                {
                    //This role doesn't exist in the roles list, so remove it
                    rolesToRemove.Add(role);
                }
            }

            //Remove from the current application all of the roles that didn't exist in the roles list
            foreach (var role in rolesToRemove)
            {
                application.Roles.Remove(role);
            }

            //Now we go through the roles that weren't matched and add them
            foreach (var role in roles)
            {
                var newRole = RoleBLL.Queryable.Where(r => r.Name == role).Single();
                application.Roles.Add(newRole);
            }

            //Now we should have an application with reconciled roles
        }

        /// <summary>
        /// Updates an existing application
        /// </summary>
        public static void Update(Application application, string trackingUserName)
        {
            Tracking tracking = TrackingBLL.GetTrackingInstance(trackingUserName, TrackingTypes.Application, TrackingActions.Change);
            tracking.Comments = string.Format("Application {0} updated", application.ID);

            using (TransactionScope ts = new TransactionScope())
            {
                ApplicationBLL.EnsurePersistent(ref application); //Persist the application
                TrackingBLL.EnsurePersistent(ref tracking);

                ts.CommittTransaction();
            }
        }

        /// <summary>
        /// Creates a new application from the given instance
        /// </summary>
        public static void Create(Application application, string trackingUserName)
        {
            Tracking tracking = TrackingBLL.GetTrackingInstance(trackingUserName, TrackingTypes.Application, TrackingActions.Add);
            
            using (TransactionScope ts = new TransactionScope())
            {
                ApplicationBLL.EnsurePersistent(ref application); //Persist the application

                tracking.Comments = string.Format("Application {0} created", application.ID);
                TrackingBLL.EnsurePersistent(ref tracking);

                ts.CommittTransaction();
            }
        }

        /// <summary>
        /// Returns false if the application could not be found or if it doesn't need to change inactive status
        /// </summary>
        public static bool SetActiveStatus(int applicationID, bool inactive, string trackingUserName)
        {
            //Get the application
            Application application = ApplicationBLL.GetByID(applicationID);
            
            //Does this application's active status need to be changed?
            if (application.Inactive == inactive)
            {
                return false;
            }
            else
            {
                application.Inactive = inactive; //make the change
            }

            //Track the change
            Tracking tracking = TrackingBLL.GetTrackingInstance(trackingUserName, TrackingTypes.Application, TrackingActions.Change);
            tracking.Comments = string.Format("Application {0} status changed to inactive={1}", applicationID, inactive);

            using (TransactionScope ts = new TransactionScope())
            {
                ApplicationBLL.EnsurePersistent(ref application);
                TrackingBLL.EnsurePersistent(ref tracking);

                ts.CommittTransaction();
            }

            return true;
        }

        /// <summary>
        /// Returns an application's ID given its name
        /// </summary>
        public static int GetID(string application)
        {
            return Queryable.Where(app => app.Name == application).Select(app => app.ID).Single();
        }
    }
}
