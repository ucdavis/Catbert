using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAESDO.Catbert.Core.Domain
{
    public class Role : DomainObject<Role, int>
    {
        public virtual string Name { get; set; }

        public virtual bool Inactive { get; set; }

        public Role()
        {

        }
    }
}
