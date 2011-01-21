using FluentNHibernate.Mapping;

namespace Catbert4.Core.Service
{
    public class ServiceUnitAssociation
    {
        public virtual int ID { get; set; }

        public virtual bool Inactive { get; set; }
        
        public virtual ServiceUser User { get; set; }
        
        public virtual ServiceApplication Application { get; set; }
        
        public virtual ServiceUnit Unit { get; set; }
    }

    public class ServiceUnitAssociationMap : ClassMap<ServiceUnitAssociation>
    {
        public ServiceUnitAssociationMap()
        {
            Table("UnitAssociations");

            ReadOnly();

            Where("Inactive = 0");

            Id(x => x.ID).Column("UnitAssociationID");

            Map(x => x.Inactive);

            References(x => x.User).Column("UserID").Not.Nullable();
            References(x => x.Unit).Column("UnitID").Not.Nullable();
            References(x => x.Application).Column("ApplicationID").Not.Nullable();
        }
    }
}