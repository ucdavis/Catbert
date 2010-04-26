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
    }
}
