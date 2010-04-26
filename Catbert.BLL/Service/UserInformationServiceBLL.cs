using System.Collections.Generic;
using System.Linq;
using CAESDO.Catbert.Core.ServiceObjects;
using CAESArch.Core.Utils;

namespace CAESDO.Catbert.BLL.Service
{
    public class UserInformationServiceBLL
    {
        public static UserInformation GetInformationByLoginId(string loginId)
        {
            var user = UserBLL.GetUser(loginId);

            Check.Require(user != null);

            var userInformation = new UserInformation
                                      {
                                          LoginId = user.LoginID,
                                          UnitAssociations = GetUnitAssociationsByLoginId(loginId),
                                          PermissionAssociations = GetPermissionAssociationsByLoginId(loginId)
                                      };


            return userInformation;
        }
        
        /// <summary>
        /// Returns a list of app Name <--> unit Name objects
        /// </summary>
        public static List<UnitAssociation> GetUnitAssociationsByLoginId(string loginId)
        {
            Check.Require(!string.IsNullOrEmpty(loginId));

            var unitAssociations = from association in UnitAssociationBLL.EntitySet
                                    where association.User.LoginID == loginId && association.Inactive == false
                                    select
                                      new UnitAssociation
                                          {
                                              ApplicationName = association.Application.Name,
                                              UnitName = association.Unit.ShortName
                                          };

            return unitAssociations.ToList();
        }

        /// <summary>
        /// Returns a list of app name <--> role name objects
        /// </summary>
        public static List<PermissionAssociation> GetPermissionAssociationsByLoginId(string loginId)
        {
            Check.Require(!string.IsNullOrEmpty(loginId));

            var permissions = from perm in PermissionBLL.EntitySet
                              where perm.User.LoginID == loginId && perm.Inactive == false
                              select
                                  new PermissionAssociation
                                      {
                                          ApplicationName = perm.Application.Name, 
                                          RoleName = perm.Role.Name
                                      };

            return permissions.ToList();
        }
    }
}
