using System;
using CAESDO.Catbert.Core.Domain;

namespace CAESDO.Catbert.BLL
{
    public class TrackingBLL : GenericBLL<Tracking, int>
    {
        internal static Tracking GetTrackingInstance(string username, TrackingTypes type, TrackingActions action)
        {
            var tracking = new Tracking() {UserName = username, ActionDate = DateTime.Now};

            tracking.Action = GetTrackingAction(TrackingActions.Change);
            tracking.Type = GetTrackingType(TrackingTypes.Application);

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

    internal class TrackingTypeBLL : GenericBLL<TrackingType, int>
    {
    }

    internal class TrackingActionBLL : GenericBLL<TrackingAction, int>
    {
    }
}