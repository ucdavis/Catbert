using System.Linq;
using NHibernate.Linq;
using UCDArch.Data.NHibernate;

namespace Catbert4.Services
{
    public interface IQueryService
    {
        IQueryable<T> GetQueryable<T>();
    }

    public class QueryService : IQueryService
    {
        public IQueryable<T> GetQueryable<T>()
        {
            return NHibernateSessionManager.Instance.GetSession().Query<T>();
        }
    }
}