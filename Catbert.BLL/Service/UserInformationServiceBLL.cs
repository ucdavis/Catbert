using System;
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

            var userInformation = new UserInformation {LoginId = user.LoginID};

            return userInformation;
        }
    }
}
