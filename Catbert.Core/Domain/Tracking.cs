using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace CAESDO.Catbert.Core.Domain
{
    public class Tracking : DomainObject<Tracking, int>
    {
        [NotNullValidator]
        public virtual TrackingType Type { get; set; }

        [NotNullValidator]
        public virtual TrackingAction Action { get; set; }

        [StringLengthValidator(10)]
        [NotNullValidator]
        public virtual string UserName { get; set; }

        [NotNullValidator]
        public virtual DateTime ActionDate { get; set; }

        public virtual string Comments { get; set; }

        public Tracking()
        {

        }
    }

    /// <summary>
    /// Tracking Types, which include Applications, Permissions, Units, etc.  Anything that can be tracked
    /// </summary>
    public class TrackingType : DomainObject<TrackingType, int>
    {
        public virtual string Name { get; set; }

        public TrackingType()
        {

        }
    }

    /// <summary>
    /// Tracking Actions, like Add/Delete/Update
    /// </summary>
    public class TrackingAction : DomainObject<TrackingAction, int>
    {
        public virtual string Name { get; set; }

        public TrackingAction()
        {

        }
    }
}
