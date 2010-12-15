using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;
using UCDArch.Core.NHibernateValidator.Extensions;

namespace Catbert4.Core.Domain
{
    public class Unit : DomainObject
    {
        [Length(50)]
        [Required]
        public virtual string FullName { get; set; }

        [Length(50)]
        [Required]
        public virtual string ShortName { get; set; }

        [Length(6)]
        public virtual string PpsCode { get; set; }

        [Length(4)]
        [Required]
        public virtual string FisCode { get; set; }

        public virtual Unit Parent { get; set; }

        [NotNull]
        public virtual School School { get; set; }
        
        [NotNull(Message = "Fill it in")]
        public virtual UnitType Type { get; set; }
    }

    public enum UnitType
    {
        Cluster,
        Department
    }
}