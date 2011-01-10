using System.Collections.Generic;
using System.Linq;
using Catbert4.Core.Domain;

namespace Catbert4.Services.UserManagement
{
    public interface IUnitService
    {
        /// <summary>
        /// Get all of the units associated with the given user, depending on role
        /// ManageAll: GetAllUnits
        /// ManageSchool: Get All Units which are associated with the user's schools
        /// ManageUnit: Get Just the units you are associated with
        /// </summary>
        IQueryable<Unit> GetVisibleByUser(string application, string login);
    }
}