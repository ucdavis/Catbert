using Catbert4.Core.Domain;
using FluentNHibernate.Mapping;

namespace Catbert4.Core.Mappings
{
    public class ApplicationRoleMap : ClassMap<ApplicationRole>
    {
        public ApplicationRoleMap()
        {
            Id(x => x.Id).Column("ApplicationRoleID");

            Map(x => x.Level);

            References(x => x.Application);
            References(x => x.Role).Fetch.Join();

        }
    }
}
