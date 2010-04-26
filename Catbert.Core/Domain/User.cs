using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAESDO.Catbert.Core.Domain
{
    public class User : DomainObject<User, int>
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }

        public virtual string EmployeeID { get; set; }
        public virtual string StudentID { get; set; }

        public virtual Guid UserKey { get; set; }

        public virtual bool Inactive { get; set; }

        public User()
        {

        }
    }
}
