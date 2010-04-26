﻿using CAESArch.Core.Domain;

namespace CAESDO.Catbert.Core.Domain
{
    public class Permission : DomainObject<Permission, int>
    {
        public virtual bool Inactive { get; set; }

        public virtual User User { get; set; }
        public virtual Application Application { get; set; }
        public virtual Role Role { get; set; }

        public Permission()
        {

        }
    }
}
