using Catbert4.Core.Domain;
using FluentNHibernate.Mapping;

namespace Catbert4.Core.Mappings
{
    public class UnitMap : ClassMap<Unit>
    {
        public UnitMap()
        {
            Table("Unit");

            Id(x => x.Id).Column("UnitID");

            Map(x => x.ShortName);
            Map(x => x.FullName);
            Map(x => x.FisCode).Column("FIS_Code");
            Map(x => x.PpsCode).Column("PPS_Code");

            Map(x => x.Type).Column("`Type`").CustomType(typeof(NHibernate.Type.EnumStringType<UnitType>)).Not.Nullable();

            References(x => x.School).Column("SchoolCode").Fetch.Join();
            References(x => x.Parent).Column("ParentID").Cascade.None().Nullable();

            HasMany(x => x.UnitAssociations).Inverse().ReadOnly().Where("Inactive = 0");
        }
    }
}
