using Catbert4.Core.Domain;
using FluentNHibernate.Mapping;

namespace Catbert4.Core.Mappings
{
    public class AuditMap : ClassMap<Audit>
    {
        public AuditMap()
        {
            Id(x => x.Id).GeneratedBy.GuidComb();
            Map(x => x.ObjectName);
            Map(x => x.ObjectId);
            Map(x => x.AuditActionTypeId);
            Map(x => x.Username);
            Map(x => x.AuditDate);
        }
    }
}