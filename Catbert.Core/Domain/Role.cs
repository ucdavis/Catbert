﻿using CAESArch.Core.Domain;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace CAESDO.Catbert.Core.Domain
{
    public class Role : DomainObject<Role, int>
    {
        [StringLengthValidator(50)]
        [NotNullValidator]
        public virtual string Name { get; set; }

        public virtual bool Inactive { get; set; }

        public Role()
        {

        }
    }
}
