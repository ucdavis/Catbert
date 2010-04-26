using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Configuration;
using System.Collections;
using CAESOps;

namespace CAESDO
{
    /// <summary>
    /// CAESDORoleProvider inherits from the generic RoleProvider provided by ASP.NET that is part
    /// of their provider model (which includes Membership, Role and Profile Providers)
    /// </summary>
    /// <remarks>Could also inherit from the SqlRoleProvider -- maybe worth looking at</remarks>
    /// <see cref="http://msdn.microsoft.com/asp.net/downloads/providers/"/>
    public class CAESDORoleProvider : RoleProvider
    {
        //private vars
        private string _applicationName;
        private string _connectionStringKey;
        private string _description;
        private string _name;
        private string _connectionString;
        private DataOps _dops;

        //public accessors
        public override string Description
        {
            get
            {
                return _description;
            }
        }
        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }
        public override string ApplicationName
        {
            get
            {
                return _applicationName;
            }
            set
            {
                _applicationName = value;
            }
        }
        public override string Name
        {
            get
            {
                return _name;
            }
        }
        private DataOps Dops
        {
            get { return _dops; }
            set { _dops = value; }
        }
        
        #region Methods

        /// <summary>
        /// Initialized from the web.config file when the application loads for the first time.  
        /// </summary>
        /// <param name="name">The name of the role provider</param>
        /// <param name="config">Collection of keys.  They need to include ApplicationName, ConnectionString.
        /// Description is optional</param>
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            //Make sure we have a valid config collection
            if (config == null)
                throw new ArgumentNullException("config");

            //If no name was given, we'll give it a generic name
            if (string.IsNullOrEmpty(name))
                name = "CAESDORoleProvider";

            //If no description is given, we'll give it a generic one
            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "CAESDO Role Provider");
            }

            //Initialize the RoleProvider base
            base.Initialize(name, config);

            //Now initialize our vars (these values should get overwritten next)
            _applicationName = "DefaultApp";
            _name = name;

            //Loop through the config collection and set our private variables
            foreach (string key in config.Keys)
            {
                switch (key.ToLower())
                {
                    case "applicationname":
                        ApplicationName = config[key];
                        break;
                    case "connectionstring":
                        _connectionStringKey = config[key];
                        break;
                    case "description":
                        _description = config[key];
                        break;
                }
            }

            //Setup the dataops -- We probably want to change the 'connection string' to a connection string key 
            //pointing to a web.config connection strings section
            //_connectionString = WebConfigurationManager.ConnectionStrings[_connectionStringKey].ToString();
            _dops = new DataOps();
            _dops.ConnectionString = _connectionStringKey;
        }

        /// <summary>
        /// Determine if a user is in the supplied role in this application.
        /// </summary>
        /// <param name="username">LoginID</param>
        /// <param name="roleName">RoleName</param>
        /// <returns>True if the user is in the role, else false</returns>
        public override bool IsUserInRole(string username, string roleName)
        {
            //Start by resetting dops
            _dops.ResetDops();
            _dops.Sproc = "usp_getRolesInAppByLoginID";

            //We only want to search in the context of this application
            _dops.SetParameter("@AppName", _applicationName, "IN");
            _dops.SetParameter("@LoginID", username, "IN");

            ArrayList fields = new ArrayList();
            fields.Add("Role");

            ArrayList resutls = _dops.get_arrayList(fields);

            //Check to see if the user's roles include the one we want
            if (resutls.Contains(roleName))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Gets all roles in the application context
        /// </summary>
        /// <returns>Roles</returns>
        public override string[] GetAllRoles()
        {
            _dops.ResetDops();
            _dops.Sproc = "usp_getAllRolesInApp";

            //Search in the application context
            _dops.SetParameter("@AppName", _applicationName, "IN");

            ArrayList fields = new ArrayList();
            fields.Add("Role");

            //Get back the array list, and cast it to a string[] using the ToArray(typeof(string)) construct
            return (string[])_dops.get_arrayList(fields).ToArray(typeof(string));

        }

        /// <summary>
        /// Gets all roles for the user in the context of this application
        /// </summary>
        /// <param name="username">LoginID to get the roles for</param>
        public override string[] GetRolesForUser(string username)
        {
            _dops.ResetDops();
            _dops.Sproc = "usp_getRolesInAppByLoginID";

            //Search in application context
            _dops.SetParameter("@AppName", _applicationName, "IN");
            _dops.SetParameter("@LoginID", username, "IN");

            ArrayList fields2 = new ArrayList();
            fields2.Add("Role");

            //Cast arraylist to string[]
            return (string[])_dops.get_arrayList(fields2).ToArray(typeof(string));
        }

        /// <summary>
        /// Gets all users that are in the current role in the application context
        /// </summary>
        public override string[] GetUsersInRole(string roleName)
        {
            _dops.ResetDops();
            _dops.Sproc = "usp_getUsersInRole";

            //Search in application context for the specific role
            _dops.SetParameter("@AppName", _applicationName, "IN");
            _dops.SetParameter("@RoleName", roleName, "IN");

            ArrayList fields = new ArrayList();
            fields.Add("LoginID");

            return (string[])_dops.get_arrayList(fields).ToArray(typeof(string));
        }

        /// <summary>
        /// Find out if a role exists within an application context
        /// </summary>
        /// <param name="roleName">RoleName</param>
        /// <returns>True if the role exists in the application context, else false</returns>
        /// <remarks>A return of false doesn't mean that the role doesn't exist in the db, just that it doesn't
        /// exists withing the context of this application</remarks>
        public override bool RoleExists(string roleName)
        {
            _dops.ResetDops();
            _dops.Sproc = "usp_getAllRolesInApp";

            //Pass application name
            _dops.SetParameter("@AppName", _applicationName, "IN");

            ArrayList fields = new ArrayList();
            fields.Add("Role");

            ArrayList resutls = _dops.get_arrayList(fields);

            //Return whether or not the desired role is in the list
            if (resutls.Contains(roleName))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Creates the role 'roleName' in the database, but doesn't assign it to any user.
        /// </summary>
        /// <param name="roleName">The role to add</param>
        /// <remarks>Once the role is created, if you do not assign it to a user the role still doesn't
        /// 'exist' as far as the application is concerned</remarks>
        public override void CreateRole(string roleName)
        {
            _dops.ResetDops();
            _dops.Sproc = "usp_insertRole";

            //Insert the role
            _dops.SetParameter("@RoleName", roleName, "IN");
            
            //Will raise an error if the role is already in the db
            _dops.Execute_Sql();

        }

        /// <summary>
        /// Deletes a role only if there are no users in it
        /// </summary>
        /// <param name="roleName">The role to delete</param>
        /// <param name="throwOnPopulatedRole">Not implemented</param>
        /// <returns>Returns true is role was deleted, false if the role still has users in it</returns>
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            _dops.ResetDops();
            _dops.Sproc = "usp_deleteEmptyRole";

            _dops.SetParameter("@RoleName", roleName, "IN");

            ArrayList fields = new ArrayList();
            fields.Add("LoginID");

            ArrayList results = _dops.get_arrayList(fields);

            if (results.Count == 0) //if there were no returned users in the group
                return true;
            else
                return false;
        }

        /// <summary>
        /// Abstraction that allows an arbitrary number of users to get added to an arbitarty number of roles.
        /// On the client side, this is implemened as four functions, including AddUserToRole and AddUserToRoles
        /// </summary>
        /// <param name="usernames">The array of users to add</param>
        /// <param name="roleNames">The array of roles that the user(s) are added to</param>
        /// <remarks>Keeps track of who (using the HTTPContext) added this user to this role</remarks>
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            string CurrentUser = HttpContext.Current.User.Identity.Name; //Current Logged In User

            _dops.ResetDops();
            _dops.Sproc = "usp_getAllRoles";

            ArrayList fields = new ArrayList();
            fields.Add("Role");

            //First pull out all the roles in the application so we can find out if the 
            //role exists without requery'ing the database each time.
            ArrayList allRolesInApp = _dops.get_arrayList(fields);

            foreach (string roleName in roleNames) //For each role they gave us
            {
                //see if the role exists 
                if (allRolesInApp.Contains(roleName))
                {
                    //if it does, for each user assign them to this role
                    foreach (string username in usernames)
                    {
                        //if the user is not already in this role, add them
                        if (IsUserInRole(username, roleName) == false)
                        {
                            //Add 'username' to 'roleName'
                            _dops.ResetDops();
                            _dops.Sproc = "usp_insertUserInRole";

                            _dops.SetParameter("@LoginID", username, "IN");
                            _dops.SetParameter("@RoleName", roleName, "IN");
                            _dops.SetParameter("@AppName", _applicationName, "IN");
                            _dops.SetParameter("@AddedBy", CurrentUser, "IN");

                            _dops.Execute_Sql();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Abstraction that allows an arbitraty number of users to be deleted from an arbitrary number of roles.
        /// On the client side, this is implemened as four functions.
        /// </summary>
        /// <param name="usernames">Array of users to delete</param>
        /// <param name="roleNames">Array of roles</param>
        /// <remarks>Add information on the current user (from HTTPContext) to the tracking tables</remarks>
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            string CurrentUser = HttpContext.Current.User.Identity.Name; //Current User

            _dops.ResetDops();
            _dops.Sproc = "usp_getAllRolesInApp";

            _dops.SetParameter("@AppName", _applicationName, "IN");

            ArrayList fields = new ArrayList();
            fields.Add("Role");

            //First pull out all the roles in the application so we can find out if the 
            //role exists without requery'ing the database each time.
            ArrayList allRolesInApp = _dops.get_arrayList(fields);

            //Go through all of the roles
            foreach (string roleName in roleNames)
            {
                //make sure that role exists
                if (allRolesInApp.Contains(roleName))
                {
                    //for each user
                    foreach (string username in usernames)
                    {
                        //make sure the user is in the role
                        if (IsUserInRole(username, roleName) == true)
                        {
                            //remove 'username' from 'roleName' 
                            _dops.ResetDops();
                            _dops.Sproc = "usp_deleteUserFromGroup";

                            _dops.SetParameter("@LoginID", username, "IN");
                            _dops.SetParameter("@RoleName", roleName, "IN");
                            _dops.SetParameter("@AppName", _applicationName, "IN");
                            _dops.SetParameter("@DeletedBy", CurrentUser, "IN");

                            _dops.Execute_Sql();
                        }
                    }
                }
            }
        }

        #endregion

        #region Base Methods

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
        #endregion

        #region Non-Implemented Methods

        /// <summary>
        /// Searches for all users starting with the string usernameToMatch.  Not implemented.
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="usernameToMatch"></param>
        /// <returns></returns>
        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }

}