using System.Collections.Generic;
using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;
using UCDArch.Core.NHibernateValidator.Extensions;

namespace Catbert4.Core.Domain
{
    public class Application : DomainObject
    {
        public Application()
        {
            ApplicationRoles = new List<ApplicationRole>();
            //Roles = new List<Role>();
        }

        [Length(50)]
        [Required]
        public virtual string Name { get; set; }

        [Length(50)]
        public virtual string Abbr { get; set; }

        [Length(256)]
        public virtual string Location { get; set; }

        [Length(100)]
        public virtual string WebServiceHash { get; set; }

        [Length(20)]
        public virtual string Salt { get; set; }

        public virtual bool Inactive { get; set; }

        public virtual IList<ApplicationRole> ApplicationRoles { get; set; }
    }
}