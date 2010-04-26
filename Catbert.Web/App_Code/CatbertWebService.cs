using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using CAESDO.Catbert.Core.Domain;
using Catbert.Services;

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
    public List<Users> SearchNewUser(string eid, string firstName, string lastName, string login)
    {
        throw new NotImplementedException(); 
    }

    [WebMethod, SoapHeader("secureCTX", Required = true, Direction = SoapHeaderDirection.InOut)]
    public int InsertNewUser(Users user)
    {
        throw new NotImplementedException(); 
    }

    [WebMethod, SoapHeader("secureCTX", Required = true, Direction = SoapHeaderDirection.InOut)]
    public bool VerifyUser(string loginID)
    {
        throw new NotImplementedException();
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
    public List<Units> GetUnits()
    {
        throw new NotImplementedException();
    }

    [WebMethod, SoapHeader("secureCTX", Required = true, Direction = SoapHeaderDirection.InOut)]
    public List<Units> GetUnitsByUser(string loginID)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Roles

    [WebMethod, SoapHeader("secureCTX", Required = true, Direction = SoapHeaderDirection.InOut)]
    public List<Roles> GetRoles(string application)
    {
        throw new NotImplementedException();
    }

    [WebMethod, SoapHeader("secureCTX", Required = true, Direction = SoapHeaderDirection.InOut)]
    public List<Roles> GetRolesByUser(string application, string login)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Applications

    [WebMethod, SoapHeader("secureCTX", Required = true, Direction = SoapHeaderDirection.InOut)]
    public List<CatbertUsers> GetUsersByApplications(string application)
    {
        throw new NotImplementedException();
    }

    [WebMethod, SoapHeader("secureCTX", Required = true, Direction = SoapHeaderDirection.InOut)]
    public List<CatbertUsers> GetUsersByApplicationRole(string application, int roleID)
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

    [WebMethod, SoapHeader("secureCTX", Required = true, Direction = SoapHeaderDirection.InOut)]
    public List<EmailTypes> GetEmailTypes()
    {
        throw new NotImplementedException();
    }

    [WebMethod, SoapHeader("secureCTX", Required = true, Direction = SoapHeaderDirection.InOut)]
    public List<PhoneTypes> GetPhoneTypes()
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Private

    private bool ValidateCredentials(SecurityContext secureCTX)
    {
        throw new NotImplementedException();
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