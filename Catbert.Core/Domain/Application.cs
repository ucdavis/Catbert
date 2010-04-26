using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace CAESDO.Catbert.Core.Domain
{
    public class Application : DomainObject<Application, int>
    {
        [StringLengthValidator(50)]
        [NotNullValidator]
        public virtual string Name { get; set; }

        [StringLengthValidator(50)]
        public virtual string Abbr { get; set; }

        [StringLengthValidator(256)]
        public virtual string Location { get; set; }

        [StringLengthValidator(100)]
        public virtual string WebServiceHash { get; set; }
        [StringLengthValidator(20)]
        public virtual string Salt { get; set; }

        public virtual bool Inactive { get; set; }

        public virtual IList<Role> Roles { get; set; }

        public Application()
        {

        }
    }
}
