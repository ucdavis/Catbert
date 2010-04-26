using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAESDO.Catbert.Core.Domain
{
    public class School : DomainObject<School, string>
    {
        public virtual string ShortDescription { get; set; }
        public virtual string LongDescription { get; set; }

        public virtual string Abbreviation { get; set; }

        public School()
        {

        }
    }
}
