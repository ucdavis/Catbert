using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAESDO.Catbert.Core.Domain;

namespace CAESDO.Catbert.BLL
{
    public class TrackingBLL : GenericBLL<Tracking, int>
    {
        internal static Tracking GetTrackingInstance(string username, TrackingTypes type, TrackingActions action)
        {
            Tracking tracking = new Tracking() { UserName = username, ActionDate = DateTime.Now };

            tracking.Action = TrackingBLL.GetTrackingAction(TrackingActions.Change);
            tracking.Type = TrackingBLL.GetTrackingType(TrackingTypes.Application);

            return tracking;
        }

        public static TrackingType GetTrackingType(TrackingTypes type)
        {
            return TrackingTypeBLL.GetByName(type.ToString());
        }

        public static TrackingAction GetTrackingAction(TrackingActions action)
        {
            return TrackingActionBLL.GetByName(action.ToString());
        }
    }

    internal class TrackingTypeBLL : GenericBLL<TrackingType, int> { }
    internal class TrackingActionBLL : GenericBLL<TrackingAction, int> { }
}
