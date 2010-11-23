using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;
using UCDArch.Core.NHibernateValidator.Extensions;

namespace Catbert4.Core.Domain
{
    public class School : DomainObjectWithTypedId<string>
    {
        [Length(25)]
        [Required]
        public virtual string ShortDescription { get; set; }

        [Length(50)]
        [Required]
        public virtual string LongDescription { get; set; }

        [Length(12)]
        [Required]
        public virtual string Abbreviation { get; set; }

        public virtual void SetId(string newId)
        {
            Id = newId;
        }
    }
}