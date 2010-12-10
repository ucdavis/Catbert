using Catbert4.Core.Domain;
using FluentNHibernate.Mapping;

namespace Catbert4.Core.Mappings
{
    public class RoleMap : ClassMap<Role>
    {
        public RoleMap()
        {
            Where("Inactive = 0"); //active roles only

            Id(x => x.Id).Column("RoleID");

            Map(x => x.Name).Column("Role");
            Map(x => x.Inactive);
        }
    }
}
