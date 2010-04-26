using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAESDO.Catbert.Core.Domain
{
    public class Application : DomainObject<Application, int>
    {
        public virtual string Name { get; set; }
        public virtual string Abbr { get; set; }

        public virtual string Location { get; set; }

        public virtual string WebServiceHash { get; set; }
        public virtual string Salt { get; set; }

        public virtual bool Inactive { get; set; }

        public Application()
        {

        }
    }
}
