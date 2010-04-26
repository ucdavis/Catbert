using System.Collections.Generic;
using CAESArch.Core.Domain;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace CAESDO.Catbert.Core.Domain
{
    public class Application : DomainObject<Application, int>
    {
        [StringLengthValidator(50)]
        [NotNullValidator]
        public virtual string Name { get; set; }

        [StringLengthValidator(50)]
        [IgnoreNulls]
        public virtual string Abbr { get; set; }

        [StringLengthValidator(256)]
        [IgnoreNulls]
        public virtual string Location { get; set; }

        [StringLengthValidator(100)]
        [IgnoreNulls]
        public virtual string WebServiceHash { get; set; }
        [StringLengthValidator(20)]
        [IgnoreNulls]
        public virtual string Salt { get; set; }

        public virtual bool Inactive { get; set; }

        //public virtual IList<Role> Roles { get; set; }
        public virtual IList<ApplicationRole> ApplicationRoles { get; set; }

        public Application()
        {
            ApplicationRoles = new List<ApplicationRole>();
            //Roles = new List<Role>();
        }
    }
}
