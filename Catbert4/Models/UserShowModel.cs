using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

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

        public IList<PermissionModel> Permissions { get; set; }
        public IList<UnitAssociationModel> UnitAssociations { get; set; }

        public class PermissionModel
        {
            public int Id { get; set; }
            public string ApplicationName { get; set; }
            public string RoleName { get; set; }
        }

        public class UnitAssociationModel
        {
            public int Id { get; set; }
            public string ApplicationName { get; set; }
            public string UnitName { get; set; }
        }
    }
}