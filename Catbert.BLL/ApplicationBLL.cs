using System;
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

        public static bool SetActiveStatus(int applicationID, bool inactive, string trackingUserName)
        {
            //Are there any applications that need to be changed fitting the criteria?
            bool applicationExists = Queryable.Where(app => app.ID == applicationID && app.Inactive != inactive).Any();

            if (!applicationExists) return false;

            //The application exists, so make the change and track it
            Application application = Queryable.Where(app => app.ID == applicationID).Single();

            application.Inactive = inactive;

            //Tracking tracking = new Tracking() { UserName = trackingUserName, ActionDate = DateTime.Now };

            /*
            ApplicationTracking appTracking = new ApplicationTracking() { Application = application, TrackingActionDate = DateTime.Now, TrackingUserName = trackingUserName };
            appTracking.Comments = string.Format("Application {0} status changed to inactive={1}", applicationID, inactive);
            appTracking.TrackingType = GetTrackingType(TrackingTypes.Change);

            db.ApplicationTrackings.InsertOnSubmit(appTracking);
            */

            using (TransactionScope ts = new TransactionScope())
            {
                ApplicationBLL.EnsurePersistent(ref application);

                ts.CommittTransaction();
            }

            return true;
        }
    }
}
