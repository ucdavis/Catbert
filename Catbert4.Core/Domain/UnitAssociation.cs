using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;

namespace Catbert4.Core.Domain
{
    public class UnitAssociation : DomainObject
    {
        public virtual bool Inactive { get; set; }
        [NotNull]
        public virtual User User { get; set; }
        [NotNull]
        public virtual Application Application { get; set; }
        [NotNull]
        public virtual Unit Unit { get; set; }
    }
}