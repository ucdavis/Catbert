using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.ComponentModel;
using System.Net;

namespace CAESDO.Catbert.Test
{
    /// <summary>
    /// Summary description for CatbertManager
    /// </summary>
    public class CatbertManager
    {
        static readonly string AppName = "Catbert";
        public static CatbertService.CatbertWebServiceSoapClient catops = new CatbertService.CatbertWebServiceSoapClient();

        public CatbertService.SecurityContext securityContext; // = new CatbertService.SecurityContext() { Password = "CA0C898E-0287-40FE-AE76-554039B932FD", UserID = "postit" };

        public static CatbertService.ServiceUnit[] GetUnits()
        {
            return catops.GetUnits(ref GetSecurityContext());
        }

        /*

        public static CatOps.Roles[] GetRoles()
        {
            SetSecurityContext();

            return catops.GetRoles(AppName);
        }

        public static CatOps.Users[] SearchNewUsersByLogin(string EmployeeID, string FirstName, string LastName, string LoginID)
        {
            SetSecurityContext();

            return catops.SearchNewUser(EmployeeID, FirstName, LastName, LoginID);
        }

        public static CatOps.Users[] SearchCampusUser(string loginID)
        {
            SetSecurityContext();

            return catops.SearchNewUser(null, null, null, loginID);
            //return catops.SearchCampusNewUser(loginID, HASH);
        }

        public static CatOps.Users[] SearchNewUsersByLogin(string login)
        {
            SetSecurityContext();

            return catops.SearchNewUser(null, null, null, login);
        }

        public static bool AddUserToRole(CatOps.Users user, CatOps.Roles role)
        {
            SetSecurityContext();

            return catops.AssignPermissions(user.Login, AppName, role.RoleID);
        }

        public static bool AddUserToRole(string login, int roleID)
        {
            SetSecurityContext();

            return catops.AssignPermissions(login, AppName, roleID);
        }

        public static bool RemoveUserFromRole(int roleID, string login)
        {
            SetSecurityContext();

            return catops.DeletePermissions(login, AppName, roleID);
        }

        public static bool AddUserToUnit(string login, int UnitID)
        {
            SetSecurityContext();

            return catops.AddUnit(login, UnitID);
        }

        public static bool RemoveUserFromUnit(string login, int UnitID)
        {
            SetSecurityContext();

            return catops.DeleteUnit(login, UnitID);
        }

        public static CatOps.Roles[] GetRolesByUser(string login)
        {
            SetSecurityContext();

            return catops.GetRolesByUser(AppName, login);
        }

        public static CatOps.CatbertUsers[] GetUsersInApplication()
        {
            SetSecurityContext();

            return catops.GetUsersByApplications(AppName);
        }

        public static int InsertNewUser(string login)
        {
            SetSecurityContext();

            CatOps.Users[] newUsers = CatbertManager.SearchNewUsersByLogin(login);
            if (newUsers.Length != 1)
                return -1;

            return catops.InsertNewUser(newUsers[0]);
        }

        public static int InsertNewUser(CatOps.Users user)
        {
            SetSecurityContext();

            return catops.InsertNewUser(user);
        }

        public static bool VerifyUser(string login)
        {
            SetSecurityContext();

            return catops.VerifyUser(login);
        }
         */

        public CatbertManager()
        {

        }

        public static CatbertService.SecurityContext GetSecurityContext()
        {
            return new CAESDO.Catbert.Test.CatbertService.SecurityContext() { UserID = "", Password = "" };
        }

        public static void SetSecurityContext()
        {
            
            /*
            CatOps.SecurityContext sc = new CatOps.SecurityContext();

            string username = HttpContext.Current.User.Identity.Name;

            User user = UserBLL.GetCurrent();

            sc.userID = username;
            sc.password = user.UserKey.ToString();

            System.Net.ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy();

            catops.SecurityContextValue = sc;
             */
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class SetCabertSecurityContextAttribute : Attribute
    {
        public SetCabertSecurityContextAttribute(CatbertService.SecurityContext service)
        {
            service.UserID = "postit";
            service.Password = "CA0C898E-0287-40FE-AE76-554039B932FD";

            /*
            CatbertService.SecurityContext sc = new CatbertService.SecurityContext();
            
            sc.UserID = "postit";
            sc.Password = "CA0C898E-0287-40FE-AE76-554039B932FD";

            System.Net.ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy();

            
            service.SecurityContextValue = sc;
             */
        }
    }

    // Accept all certificates even self signed
    public class TrustAllCertificatePolicy : System.Net.ICertificatePolicy
    {
        public TrustAllCertificatePolicy()
        { }

        public bool CheckValidationResult(ServicePoint sp,
         System.Security.Cryptography.X509Certificates.X509Certificate cert, WebRequest req, int problem)
        {
            return true;
        }
    }

}