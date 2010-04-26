using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAESDO.Catbert.Core.Domain;

namespace CAESDO.Catbert.BLL
{
    public class UnitBLL : GenericBLL<Unit, int>
    {
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
    }
}
