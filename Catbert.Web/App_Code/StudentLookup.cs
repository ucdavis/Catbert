using System;
using System.Web.Services;
using CAESDO.Catbert.BLL;
using System.Web.Configuration;

/// <summary>
/// Summary description for StudentLookup
/// </summary>
[WebService]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
 public class StudentLookup : WebService
{
    public string SecurityToken
    {
        get { return WebConfigurationManager.AppSettings["StudentLookupSecurityToken"]; }
    }

    [WebMethod]
    public string FindStudent(string studentId, string securityToken)
    {
        if (securityToken != SecurityToken) throw new ArgumentException("The Security Token Is Invalid");

        var student = DirectoryServices.FindStudent(studentId);

        return student == null ? null : student.LoginID;
    }
}

