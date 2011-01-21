using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace Catbert4.Core.Service
{
    public class ServiceSchool
    {
        public virtual string ID { get; set; }

        public virtual string ShortDescription { get; set; }

        public virtual string LongDescription { get; set; }

        public virtual string Abbreviation { get; set; }

        public virtual IList<ServiceUnit> Units { get; set; }
    }

    public class ServiceSchoolMap : ClassMap<ServiceSchool>
    {
        public ServiceSchoolMap()
        {
            Table("Schools");

            ReadOnly();

            Id(x => x.ID).Column("SchoolCode").GeneratedBy.Assigned();

            Map(x => x.ShortDescription);
            Map(x => x.LongDescription);
            Map(x => x.Abbreviation);

            HasMany(x => x.Units).KeyColumn("SchoolCode").ReadOnly();
        }
    }
}
