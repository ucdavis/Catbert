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

        public static Audit Audit(int? counter, bool populateAllFields = false)
        {
            var rtValue = new Audit();
            rtValue.ObjectName = "ObjectName" + counter.Extra();
            rtValue.SetActionCode(AuditActionType.Create);
            rtValue.Username = "Username" + counter.Extra();
            rtValue.AuditDate = DateTime.Now;
            if (populateAllFields)
            { 
                rtValue.ObjectId = "x".RepeatTimes(50);
            }
            return rtValue;
        }

        public static Permission Permission(int? counter)
        {
            var rtValue = new Permission();
            rtValue.User = new User();
            rtValue.Application = new Application();
            rtValue.Role = new Role();
            rtValue.Inactive = false;
            

            return rtValue;
        }

        public static User User(int? counter, bool populateAllFields = false)
        {
            var rtValue = new User();
            rtValue.LoginId = "LoginId" + counter.Extra();
            var localCounter = 99;
            if (counter != null)
                localCounter = (int)counter;
            rtValue.UserKey = SpecificGuid.GetGuid(localCounter);
            rtValue.Inactive = false;
            if (populateAllFields)
            {
                rtValue.Email = "test@testy.com";
                rtValue.EmployeeId = "x".RepeatTimes(9);
                rtValue.FirstName = "x".RepeatTimes(50);
                rtValue.LastName = "x".RepeatTimes(50);
                //rtValue.Phone = "555-555-5555"; //Not Sure
                rtValue.StudentId = "x".RepeatTimes(9);
            }

            return rtValue;
        }

        public static School School(int? counter)
        {
            var rtValue = new School();
            rtValue.Abbreviation = "Abbr" + counter.Extra();
            rtValue.LongDescription = "LongDescription" + counter.Extra();
            rtValue.ShortDescription = "ShortDescription" + counter.Extra();
            var stringId = "99";
            if (counter != null)
            {
                stringId = counter.ToString();
            }
            rtValue.SetId(stringId);
            return rtValue;
        }

        public static Unit Unit(int? counter, bool populateAllFields = false)
        {
            var rtValue = new Unit();
            rtValue.FullName = "FullName" + counter.Extra();
            rtValue.FisCode = "F" + counter.Extra();
            rtValue.School = new School();
            if (populateAllFields)
            {
                rtValue.ShortName = "x".RepeatTimes(50);
                rtValue.PpsCode = counter.Extra();
            }
            return rtValue;
        }

        public static UnitAssociation UnitAssociation(int? counter)
        {
            var rtValue = new UnitAssociation();
            rtValue.User = new User();
            rtValue.Application = new Application();
            rtValue.Unit = new Unit();
            rtValue.Inactive = false;


            return rtValue;
        }
    }
}
