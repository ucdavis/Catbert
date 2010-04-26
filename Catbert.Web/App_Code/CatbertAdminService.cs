using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using CAESDO.Catbert.BLL;
using CAESDO.Catbert.Core.Domain;
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

