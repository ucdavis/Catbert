using System;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Protocols;
using CAESDO.Catbert.BLL;
using Catbert.Services;
using CAESDO.Catbert.Core.Domain;
using System.Web;

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
            return HttpContext.Current.User.Identity.Name;
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

    private bool ValidateApplicationPermission(string CurrentServiceUser, string application)
    {
        throw new NotImplementedException();
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
    public List<CatbertUser> GetUsersByApplication(string application)
    {
        List<CatbertUser> users = ConvertFromUserList(UserBLL.GetByApplication(application), application);

        return users;
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