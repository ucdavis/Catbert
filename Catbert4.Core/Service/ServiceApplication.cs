using FluentNHibernate.Mapping;

namespace Catbert4.Core.Service
{
    public class ServiceApplication
    {
        public virtual int ID { get; set; }

        public virtual string Name { get; set; }

        public virtual string Abbr { get; set; }

        public virtual string Location { get; set; }

        public virtual bool Inactive { get; set; }
    }

    public class ServiceApplicationMap : ClassMap<ServiceApplication>
    {
        public ServiceApplicationMap()
        {
            Table("Applications");
            ReadOnly();

            Id(x => x.ID).Column("ApplicationID");

            Map(x => x.Name);
            Map(x => x.Abbr);
            Map(x => x.Location);

            Map(x => x.Inactive);
        }
    }

}