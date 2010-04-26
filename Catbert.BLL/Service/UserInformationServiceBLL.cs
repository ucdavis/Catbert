using System;
using CAESDO.Catbert.Core.ServiceObjects;
using CAESArch.Core.Utils;

namespace CAESDO.Catbert.BLL.Service
{
    public class UserInformationServiceBLL
    {
        public static UserInformation GetInformationByLoginId(string loginId)
        {
            Check.Require(!string.IsNullOrEmpty(loginId));

            throw new NotImplementedException();
        }
    }
}
