using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using CAESDO.Catbert.BLL;
using CAESDO.Catbert.BLL.Service;
using CAESDO.Catbert.Core.Domain;
using CAESDO.Catbert.Core.ServiceObjects;
using Catbert.Services;

/// <summary>
/// Summary description for CatbertAdminService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class CatbertAdminService : WebService
{
    public string CurrentServiceUser
    {
        get
        {
            return HttpContext.Current.User.Identity.Name;
        }
    }

    [WebMethod]
    public RecordSet GetUsers(string application, string search, string unit, string role, int page, int pagesize, string sortname, string sortorder)
    {
        if (page <= 0 || pagesize <= 0) throw new ArgumentException("Page variables must be positive integers");

        string orderBy = string.IsNullOrEmpty(sortname) ? "LastName DESC" : string.Format("{0} {1}", sortname, sortorder);

        int totalUsers = 0;

        var users = UserBLL.GetAllByCriteria(application, search, page, pagesize, orderBy, out totalUsers);

        var serviceUsers = ConvertFromUserList(users);

        var grid = new RecordSet() { page = page, total = (int)Math.Ceiling((double)totalUsers / pagesize), records = serviceUsers.Count };

        foreach (var user in serviceUsers)
        {
            grid.rows.Add(user);
        }

        return grid;
    }

    [WebMethod]
    public List<AutoCompleteData> GetUsersAutoComplete(string application, string q /*query*/, int limit)
    {
        var users = UserBLL.GetAllByCriteria(application, q, 1 /* Start at the first record */, limit);

        var auto = new List<AutoCompleteData>();

        foreach (var user in users)
        {
            auto.Add(new AutoCompleteData(user.LoginID, string.Empty,
                new
                {
                    Name = string.Format("{0} {1}", user.FirstName, user.LastName),
                    Login = user.LoginID,
                    Email = user.Email
                }
            ));
        }

        return auto;
    }

    /// <summary>
    /// We are going to search for the user with the given term, currently can be email or loginID
    /// </summary>
    [WebMethod]
    public ServiceUser FindUser(string searchTerm)
    {
        var foundUser = DirectoryServices.FindUser(searchTerm);

        if (foundUser == null) return null;

        var serviceUser = new ServiceUser()
        {
            EmployeeID = foundUser.EmployeeID,
            FirstName = foundUser.FirstName,
            LastName = foundUser.LastName,
            Login = foundUser.LoginID,
            Email = foundUser.EmailAddress,
            Phone = foundUser.PhoneNumber
        };

        return serviceUser;
    }

    [WebMethod]
    public UserInformation GetUserInfo(string loginId)
    {
        return UserInformationServiceBLL.GetInformationByLoginId(loginId);
    }

    /// <summary>
    /// Creates a list of service users with only the top level user information filled out.
    /// </summary>
    private static List<ServiceUser> ConvertFromUserList(List<User> users)
    {
        var serviceUsers = new List<ServiceUser>();

        foreach (var user in users)
        {
            var serviceUser = new ServiceUser
                          {
                              Email = user.Email,
                              FirstName = user.FirstName,
                              LastName = user.LastName,
                              Login = user.LoginID
                          };

            serviceUsers.Add(serviceUser);
        }

        return serviceUsers;
    }
}

