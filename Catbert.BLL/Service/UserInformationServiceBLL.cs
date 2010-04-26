using System;
using System.Collections.Generic;
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
        private static List<UnitAssociation> GetUnitAssociationsByLoginId(string loginId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a list of app name <--> role name objects
        /// </summary>
        private static List<PermissionAssociation> GetPermissionAssociationsByLoginId(string loginId)
        {
            throw new NotImplementedException();
        }
    }
}
