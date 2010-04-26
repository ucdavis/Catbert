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

        return UserBLL.InsertNewUser(user);
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

    [WebMethod, SoapHeader("secureCTX", Required = true, Direction = SoapHeaderDirection.InOut)]
    public bool AssignPermissions(string login, string application, int role)
    {
        throw new NotImplementedException();
    }

    [WebMethod, SoapHeader("secureCTX", Required = true, Direction = SoapHeaderDirection.InOut)]
    public bool DeletePermissions(string login, string application, int role)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Units

    [WebMethod, SoapHeader("secureCTX", Required = true, Direction = SoapHeaderDirection.InOut)]
    public bool AddUnit(string login, int unitID)
    {
        throw new NotImplementedException();
    }

    [WebMethod, SoapHeader("secureCTX", Required = true, Direction = SoapHeaderDirection.InOut)]
    public bool DeleteUnit(string login, int unitID)
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
        throw new NotImplementedException();
    }

    private void EnsureCredentials(SecurityContext secureCTX)
    {
        if (!ValidateCredentials(secureCTX)) throw new ApplicationException("Authorization Failed");
    }

    private bool ValidateApplicationPermission(SecurityContext secureCTX, int applicationID)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Finds the applicationID from a given application
    /// </summary>
    /// <param name="application"></param>
    /// <returns></returns>
    private int GetApplicationID(string application)
    {
        throw new NotImplementedException();
    }

    #endregion
}

public class SecurityContext : SoapHeader
{
    public string UserID;
    public string Password;
}