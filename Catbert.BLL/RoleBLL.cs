using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAESDO.Catbert.Core.Domain;
using CAESDO.Catbert.Data;
using System.Web;

namespace CAESDO.Catbert.BLL
{
    public class RoleBLL : GenericBLL<Role, int>
    {
        public static void CreateRole(string roleName, string trackingUserName)
        {
            if (RoleBLL.GetByName(roleName) != null) //If there is already a role by that name, throw an error
            {
                throw new ArgumentException(string.Format("The role {0} already exists in the system", roleName));
            }
            else
            {
                //Create a new role
                Role role = new Role() { Inactive = false, Name = roleName };

                var tracking = TrackingBLL.GetTrackingInstance(trackingUserName, TrackingTypes.Role, TrackingActions.Add);
                tracking.Comments = string.Format("Role {0} added", roleName);

                using (var ts = new TransactionScope())
                {
                    RoleBLL.MakePersistent(ref role);
                    TrackingBLL.MakePersistent(ref tracking);

                    ts.CommittTransaction();
                }
            }
        }

        /// <summary>
        /// Gets all of the roles in this application
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public static List<ApplicationRole> GetRolesByApplication(string application)
        {
            var query = from role in ApplicationRoleBLL.Queryable
                        where role.Application.Name == application
                                && role.Role.Inactive == false
                        select role;

            return query.ToList();
        }

        public static List<Role> GetVisibleByUser(string application)
        {
            return GetVisibleByUser(application, HttpContext.Current.User.Identity.Name);
        }

        public static List<Role> GetVisibleByUser(string application, string login)
        {
            return daoFactory.GetRoleDao().GetVisibleByUser(application, login);
        }

        /// <summary>
        /// Returns a role's ID given its name
        /// </summary>
        public static int GetID(string role)
        {
            return Queryable.Where(r => r.Name == role).Select(r => r.ID).Single();
        }
    }
}
