using System;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Protocols;
using CAESDO.Catbert.BLL;
using Catbert.Services;
using CAESDO.Catbert.Core.Domain;

/// <summary>
/// Summary description for CatbertWebService
/// </summary>
[WebService(Namespace = "CAESDO.Services")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class CatbertWebService : System.Web.Services.WebService
{
    public SecurityContext secureCTX;
    
    public CatbertWebService()
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    /// <summary>
    /// Finds all uses that meet the given criteri
    /// </summary>
    /// <returns>list of users, or an empty list if none found</returns>
    [WebMethod, SoapHeader("secureCTX", Required = true, Direction = SoapHeaderDirection.InOut)]
    public List<ServiceUser> SearchNewUser(string eid, string firstName, string lastName, string login)
    {
        EnsureCredentials(secureCTX);

        List<ServiceUser> users = new List<ServiceUser>();

        foreach (var person in DirectoryServices.SearchUsers(eid, firstName, lastName, login))
        {
            users.Add(new ServiceUser()
            {
                EmployeeID = person.EmployeeID,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Login = person.LoginID,
                Email = person.EmailAddress
            });
        }

        return users;
    }

    [WebMethod, SoapHeader("secureCTX", Required = true, Direction = SoapHeaderDirection.InOut)]
    public int InsertNewUser(ServiceUser serviceUser)
    {
        EnsureCredentials(secureCTX);

        User user = new User()
        {
            FirstName = serviceUser.FirstName,
            LastName = serviceUser.LastName,
            Email = serviceUser.Email,
            LoginID = serviceUser.Login,
            EmployeeID = serviceUser.EmployeeID
        };

        return UserBLL.InsertNewUser(user, secureCTX.UserID);
    }

    /// <summary>
    /// Verify that the given login exists in the database
    /// </summary>
    [WebMethod, SoapHeader("secureCTX", Required = true, Direction = SoapHeaderDirection.InOut)]
    public bool VerifyUser(string login)
    {
        EnsureCredentials(secureCTX);

        return UserBLL.VerifyUserExists(login);
    }

    #region Permissions

    /// <summary>
    /// Assigns the given role to the desired user within the application.  Returns true only on successfull assigning of permissions --
    /// If the permission is already assigned or if there are other errors, true will not be returned.
    /// </summary>
    [WebMethod, SoapHeader("secureCTX", Required = true, Direction = SoapHeaderDirection.InOut)]
    public bool AssignPermissions(string login, string application, string role)
    {
        EnsureCredentials(secureCTX);

        //If the credentials don't validate or the caller doesn't have access to this application, return false
        if (!this.ValidateApplicationPermission(secureCTX, GetApplicationID(application))) return false;

        Permission result = PermissionBLL.InsertPermission(application, role, login, secureCTX.UserID);
        
        return result != null; //true on non-null result
    }

    [WebMethod, SoapHeader("secureCTX", Required = true, Direction = SoapHeaderDirection.InOut)]
    public bool DeletePermissions(string login, string application, string role)
    {
        EnsureCredentials(secureCTX);

        if (!this.ValidateApplicationPermission(secureCTX, GetApplicationID(application))) return false;

        return PermissionBLL.DeletePermission(application, role, login, secureCTX.UserID);
    }

    /// <summary>
    /// Check to see if a permission exists and is active (works like is user in role)
    /// </summary>
    /// <returns>True if login has the correct active role in this application </returns>
    [WebMethod, SoapHeader("secureCTX", Required = true, Direction = SoapHeaderDirection.InOut)]
    public bool PermissionExists(string login, string application, string role)
    {
        return PermissionBLL.PermissionExists(application, role, login, false);
    }

    #endregion

    #region Units

    [WebMethod, SoapHeader("secureCTX", Required = true, Direction = SoapHeaderDirection.InOut)]
    public bool AddUnit(string login, string unitFIS)
    {
        throw new NotImplementedException();
    }

    [WebMethod, SoapHeader("secureCTX", Required = true, Direction = SoapHeaderDirection.InOut)]
    public bool DeleteUnit(string login, string unitFIS)
    {
        throw new NotImplementedException();
    }

    [WebMethod, SoapHeader("secureCTX", Required = true, Direction = SoapHeaderDirection.InOut)]
    public List<ServiceUnit> GetUnits()
    {
        throw new NotImplementedException();
    }

    [WebMethod, SoapHeader("secureCTX", Required = true, Direction = SoapHeaderDirection.InOut)]
    public List<ServiceUnit> GetUnitsByUser(string loginID)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Roles

    [WebMethod, SoapHeader("secureCTX", Required = true, Direction = SoapHeaderDirection.InOut)]
    public List<ServiceRole> GetRoles(string application)
    {
        throw new NotImplementedException();
    }

    [WebMethod, SoapHeader("secureCTX", Required = true, Direction = SoapHeaderDirection.InOut)]
    public List<ServiceRole> GetRolesByUser(string application, string login)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Applications

    [WebMethod, SoapHeader("secureCTX", Required = true, Direction = SoapHeaderDirection.InOut)]
    public List<CatbertUser> GetUsersByApplications(string application)
    {
        throw new NotImplementedException();
    }

    [WebMethod, SoapHeader("secureCTX", Required = true, Direction = SoapHeaderDirection.InOut)]
    public List<CatbertUser> GetUsersByApplicationRole(string application, int roleID)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Contact Information

    [WebMethod, SoapHeader("secureCTX", Required = true, Direction = SoapHeaderDirection.InOut)]
    public bool AddEmail(string login, string emailAddress, int emailTypeID)
    {
        throw new NotImplementedException();
    }

    [WebMethod, SoapHeader("secureCTX", Required = true, Direction = SoapHeaderDirection.InOut)]
    public bool AddPhoneNumber(string login, string phoneNumber, int phoneType)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Private

    private bool ValidateCredentials(SecurityContext secureCTX)
    {
        return true; //TODO: TESTING ONLY
        //throw new NotImplementedException();
    }

    private void EnsureCredentials(SecurityContext secureCTX)
    {
        if (!ValidateCredentials(secureCTX)) throw new ApplicationException("Authorization Failed");
    }

    private bool ValidateApplicationPermission(SecurityContext secureCTX, int applicationID)
    {
        return true; //TODO: TESTING ONLY!!!
        //throw new NotImplementedException();
    }

    /// <summary>
    /// Finds the applicationID from a given application name
    /// </summary>
    /// <param name="application"></param>
    /// <returns></returns>
    private int GetApplicationID(string application)
    {
        return ApplicationBLL.GetID(application);
    }

    #endregion
}

public class SecurityContext : SoapHeader
{
    public string UserID;
    public string Password;
}