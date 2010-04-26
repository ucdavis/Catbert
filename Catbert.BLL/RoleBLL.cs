using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAESDO.Catbert.Core.Domain;

namespace CAESDO.Catbert.BLL
{
    public class RoleBLL : GenericBLL<Role, int>
    {
        /// <summary>
        /// Gets all of the roles in this application
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public static List<Role> GetRolesByApplication(string application)
        {
            //First get the application
            Application app = ApplicationBLL.GetByName(application);

            //return the roles
            return app.Roles.ToList();
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
