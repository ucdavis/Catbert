using Catbert4.Core.Domain;
using FluentNHibernate.Mapping;

namespace Catbert4.Core.Mappings
{
    public class PermissionMap : ClassMap<Permission>
    {
        public PermissionMap()
        {
            Where("Inactive = 0");

            Id(x => x.Id).Column("PermissionID");

            Map(x => x.Inactive);

            References(x => x.Application).Not.Nullable();
            References(x => x.Role).Fetch.Join().Not.Nullable();
            References(x => x.User).Not.Nullable();
        }
    }
}
