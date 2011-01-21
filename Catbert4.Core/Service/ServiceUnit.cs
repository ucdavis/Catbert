using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace Catbert4.Core.Service
{
    public class ServiceUnit
    {
        public ServiceUnit()
        {
            UnitAssociations = new List<ServiceUnitAssociation>();
        }

        public virtual int ID { get; set; }

        public virtual string FullName { get; set; }

        public virtual string ShortName { get; set; }

        public virtual string PpsCode { get; set; }

        public virtual string FisCode { get; set; }

        public virtual ServiceUnit Parent { get; set; }

        public virtual ServiceSchool School { get; set; }

        public virtual string Type { get; set; }

        public virtual IList<ServiceUnitAssociation> UnitAssociations { get; set; }
    }

    public class ServiceUnitMap : ClassMap<ServiceUnit>
    {
        public ServiceUnitMap()
        {
            ReadOnly();
            
            Table("Unit");

            Id(x => x.ID).Column("UnitID");

            Map(x => x.ShortName);
            Map(x => x.FullName);
            Map(x => x.FisCode).Column("FIS_Code");
            Map(x => x.PpsCode).Column("PPS_Code");

            Map(x => x.Type).Column("`Type`");

            References(x => x.School).Column("SchoolCode");
            References(x => x.Parent).Column("ParentID").Cascade.None().Nullable();

            HasMany(x => x.UnitAssociations).KeyColumn("UnitID").ReadOnly().Where("Inactive = 0");
        }
    }
}
