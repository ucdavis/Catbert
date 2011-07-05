using Catbert4.Core.Domain;
using FluentNHibernate.Mapping;

namespace Catbert4.Core.Mappings
{
    public class ApplicationUnitMap : ClassMap<ApplicationUnit>
    {
        public ApplicationUnitMap()
        {
            Id(x => x.Id).Column("ApplicationUnitID");

            References(x => x.Application);
            References(x => x.Unit).Fetch.Join();
        }
    }
}
