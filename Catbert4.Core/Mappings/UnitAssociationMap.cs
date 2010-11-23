using Catbert4.Core.Domain;
using FluentNHibernate.Mapping;

namespace Catbert4.Core.Mappings
{
    public class UnitAssociationMap : ClassMap<UnitAssociation>
    {
        public UnitAssociationMap()
        {
            Id(x => x.Id).Column("UnitAssociationID");

            Map(x => x.Inactive);

            References(x => x.Application).Not.Nullable();
            References(x => x.Unit).Fetch.Join().Not.Nullable();
            References(x => x.User).Not.Nullable();
        }
    }
}
