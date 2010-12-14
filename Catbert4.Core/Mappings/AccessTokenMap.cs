using FluentNHibernate.Mapping;
using Catbert4.Core.Domain;

namespace Catbert4.Core.Mappings
{
    public class AccessTokenMap : ClassMap<AccessToken>
    {
        public AccessTokenMap()
        {
            Id(x => x.Id);

            Map(x => x.Token);
            Map(x => x.ContactEmail);
            Map(x => x.Reason);
            Map(x => x.Active);
            
            References(x => x.Application).Column("ApplicationID");
        }
    }
}
