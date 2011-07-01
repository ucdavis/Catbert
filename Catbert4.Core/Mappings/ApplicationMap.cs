using Catbert4.Core.Domain;
using FluentNHibernate.Mapping;

namespace Catbert4.Core.Mappings
{
    public class ApplicationMap : ClassMap<Application>
    {
        public ApplicationMap()
        {
            Id(x => x.Id).Column("ApplicationID");

            Map(x => x.Name);
            Map(x => x.Abbr);
            Map(x => x.Location);

            //Map(x => x.WebServiceHash);
            //Map(x => x.Salt);

            Map(x => x.Inactive);

            HasMany(x => x.ApplicationRoles).Inverse().Cascade.AllDeleteOrphan();
            
            HasManyToMany(x => x.ApplicationUnits).ParentKeyColumn("ApplicationID").ChildKeyColumn("UnitID").Table("ApplicationUnits").Cascade.None();
        }
    }
}
