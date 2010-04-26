using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAESDO.Catbert.Core.Domain
{
    public class Unit : DomainObject<Unit, int>
    {
        public virtual string FullName { get; set; }
        public virtual string ShortName { get; set; }

        public virtual string PPSCode { get; set; }
        public virtual string FISCode { get; set; }

        public virtual School School { get; set; }

        public Unit()
        {

        }
    }
}
