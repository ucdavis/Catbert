using CAESArch.Core.Domain;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace CAESDO.Catbert.Core.Domain
{
    public class School : DomainObject<School, string>
    {
        [StringLengthValidator(25)]
        [NotNullValidator]

        public virtual string ShortDescription { get; set; }
        [StringLengthValidator(50)]
        [NotNullValidator]
        public virtual string LongDescription { get; set; }

        [StringLengthValidator(12)]
        [NotNullValidator]
        public virtual string Abbreviation { get; set; }

        public School()
        {

        }
    }
}
