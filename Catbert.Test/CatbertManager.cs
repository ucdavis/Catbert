using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.ComponentModel;
using System.Net;
using CAESDO.Catbert.Test.CatbertService;

namespace CAESDO.Catbert.Test
{
    /// <summary>
    /// Summary description for CatbertManager
    /// </summary>
    public class CatbertManager
    {
        static readonly string AppName = "Catbert";
        public static CatbertService.CatbertWebServiceSoapClient catops = new CatbertService.CatbertWebServiceSoapClient();

        public static CatbertUser GetUser(string login)
        {
            return catops.GetUser(login, AppName);
        }

        public static bool SetEmail(string login, string emailAddress)
        {
            return catops.SetEmail(login, emailAddress);
        }

        public static bool SetPhone(string login, string phoneNumber)
        {
            return catops.SetPhoneNumber(login, phoneNumber);
        }

        public static CatbertUser[] GetUserInApplicationRole(string role)
        {
            return catops.GetUsersByApplicationRole(AppName, role);
        }

        public static CatbertUser[] GetUsersInApplication()
        {
            return catops.GetUsersByApplication(AppName);
        }

        public static ServiceRole[] GetRoles()
        {
            return catops.GetRoles(AppName);
        }

        public static ServiceRole[] GetRolesByUser(string login)
        {
            return catops.GetRolesByUser(AppName, login);
        }

        public static bool AddUserToUnit(string login, string unitFIS)
        {
            return catops.AddUnit(login, unitFIS);
        }

        public static bool RemoveUserFromUnit(string login, string unitFIS)
        {
            return catops.DeleteUnit(login, unitFIS);
        }

        public static ServiceUnit[] GetUnitsForUser(string login)
        {
            return catops.GetUnitsByUser(login);
        }

        public static ServiceUnit[] GetAllUnits()
        {
            return catops.GetUnits();
        }
        
        public static bool AddUserToRole(CatbertService.ServiceUser user, CatbertService.ServiceRole role)
        {
            return catops.AssignPermissions(user.Login, AppName, role.Name);
        }

        public static bool AddUserToRole(string login, string role)
        {
            return catops.AssignPermissions(login, AppName, role);
        }

        public static bool RemoveUserFromRole(string login, string role)
        {
            return catops.DeletePermissions(login, AppName, role);
        }

        public static bool IsUserInRole(string login, string role)
        {
            return catops.PermissionExists(login, AppName, role);
        }

        public static ServiceUser[] SearchNewUsers(string EmployeeID, string FirstName, string LastName, string Login)
        {
            return catops.SearchNewUser(EmployeeID, FirstName, LastName, Login);
        }

        public static bool VerifyUser(string login)
        {   
            return catops.VerifyUser(login);
        }

        public CatbertManager()
        {

        }
    }
}