using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Linq;
using CAESArch.BLL;
using CAESDO.Catbert.BLL;
using CAESDO.Catbert.BLL.Service;
using CAESDO.Catbert.Core.Domain;
using CAESDO.Catbert.Core.ServiceObjects;
using Catbert.Services;
using CAESArch.Core.Utils;

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

        int totalUsers;

        var users = UserBLL.GetAllByCriteria(application, search, page, pagesize, orderBy, out totalUsers);

        var serviceUsers = ConvertFromUserList(users);

        var grid = new RecordSet { page = page, total = (int)Math.Ceiling((double)totalUsers / pagesize), records = serviceUsers.Count };

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
                    user.Email
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

        var serviceUser = new ServiceUser
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

    [WebMethod]
    public void UpdateUserInfo(ServiceUser serviceUser)
    {
        Check.Require(serviceUser != null);

        UserBLL.SetUserInfo(serviceUser.Login, serviceUser.FirstName, serviceUser.LastName, serviceUser.Phone,
                            serviceUser.Email, CurrentServiceUser);
    }

    [WebMethod]
    public List<string> GetRolesForApplication(string application)
    {
        //Get back all of the application roles
        var appRoles = RoleBLL.GetRolesByApplication(application);

        //Just pull out the list of names
        return appRoles.OrderBy(role => role.Role.Name).Select(role => role.Role.Name).ToList();
    }

    [WebMethod]
    public void InsertUserWithRoleAndUnit(ServiceUser serviceUser, string role, string unit, string application)
    {
        var user = new User
        {
            FirstName = serviceUser.FirstName,
            LastName = serviceUser.LastName,
            Email = serviceUser.Email,
            LoginID = serviceUser.Login,
            EmployeeID = serviceUser.EmployeeID,
            Phone = serviceUser.Phone
        };

        UserBLL.InsertNewUserWithRoleAndUnit(user, role, unit, application, CurrentServiceUser);
    }

    #region Associations

    [WebMethod]
    public void AssociateUnit(string login, string application, string unitFIS)
    {
        UserBLL.AssociateUnit(login, application, unitFIS, CurrentServiceUser);
    }

    [WebMethod]
    public void DissociateUnit(string login, string application, string unitFIS)
    {
        UserBLL.UnassociateUnit(login, application, unitFIS, CurrentServiceUser);
    }

    [WebMethod]
    public void AssociateRole(string login, string application, string role)
    {
        PermissionBLL.InsertPermission(application, role, login, CurrentServiceUser);
    }

    [WebMethod]
    public void DissociateRole(string login, string application, string role)
    {
        PermissionBLL.DeletePermission(application, role, login, CurrentServiceUser);
    }

    #endregion

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

