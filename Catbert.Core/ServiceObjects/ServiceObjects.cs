using System;
using System.Collections.Generic;

namespace CAESDO.Catbert.Core.ServiceObjects
{
    /// <summary>
    /// Represents the association of a permission to an application
    /// </summary>
    [Serializable]
    public class PermissionAssociation
    {
        public string ApplicationName { get; set; }
        public string RoleName { get; set; }
    }

    /// <summary>
    /// Represents the association of a unit to an application
    /// </summary>
    [Serializable]
    public class UnitAssociation
    {
        public string ApplicationName { get; set; }
        public string UnitName { get; set; }
    }

    /// <summary>
    /// Represents a generalized view of the information for a specific user,
    /// regardless of the current application
    /// </summary>
    [Serializable]
    public class UserInformation
    {
        public string LoginId { get; set; }

        public List<PermissionAssociation> PermissionAssociations { get; set; }
        public List<UnitAssociation> UnitAssociations { get; set; }
    }
}