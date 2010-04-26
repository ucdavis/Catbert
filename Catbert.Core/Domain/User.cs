using System;
using System.Collections.Generic;
using CAESArch.Core.Domain;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace CAESDO.Catbert.Core.Domain
{
    public class User : DomainObject<User, int>
    {
        [StringLengthValidator(50)]
        [IgnoreNulls]
        public virtual string FirstName { get; set; }

        [StringLengthValidator(50)]
        [IgnoreNulls]
        public virtual string LastName { get; set; }

        [StringLengthValidator(10)]
        [NotNullValidator]
        public virtual string LoginID { get; set; }

        [StringLengthValidator(50)]
        [RegexValidator(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*")]
        [IgnoreNulls]
        public virtual string Email { get; set; }

        [StringLengthValidator(50)]
        [RegexValidator(@"((\+\d{1,3}(-| )?\(?\d\)?(-| )?\d{1,5})|(\(?\d{2,6}\)?))(-| )?(\d{3,4})(-| )?(\d{4})(( x| ext)\d{1,5}){0,1}")]
        [IgnoreNulls]
        public virtual string Phone { get; set; }

        [StringLengthValidator(9)]
        [IgnoreNulls]
        public virtual string EmployeeID { get; set; }

        [StringLengthValidator(9)]
        [IgnoreNulls]
        public virtual string StudentID { get; set; }

        [NotNullValidator]
        public virtual Guid UserKey { get; set; }

        public virtual bool Inactive { get; set; }

        public virtual IList<UnitAssociation> UnitAssociations { get; set; }

        public virtual IList<Permission> Permissions { get; set; }
    }
}