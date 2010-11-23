using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;
using UCDArch.Core.NHibernateValidator.Extensions;

namespace Catbert4.Core.Domain
{
    public class Role : DomainObject
    {
        [Length(50)]
        [Required]
        public virtual string Name { get; set; }

        public virtual bool Inactive { get; set; }
    }
}