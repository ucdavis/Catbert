using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using CAESDO.Catbert.Core.Domain;

namespace CAESDO.Catbert.BLL
{
    public class UnitBLL : GenericBLL<Unit, int>
    {
        private const string STR_Units = "Units";
        
        public static List<Unit> GetAllUnits()
        {
            return GetAllUnits("ShortName", true);
        }

        public static List<Unit> GetAllUnits(string propertyName, bool ascending)
        {
            return GetAll(propertyName, ascending);
        }

        /// <summary>
        /// Get back the ID of the unit given by unitFIS
        /// </summary>
        /// <param name="unitFIS"></param>
        /// <returns></returns>
        public static int GetID(string unitFIS)
        {
            return Queryable.Where(u => u.FISCode == unitFIS).Select(u => u.ID).Single();
        }

        public static Unit GetByFIS(string unitFIS)
        {
            return Queryable.Where(u => u.FISCode == unitFIS).SingleOrDefault();
        }

        /// <summary>
        /// Get all of the units that are visible to the current user in the context of the given application
        /// </summary>
        public static List<Unit> GetVisibleByUser(string application)
        {
            return GetVisibleByUser(HttpContext.Current.User.Identity.Name, application);
        }

        /// <summary>
        /// Get all of the units associated with the given user, depending on role
        /// ManageAll: GetAllUnits
        /// ManageSchool: Get All Units which are associated with the user's schools
        /// ManageUnit: Get Just the units you are associated with
        /// </summary>
        public static List<Unit> GetVisibleByUser(string login, string application)
        {
            return DaoFactory.GetUnitDao().GetVisibleByUser(login, application);
        }

        /// <summary>
        /// Get all of the units associated with the given user
        /// </summary>
        public static List<Unit> GetByUser(string login, string application)
        {
            List<UnitAssociation> unitAssociations =
                UnitAssociationBLL.Queryable.Where(
                    assoc =>
                    assoc.User.LoginID == login && assoc.Application.Name == application && assoc.Inactive == false).
                    ToList();

            return unitAssociations.Select(u => u.Unit).ToList();
        }
    }
}