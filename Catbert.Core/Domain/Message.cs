using System;
using System.Collections.Generic;
using CAESArch.Core.Domain;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace CAESDO.Catbert.Core.Domain
{
    public class Message : DomainObject<Message, int>
    {
    //?      [StringLengthValidator(50)]
        [NotNullValidator]
        public virtual string MessageText { get; set; }
        [NotNullValidator]
        public virtual DateTime BeginDisplayDate { get; set; }
        public virtual DateTime? EndDisplayDate { get; set; }
        
        [NotNullValidator] 
        public virtual bool IsActive { get; set; }
        public virtual Application Application { get; set; }


        public virtual string BeginDisplayDateString
        {
            get{
                return BeginDisplayDate.ToShortDateString();
            }
        }

        public virtual string EndDisplayDateString
        {
            get {
                return EndDisplayDate.HasValue ? EndDisplayDate.Value.ToShortDateString() : "N/A";
            }
        }
    

    }
}
