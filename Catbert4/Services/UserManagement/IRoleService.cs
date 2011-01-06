using System.Collections.Generic;
using Catbert4.Core.Domain;

namespace Catbert4.Services.UserManagement
{
    public interface IRoleService
    {
        List<Role> GetVisibleByUser(string application, string login);
    }
}