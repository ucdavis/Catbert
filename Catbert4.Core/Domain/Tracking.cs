using System;
using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;
using UCDArch.Core.NHibernateValidator.Extensions;

namespace Catbert4.Core.Domain
{
    public class Tracking : DomainObject
    {
        [NotNull]
        public virtual TrackingType Type { get; set; }

        [NotNull]
        public virtual TrackingAction Action { get; set; }

        [Length(10)]
        [Required]
        public virtual string UserName { get; set; }

        [NotNull]
        public virtual DateTime ActionDate { get; set; }

        public virtual string Comments { get; set; }
    }

    /// <summary>
    /// Tracking Types, which include Applications, Permissions, Units, etc.  Anything that can be tracked
    /// </summary>
    public class TrackingType : DomainObject
    {
        public virtual string Name { get; set; }
    }

    public enum TrackingTypes
    {
        Application,
        Role,
        Permission,
        Unit,
        User
    }

    /// <summary>
    /// Tracking Actions, like Add/Delete/Update
    /// </summary>
    public class TrackingAction : DomainObject
    {
        public virtual string Name { get; set; }
    }

    public enum TrackingActions
    {
        Add,
        Change,
        Delete
    }
}