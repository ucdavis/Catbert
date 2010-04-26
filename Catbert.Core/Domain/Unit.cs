using CAESArch.Core.Domain;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace CAESDO.Catbert.Core.Domain
{
    public class Unit : DomainObject<Unit, int>
    {
        [StringLengthValidator(50)]
        [NotNullValidator]
        public virtual string FullName { get; set; }

        [StringLengthValidator(50)]
        public virtual string ShortName { get; set; }

        [StringLengthValidator(6)]
        public virtual string PPSCode { get; set; }

        [StringLengthValidator(4)]
        [NotNullValidator]
        public virtual string FISCode { get; set; }

        [NotNullValidator]
        public virtual School School { get; set; }

        public Unit()
        {

        }
    }
}
