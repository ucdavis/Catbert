using CAESDO.Catbert.Core.Domain;
using System.Collections.Generic;

namespace CAESDO.Catbert.Core.DataInterfaces
{
    /// <summary>
    /// Provides an interface for retrieving DAO objects
    /// </summary>
    public interface IDaoFactory 
    {
        IGenericDao<T, IdT> GetGenericDao<T, IdT>();
        IUserDao GetUserDao();
        IUnitDao GetUnitDao();
        IRoleDao GetRoleDao();
    }

    // There's no need to declare each of the DAO interfaces in its own file, so just add them inline here.
    // But you're certainly welcome to put each declaration into its own file.
    #region Inline interface declarations

    public interface IGenericDao<T, IdT> {}

    public interface IUserDao {
        List<User> GetByApplication(string application, string currentLogin, string role, string unit, string searchToken, int page, int pageSize, string orderBy, out int totalUsers);
    }

    public interface IUnitDao {
        List<Unit> GetVisibleByUser(string login, string application);
    }

    public interface IRoleDao
    {
        List<Role> GetVisibleByUser(string application, string login);
    }

    #endregion
}
