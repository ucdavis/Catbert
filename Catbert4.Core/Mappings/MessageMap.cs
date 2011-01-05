using Catbert4.Core.Domain;
using FluentNHibernate.Mapping;

namespace Catbert4.Core.Mappings
{
    public class MessageMap : ClassMap<Message>
    {
        public MessageMap()
        {
            Id(x => x.Id);

            Map(x => x.Text).Column("Message");
            Map(x => x.BeginDisplayDate);
            Map(x => x.EndDisplayDate);
            Map(x => x.Critical);
            Map(x => x.Active).Column("IsActive");

            References(x => x.Application).Cascade.None().Nullable();
        }
    }
}
