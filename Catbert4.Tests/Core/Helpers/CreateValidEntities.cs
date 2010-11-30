using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Catbert4.Core.Domain;
using Catbert4.Tests.Core.Extensions;

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

        public static Application Application(int? counter, bool populateAllFields = false)
        {
            var rtValue = new Application();
            rtValue.Name = "Name" + counter.Extra();
            if (populateAllFields)
            {
                rtValue.Abbr = "x".RepeatTimes(50);
                rtValue.Location = "x".RepeatTimes(256);
                //rtValue.WebServiceHash = "x".RepeatTimes(100);
                //rtValue.Salt = "x".RepeatTimes(20);
            }

            return rtValue;
        }

        public static ApplicationRole ApplicationRole(int? counter)
        {
            var rtValue = new ApplicationRole();
            rtValue.Level = counter;

            return rtValue;
        }

        public static Role Role(int? counter)
        {
            var rtValue = new Role();
            rtValue.Name = "Name" + counter.Extra();
            rtValue.Inactive = false;

            return rtValue;
        }

    }
}
