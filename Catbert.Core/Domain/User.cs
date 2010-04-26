using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace CAESDO.Catbert.Core.Domain
{
    public class User : DomainObject<User, int>
    {
        [StringLengthValidator(50)]
        public virtual string FirstName { get; set; }
        [StringLengthValidator(50)]
        public virtual string LastName { get; set; }

        [StringLengthValidator(9)]
        public virtual string EmployeeID { get; set; }
        [StringLengthValidator(9)]
        public virtual string StudentID { get; set; }

        [NotNullValidator]
        public virtual Guid UserKey { get; set; }

        public virtual bool Inactive { get; set; }

        public virtual IList<Unit> Units { get; set; }

        public User()
        {

        }
    }
}
