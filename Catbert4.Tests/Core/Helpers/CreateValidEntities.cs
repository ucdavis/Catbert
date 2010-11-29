using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Catbert4.Tests.Core.Helpers
{
    public static class CreateValidEntities
    {
        #region Helper Extension

        private static string Extra(this int? counter)
        {
            var extraString = "";
            if (counter != null)
            {
                extraString = counter.ToString();
            }
            return extraString;
        }

        #endregion Helper Extension

        //Sample:
        /*
        public static User User(int? counter)
        {
            var rtValue = new User();
            rtValue.Email = "Email" + counter.Extra();
            rtValue.LoginId = "LoginId" + counter.Extra();
            rtValue.FirstName = "FirstName" + counter.Extra();
            rtValue.LastName = "LastName" + counter.Extra();
            return rtValue;
        }
         */
    }
}
