using System;
using System.Collections.Generic;
using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;
using UCDArch.Core.NHibernateValidator.Extensions;

namespace Catbert4.Core.Domain
{
    public class User : DomainObject
    {
        [Length(50)]
        public virtual string FirstName { get; set; }

        [Length(50)]
        public virtual string LastName { get; set; }

        [Length(10)]
        [Required]
        public virtual string LoginId { get; set; }

        [Length(50)]
        [Email]
        public virtual string Email { get; set; }

        [Length(50)]
        [Pattern(@"((\+\d{1,3}(-| )?\(?\d\)?(-| )?\d{1,5})|(\(?\d{2,6}\)?))(-| )?(\d{3,4})(-| )?(\d{4})(( x| ext)\d{1,5}){0,1}")]
        public virtual string Phone { get; set; }

        [Length(9)]
        public virtual string EmployeeId { get; set; }

        [Length(9)]
        public virtual string StudentId { get; set; }

        [NotNull]
        public virtual Guid UserKey { get; set; } //Test that an empty guid is invalid?

        public virtual bool Inactive { get; set; }

        public virtual IList<UnitAssociation> UnitAssociations { get; set; }

        public virtual IList<Permission> Permissions { get; set; }
    }
}