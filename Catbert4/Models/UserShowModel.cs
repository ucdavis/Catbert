using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Catbert4.Core.Domain;

namespace Catbert4.Models
{
    public class UserShowModel
    {
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        [HiddenInput(DisplayValue = false)]
        public string FullNameAndLogin { get; set; }
        
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Phone { get; set; }

        public IList<Permission> Permissions { get; set; }
        public IList<UnitAssociation> UnitAssociations { get; set; }
    }
}