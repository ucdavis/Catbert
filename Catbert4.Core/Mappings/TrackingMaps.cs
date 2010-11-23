using Catbert4.Core.Domain;
using FluentNHibernate.Mapping;

namespace Catbert4.Core.Mappings
{
    public class TrackingMap : ClassMap<Tracking>
    {
        public TrackingMap()
        {
            Id(x => x.Id).Column("TrackingID");

            Map(x => x.ActionDate).Column("TrackingActionDate");
            Map(x => x.Comments);
            Map(x => x.UserName).Column("TrackingUserName");

            References(x => x.Action).Not.Nullable();
            References(x => x.Type).Not.Nullable();
        }
    }

    public class TrackingTypeMap : ClassMap<TrackingType>
    {
        public TrackingTypeMap()
        {
            Id(x => x.Id).Column("TrackingTypeID");

            Map(x => x.Name);

        }
    }

    public class TrackingActionMap : ClassMap<TrackingAction>
    {
        public TrackingActionMap()
        {
            Id(x => x.Id).Column("TrackingActionID");

            Map(x => x.Name);

        }
    }
}
