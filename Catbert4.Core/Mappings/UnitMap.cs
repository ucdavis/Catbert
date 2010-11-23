using Catbert4.Core.Domain;
using FluentNHibernate.Mapping;

namespace Catbert4.Core.Mappings
{
    public class UnitMap : ClassMap<Unit>
    {
        public UnitMap()
        {
            Id(x => x.Id).Column("UnitID");

            Map(x => x.ShortName);
            Map(x => x.FullName);
            Map(x => x.FisCode).Column("FIS_Code");
            Map(x => x.PpsCode).Column("PPS_Code");

            References(x => x.School);
        }
    }
}
