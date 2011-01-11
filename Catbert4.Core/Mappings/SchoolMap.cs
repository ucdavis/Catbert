using Catbert4.Core.Domain;
using FluentNHibernate.Mapping;

namespace Catbert4.Core.Mappings
{
    public class SchoolMap : ClassMap<School>
    {
        public SchoolMap()
        {
            Cache.ReadWrite();

            Id(x => x.Id).Column("SchoolCode").GeneratedBy.Assigned();

            Map(x => x.ShortDescription);
            Map(x => x.LongDescription);
            Map(x => x.Abbreviation);

            HasMany(x => x.Units).KeyColumn("SchoolCode").ReadOnly();
        }
    }
}
