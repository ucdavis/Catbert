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
using CAESArch.Core.Utils;

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
        get { return HttpContext.Current.User.Identity.Name; }
    }

    public CatbertWebService()
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    #region ServiceMethods

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
    public void InsertUserWithRoleAndUnit(ServiceUser serviceUser, string role, string unit, string application)
    {
        Check.Require(UserBLL.UserHasManagementRoleInApplication(application, CurrentServiceUser));

        User user = new User()
                        {
                            FirstName = serviceUser.FirstName,
                            LastName = serviceUser.LastName,
                            Email = serviceUser.Email,
                            LoginID = serviceUser.Login,
                            EmployeeID = serviceUser.EmployeeID,
                            Phone = serviceUser.Phone.AsNullIfWhiteSpace()
                        };

        UserBLL.InsertNewUserWithRoleAndUnit(user, role, unit, application, CurrentServiceUser);
    }

    [WebMethod]
    public CatbertUser GetUser(string login, string application)
    {
        EnsureCurrentUserCanManageLogin(application, login);

        User user = UserBLL.GetUser(login);

        if (user == null) return null; //make sure we have a real user

        List<User> users = new List<User>();
        users.Add(user); //Get the user, and add it to the user list

        List<CatbertUser> catbertUsers = ConvertFromUserList(users, application);

        if (catbertUsers.Count != 1) return null; //There should just be the one catbert user

        return catbertUsers[0];
    }

    [WebMethod]
    public bool AddUnit(string login, string application, string unitFIS)
    {
        EnsureCurrentUserCanManageLogin(application, login);

        return UserBLL.AssociateUnit(login, application, unitFIS, CurrentServiceUser);
    }

    [WebMethod]
    public bool DeleteUnit(string login, string application, string unitFIS)
    {
        EnsureCurrentUserCanManageLogin(application, login);

        return UserBLL.UnassociateUnit(login, application, unitFIS, CurrentServiceUser);
    }

    [WebMethod]
    public void AssociateRole(string login, string role, string application)
    {
        EnsureCurrentUserCanManageLogin(application, login);

        PermissionBLL.InsertPermission(application, role, login, CurrentServiceUser);
    }

    [WebMethod]
    public void DissociateRole(string login, string role, string application)
    {
        EnsureCurrentUserCanManageLogin(application, login);

        PermissionBLL.DeletePermission(application, role, login, CurrentServiceUser);
    }

    /// <summary>
    /// Get all users in this application, filtered by query, unit and role
    /// </summary>
    [WebMethod]
    public RecordSet GetUsers(string application, string search, string unit, string role, int page, int pagesize,
                              string sortname, string sortorder)
    {
        if (page <= 0 || pagesize <= 0) throw new ArgumentException("Page variables must be positive integers");

        string orderBy = string.IsNullOrEmpty(sortname)
                             ? "LastName DESC"
                             : string.Format("{0} {1}", sortname, sortorder);

        int totalUsers = 0;

        var users = UserBLL.GetByApplication(application, CurrentServiceUser, role, unit, search, page, pagesize,
                                             orderBy, out totalUsers);
        var serviceUsers = ConvertFromUserList(users, application);

        RecordSet grid = new RecordSet()
                             {
                                 page = page,
                                 total = (int) Math.Ceiling((double) totalUsers/pagesize),
                                 records = serviceUsers.Count
                             };

        foreach (var user in serviceUsers)
        {
            grid.rows.Add(user);
        }

        return grid;
    }

    private void EnsureCurrentUserCanManageLogin(string application, string loginToManage)
    {
        Check.Require(UserBLL.CanUserManageGivenLogin(application, CurrentServiceUser, loginToManage),
                      string.Format("{0} does not have access to manage {1} within the {2} application",
                                    CurrentServiceUser, loginToManage, application));
    }

    #endregion

    #region NonServiceMethods

    /// <summary>
    /// Finds all uses that meet the given criteri
    /// </summary>
    /// <returns>list of users, or an empty list if none found</returns>
    public List<ServiceUser> SearchNewUser(string eid, string firstName, string lastName, string login, string email)
    {
        List<ServiceUser> users = new List<ServiceUser>();

        foreach (var person in DirectoryServices.SearchUsers(eid, firstName, lastName, login, email))
        {
            users.Add(new ServiceUser()
                          {
                              EmployeeID = person.EmployeeID,
                              FirstName = person.FirstName,
                              LastName = person.LastName,
                              Login = person.LoginID,
                              Email = person.EmailAddress,
                              Phone = person.PhoneNumber
                          });
        }

        return users;
    }

    public int InsertNewUser(ServiceUser serviceUser)
    {
        User user = new User()
                        {
                            FirstName = serviceUser.FirstName,
                            LastName = serviceUser.LastName,
                            Email = serviceUser.Email,
                            LoginID = serviceUser.Login,
                            EmployeeID = serviceUser.EmployeeID,
                            Phone = serviceUser.Phone
                        };

        var insertedUser = UserBLL.InsertNewUser(user, CurrentServiceUser);
        return insertedUser.ID;
    }

    /// <summary>
    /// Verify that the given login exists in the database
    /// </summary>
    public bool VerifyUser(string login)
    {
        return UserBLL.VerifyUserExists(login);
    }

    #region Permissions

    /// <summary>
    /// Assigns the given role to the desired user within the application.  Returns true only on successfull assigning of permissions --
    /// If the permission is already assigned or if there are other errors, true will not be returned.
    /// </summary>
    public bool AssignPermissions(string login, string application, string role)
    {
        //If the credentials don't validate or the caller doesn't have access to this application, return false
        if (!this.ValidateApplicationPermission(CurrentServiceUser, application)) return false;

        Permission result = PermissionBLL.InsertPermission(application, role, login, CurrentServiceUser);

        return result != null; //true on non-null result
    }


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
    public bool PermissionExists(string login, string application, string role)
    {
        return PermissionBLL.PermissionExists(application, role, login, false);
    }

    #endregion

    #region Units

    public List<ServiceUnit> GetUnits()
    {
        List<ServiceUnit> serviceUnits = new List<ServiceUnit>();

        foreach (var unit in UnitBLL.GetAllUnits())
        {
            serviceUnits.Add(new ServiceUnit() {ID = unit.ID, Name = unit.ShortName, UnitFIS = unit.FISCode});
        }

        return serviceUnits;
    }


    public List<AutoCompleteData> GetUnitsAuto(string q /*query*/, int limit)
    {
        List<AutoCompleteData> auto = new List<AutoCompleteData>();

        foreach (var unit in UnitBLL.GetAllUnits())
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


    public List<ServiceUnit> GetUnitsByUser(string login, string application)
    {
        List<ServiceUnit> serviceUnits = new List<ServiceUnit>();

        foreach (var unit in UnitBLL.GetByUser(login, application))
        {
            serviceUnits.Add(new ServiceUnit() {ID = unit.ID, Name = unit.ShortName, UnitFIS = unit.FISCode});
        }

        return serviceUnits;
    }

    #endregion

    #region Roles

    public List<ServiceRole> GetRoles(string application)
    {
        List<ServiceRole> roles = new List<ServiceRole>();

        foreach (var role in RoleBLL.GetRolesByApplication(application))
        {
            if (!role.Role.Inactive) //only return active roles
            {
                roles.Add(new ServiceRole() {ID = role.Role.ID, Name = role.Role.Name});
            }
        }

        return roles;
    }
    
    public List<ServiceRole> GetRolesByUser(string application, string login)
    {
        List<ServiceRole> roles = new List<ServiceRole>();

        foreach (var role in PermissionBLL.GetRolesForUser(application, login))
        {
            roles.Add(new ServiceRole() {ID = role.ID, Name = role.Name});
        }

        return roles;
    }

    #endregion

    /// <summary>
    /// Convert a list of users from the Core class into a web service object.  Requires the application name for
    /// resolution of roles
    /// </summary>
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
            foreach (var unit in UnitBLL.GetByUser(user.LoginID, application))
            {
                catbertUser.Units.Add(new ServiceUnit() {ID = unit.ID, Name = unit.ShortName, UnitFIS = unit.FISCode});
            }

            //Grab this user's roles only for this application
            var roles = PermissionBLL.GetRolesForUser(application, catbertUser.Login);

            foreach (var role in roles)
            {
                if (role.Inactive == false) //only add in active permissions
                {
                    catbertUser.Roles.Add(new ServiceRole() {ID = role.ID, Name = role.Name});
                }
            }

            //Add this catbertUser to the main list
            catbertUsers.Add(catbertUser);
        }
        return catbertUsers;
    }

    #region Contact Information

    public bool SetEmail(string login, string emailAddress)
    {
        return UserBLL.SetEmail(login, emailAddress, CurrentServiceUser);
    }


    public bool SetPhoneNumber(string login, string phoneNumber)
    {
        return UserBLL.SetPhone(login, phoneNumber, CurrentServiceUser);
    }

    #endregion

    #endregion
}