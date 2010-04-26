using System;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Protocols;
using CAESDO.Catbert.BLL;
using Catbert.Services;
using CAESDO.Catbert.Core.Domain;
using System.Web;
using System.Collections.Specialized;
using System.Linq;

/// <summary>
/// Summary description for CatbertWebService
/// </summary>
[WebService(Namespace = "CAESDO.Services")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class CatbertWebService : System.Web.Services.WebService
{
    public string CurrentServiceUser
    {
        get
        {
            return "postit"; //Testing Only
            //return HttpContext.Current.User.Identity.Name;
        }
    }

    public CatbertWebService()
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    /// <summary>
    /// Finds all uses that meet the given criteri
    /// </summary>
    /// <returns>list of users, or an empty list if none found</returns>
    [WebMethod]
    public List<ServiceUser> SearchNewUser(string eid, string firstName, string lastName, string login)
    {
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

    [WebMethod]
    public int InsertNewUser(ServiceUser serviceUser)
    {
        User user = new User()
        {
            FirstName = serviceUser.FirstName,
            LastName = serviceUser.LastName,
            Email = serviceUser.Email,
            LoginID = serviceUser.Login,
            EmployeeID = serviceUser.EmployeeID
        };

        return UserBLL.InsertNewUser(user, CurrentServiceUser);
    }

    /// <summary>
    /// Verify that the given login exists in the database
    /// </summary>
    [WebMethod]
    public bool VerifyUser(string login)
    {    
        return UserBLL.VerifyUserExists(login);
    }

    [WebMethod]
    public CatbertUser GetUser(string login, string application)
    {
        User user = UserBLL.GetUser(login);

        if (user == null) return null; //make sure we have a real user

        List<User> users = new List<User>();
        users.Add(user);//Get the user, and add it to the user list

        List<CatbertUser> catbertUsers = ConvertFromUserList(users, application);

        if (catbertUsers.Count != 1) return null; //There should just be the one catbert user

        return catbertUsers[0];
    }

    #region Permissions

    /// <summary>
    /// Assigns the given role to the desired user within the application.  Returns true only on successfull assigning of permissions --
    /// If the permission is already assigned or if there are other errors, true will not be returned.
    /// </summary>
    [WebMethod]
    public bool AssignPermissions(string login, string application, string role)
    {
        //If the credentials don't validate or the caller doesn't have access to this application, return false
        if (!this.ValidateApplicationPermission(CurrentServiceUser, application)) return false;

        Permission result = PermissionBLL.InsertPermission(application, role, login, CurrentServiceUser);
        
        return result != null; //true on non-null result
    }

    [WebMethod]
    public bool DeletePermissions(string login, string application, string role)
    {
        if (!this.ValidateApplicationPermission(CurrentServiceUser, application)) return false;

        return PermissionBLL.DeletePermission(application, role, login, CurrentServiceUser);
    }

    /// <summary>
    /// Return true if the current service user has any permission on this application
    /// </summary>
    /// <param name="CurrentServiceUser"></param>
    /// <param name="application"></param>
    /// <returns></returns>
    private bool ValidateApplicationPermission(string CurrentServiceUser, string application)
    {
        return PermissionBLL.AnyPermissionExists(application, CurrentServiceUser, false);
    }

    /// <summary>
    /// Check to see if a permission exists and is active (works like is user in role)
    /// </summary>
    /// <returns>True if login has the correct active role in this application </returns>
    [WebMethod]
    public bool PermissionExists(string login, string application, string role)
    {
        return PermissionBLL.PermissionExists(application, role, login, false);
    }

    #endregion

    #region Units

    [WebMethod]
    public bool AddUnit(string login, string unitFIS)
    {
        return UserBLL.AssociateUnit(login, unitFIS, CurrentServiceUser);
    }

    [WebMethod]
    public bool DeleteUnit(string login, string unitFIS)
    {
        return UserBLL.UnassociateUnit(login, unitFIS, CurrentServiceUser);
    }

    [WebMethod]
    public List<ServiceUnit> GetUnits()
    {
        List<ServiceUnit> serviceUnits = new List<ServiceUnit>();

        foreach (var unit in UnitBLL.GetAll())
        {
            serviceUnits.Add(new ServiceUnit() { ID = unit.ID, Name = unit.ShortName, UnitFIS = unit.FISCode });
        }

        return serviceUnits;
    }

    [WebMethod]
    public List<AutoCompleteData> GetUnitsAuto(string q /*query*/, int limit)
    {
        List<AutoCompleteData> auto = new List<AutoCompleteData>();

        foreach (var unit in UnitBLL.GetAll())
        {
            if (unit.FISCode.ToLower().Contains(q) || unit.ShortName.ToLower().Contains(q))
            {
                auto.Add(
                    new AutoCompleteData(
                        unit.FISCode, //Result
                        string.Empty, //Value
                        new //Additional Info Anonymous Type
                            {
                                Name = unit.ShortName,
                                FIS = unit.FISCode
                            }
                    ));
            }
        }

        return auto;
    }


    [WebMethod]
    public List<ServiceUnit> GetUnitsByUser(string login)
    {
        List<ServiceUnit> serviceUnits = new List<ServiceUnit>();

        foreach (var unit in UnitBLL.GetByUser(login))
        {
            serviceUnits.Add(new ServiceUnit() { ID = unit.ID, Name = unit.ShortName, UnitFIS = unit.FISCode });
        }
        
        return serviceUnits;
    }

    #endregion

    #region Roles

    [WebMethod]
    public List<ServiceRole> GetRoles(string application)
    {
        List<ServiceRole> roles = new List<ServiceRole>();

        foreach (var role in RoleBLL.GetRolesByApplication(application))
        {
            if (!role.Inactive) //only return active roles
            {
                roles.Add(new ServiceRole() { ID = role.ID, Name = role.Name });
            }
        }

        return roles;
    }

    [WebMethod]
    public List<ServiceRole> GetRolesByUser(string application, string login)
    {
        List<ServiceRole> roles = new List<ServiceRole>();

        foreach (var role in PermissionBLL.GetRolesForUser(application, login))
        {
            roles.Add(new ServiceRole() { ID = role.ID, Name = role.Name });
        }

        return roles;
    }

    #endregion

    #region Applications

    [WebMethod]
    public ServiceApplication GetApplication(string application)
    {
        Application app = ApplicationBLL.GetByName(application);

        return new ServiceApplication(app);        
    }

    [WebMethod]
    public List<CatbertUser> GetUsersByApplication(string application)
    {
        List<CatbertUser> users = ConvertFromUserList(UserBLL.GetByApplication(application), application);

        return users;
    }

    [WebMethod]
    public GridData DataGetUsersByApplication(string application)
    {
        var users = UserBLL.GetByApplication(application);

        GridData grid = new GridData() { page = 1, total = 20 };

        foreach (var user in users)
        {
            grid.rows.Add(new GridDataRow()
            {
                id = user.ID,
                cell = new List<string> 
                {
                    user.LoginID, 
                    user.FirstName, 
                    user.LastName, 
                    user.EmployeeID, 
                    user.Email
                }
            });
        }

        return grid;
    }

    [WebMethod]
    public jqGridData jqGetUsersByApplication(string application)
    {
        var users = ConvertFromUserList(UserBLL.GetByApplication(application), application);

        jqGridData grid = new jqGridData() { page = 1, total = 2, records = 20 };

        foreach (var user in users)
        {
            grid.rows.Add(user);
        }
        
        grid.records = grid.rows.Count;

        return grid;
    }

    /// <summary>
    /// Get all users in this application, filtered by query, unit and role
    /// </summary>
    [WebMethod]
    public jqGridData jqGetUsers(string application, string search, string unit, string role, string sortname, string sortorder)
    {
        var users = ConvertFromUserList(UserBLL.GetByApplication(application), application);
        search = search == null ? null : search.ToLower();

        jqGridData grid = new jqGridData() { page = 1, total = 2, records = 20 };

        foreach (var user in users)
        {
            var badSearch = user.Email + user.FirstName + user.LastName + user.Login + user.EmployeeID;

            if ( search == null || badSearch.ToLower().Contains(search) ) grid.rows.Add(user);
        }

        return grid;
    }

    [WebMethod]
    public List<CatbertUser> GetUsersByApplicationRole(string application, string role)
    {
        List<CatbertUser> users = ConvertFromUserList(UserBLL.GetByApplicationRole(application, role), application);

        return users;
    }

    /// <summary>
    /// Convert a list of users from the Core class into a web service object.  Requires the application name for
    /// resolution of roles
    /// </summary>
    [WebMethod]
    private static List<CatbertUser> ConvertFromUserList(List<User> users, string application)
    {
        List<CatbertUser> catbertUsers = new List<CatbertUser>();

        foreach (var user in users)
        {
            //Add the user's basic info
            CatbertUser catbertUser = new CatbertUser()
            {
                UserID = user.ID,
                Login = user.LoginID,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                EmployeeID = user.EmployeeID,
                Roles = new List<ServiceRole>(),
                Units = new List<ServiceUnit>()
            };

            //Add in the units
            foreach (var unit in user.Units)
            {
                catbertUser.Units.Add(new ServiceUnit() { ID = unit.ID, Name = unit.ShortName, UnitFIS = unit.FISCode });
            }

            //Grab this user's roles only for this application
            var roles = PermissionBLL.GetRolesForUser(application, catbertUser.Login);

            foreach (var role in roles)
            {
                if (role.Inactive == false) //only add in active permissions
                {
                    catbertUser.Roles.Add(new ServiceRole() { ID = role.ID, Name = role.Name });
                }
            }

            //Add this catbertUser to the main list
            catbertUsers.Add(catbertUser);
        }
        return catbertUsers;
    }

    #endregion

    #region Contact Information

    [WebMethod]
    public bool SetEmail(string login, string emailAddress)
    {
        return UserBLL.SetEmail(login, emailAddress, CurrentServiceUser);
    }

    [WebMethod]
    public bool SetPhoneNumber(string login, string phoneNumber)
    {
        return UserBLL.SetPhone(login, phoneNumber, CurrentServiceUser);
    }

    #endregion
}