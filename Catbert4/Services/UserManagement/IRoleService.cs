using System.Collections.Generic;
using System.Linq;
using Catbert4.Core.Domain;

namespace Catbert4.Services.UserManagement
{
    public interface IRoleService
    {
        IQueryable<Role> GetVisibleByUser(string application, string login);
    }
}