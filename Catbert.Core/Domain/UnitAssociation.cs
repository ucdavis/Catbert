﻿using CAESArch.Core.Domain;

namespace CAESDO.Catbert.Core.Domain
{
    public class UnitAssociation : DomainObject<UnitAssociation, int>
    {
        public virtual bool Inactive { get; set; }

        public virtual User User { get; set; }
        public virtual Application Application { get; set; }
        public virtual Unit Unit { get; set; }
    }
}