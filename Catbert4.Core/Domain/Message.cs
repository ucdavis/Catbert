using System.ComponentModel.DataAnnotations;
using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;
using System;

namespace Catbert4.Core.Domain
{
    public class Message : DomainObject
    {
        public Message()
        {
            Active = true;
            BeginDisplayDate = EndDisplayDate = DateTime.Now;
        }

        [UCDArch.Core.NHibernateValidator.Extensions.Required]
        [DataType(DataType.MultilineText)]
        public virtual string Text { get; set; }
        
        public virtual DateTime BeginDisplayDate { get; set; }
        
        [Future]
        public virtual DateTime EndDisplayDate { get; set; }

        public virtual bool Critical { get; set; }

        public virtual bool Active { get; set; }

        public virtual Application Application { get; set; }
    }
}
