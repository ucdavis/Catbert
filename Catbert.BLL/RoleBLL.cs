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
        /// Returns a role's ID given its name
        /// </summary>
        public static int GetID(string role)
        {
            return Queryable.Where(r => r.Name == role).Select(r => r.ID).Single();
        }
    }
}
