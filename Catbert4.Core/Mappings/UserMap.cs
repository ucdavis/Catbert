using Catbert4.Core.Domain;
using FluentNHibernate.Mapping;

namespace Catbert4.Core.Mappings
{
    public class UserMap : ClassMap<User>
    {
        private const string InactiveConstraint = "Inactive = 0";

        public UserMap()
        {
            Where(InactiveConstraint);

            Id(x => x.Id).Column("UserID");

            Map(x => x.Email);
            Map(x => x.FirstName);
            Map(x => x.LastName);
            Map(x => x.LoginId);
            Map(x => x.Phone);
            
            Map(x => x.Inactive);
            
            //Map(x => x.EmployeeId);
            //Map(x => x.StudentId);
            //Map(x => x.UserKey);

            HasMany(x => x.Permissions).Not.Inverse().Cascade.None().Where(InactiveConstraint);
            HasMany(x => x.UnitAssociations).Not.Inverse().Cascade.None().Where(InactiveConstraint);
        }
    }
}
