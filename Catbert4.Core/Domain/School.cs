using System.Collections.Generic;
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

        public virtual IList<Unit> Units { get; set; }

        //Note, these two attributes are no longer on the id because it was overridden:
                //"[Newtonsoft.Json.JsonPropertyAttribute()]", 
                //"[System.Xml.Serialization.XmlIgnoreAttribute()]"
        [Required]
        [Length(2)]
        public override string Id
        {
            get
            {
                return base.Id;
            }
            protected set
            {
                base.Id = value;
            }
        }

        public virtual void SetId(string newId)
        {
            Id = newId;
        }
    }
}