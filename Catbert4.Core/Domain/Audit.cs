using System;
using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;
using UCDArch.Core.NHibernateValidator.Extensions;

namespace Catbert4.Core.Domain
{
    public class Audit : DomainObjectWithTypedId<Guid>
    {
        [Length(50)]
        [Required]
        public virtual string ObjectName { get; set; }

        [Length(50)]
        public virtual string ObjectId { get; set; }

        [Length(1)]
        [Required]
        public virtual string AuditAction { get; set; }

        [Length(256)]
        [Required]
        public virtual string Username { get; set; }

        public virtual DateTime AuditDate { get; set; }

        public virtual void SetActionCode(AuditActionType auditActionType)
        {
            switch (auditActionType)
            {
                case AuditActionType.Create:
                    AuditAction = "C";
                    break;
                case AuditActionType.Update:
                    AuditAction = "U";
                    break;
                case AuditActionType.Delete:
                    AuditAction = "D";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("auditActionType");
            }
        }
    }

    public enum AuditActionType
    {
        Create, Update, Delete
    }
}