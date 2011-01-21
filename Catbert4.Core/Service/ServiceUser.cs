using FluentNHibernate.Mapping;

namespace Catbert4.Core.Service
{
    public class ServiceUser
    {
        public ServiceUser()
        {
            //UnitAssociations = new List<UnitAssociation>();
            //Permissions = new List<Permission>();
        }

        public virtual int ID { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string LoginId { get; set; }

        public virtual string Email { get; set; }

        public virtual string Phone { get; set; }

        public virtual bool Inactive { get; set; }

        //public virtual IList<UnitAssociation> UnitAssociations { get; set; }

        //public virtual IList<Permission> Permissions { get; set; }

        public virtual string FullNameAndLogin
        {
            get
            {
                return string.Format("{0} {1} ({2})", FirstName, LastName, LoginId);
            }
        }
    }

    public class ServiceUserMap : ClassMap<ServiceUser>
    {
        private const string InactiveConstraint = "Inactive = 0";

        public ServiceUserMap()
        {
            Table("Users");

            ReadOnly();

            Where(InactiveConstraint);

            Id(x => x.ID).Column("UserID");

            Map(x => x.Email);
            Map(x => x.FirstName);
            Map(x => x.LastName);
            Map(x => x.LoginId);
            Map(x => x.Phone);
            
            Map(x => x.Inactive);
            
            //Map(x => x.EmployeeId);
            //Map(x => x.StudentId);
            //Map(x => x.UserKey);

            //HasMany(x => x.Permissions).Inverse().Cascade.AllDeleteOrphan().Where(InactiveConstraint);
            //HasMany(x => x.UnitAssociations).Inverse().Cascade.AllDeleteOrphan().Where(InactiveConstraint);

        }
    }
}
