using CAESDO.Catbert.Core.DataInterfaces;
using CAESDO.Catbert.Core.Domain;
using System.Collections.Generic;
using NHibernate;
using System.ComponentModel;
using System.Web;
using NHibernate.Criterion;

namespace CAESDO.Catbert.Data
{
    /// <summary>
    /// Exposes access to NHibernate DAO classes.  Motivation for this DAO
    /// framework can be found at http://www.hibernate.org/328.html.
    /// </summary>
    public class NHibernateDaoFactory : IDaoFactory
    {
        #region Dao Retrieval Operations

        public IGenericDao<T, IdT> GetGenericDao<T, IdT>()
        {
            return new GenericDao<T, IdT>();
        }

        public IUserDao GetUserDao()
        {
            return new UserDao();
        }

        #endregion

        #region Inline DAO implementations

        public class GenericDao<T, IdT> : AbstractNHibernateDao<T, IdT>, IGenericDao<T, IdT> { }

        public class UserDao : AbstractNHibernateDao<User, int>, IUserDao
        {
            public List<User> GetByApplication(string application, string role, string unit, string searchToken, int page, int pageSize, string orderBy, out int totalUsers)
            {
                User u = new User();
                //u.Permissions[0].Role.Name
                ICriteria criteria = NHibernateSessionManager.Instance.GetSession().CreateCriteria(typeof(User))
                    .Add(
                        Expression.Disjunction()
                            .Add(Expression.Like("Email", searchToken, MatchMode.Anywhere))
                            .Add(Expression.Like("FirstName", searchToken, MatchMode.Anywhere))
                            .Add(Expression.Like("LastName", searchToken, MatchMode.Anywhere))
                            .Add(Expression.Like("LoginID", searchToken, MatchMode.Anywhere))
                        )
                    .CreateAlias("Permissions", "Perms")
                    .CreateAlias("Perms.Application", "Application")
                    .CreateAlias("Perms.Role", "Role")
                    .Add(
                        Expression.Conjunction()
                            .Add(Expression.Eq("Perms.Inactive", false))
                            .Add(Expression.Eq("Application.Name", application))
                        );

                if (!string.IsNullOrEmpty(role))
                    criteria = criteria.Add(Expression.Eq("Role.Name", role));

                //TODO: Have to figure out how to find the total number of users
                totalUsers = 7; // criteria.SetProjection(Projections.RowCount()).UniqueResult<int>();
                
                /*
                criteria = criteria
                            .SetFirstResult(page * pageSize)
                            .SetMaxResults(pageSize);
                */

                criteria = criteria
                            //.SetProjection(Projections.Distinct(Projections.Id()))
                            .AddOrder(GetOrder(orderBy));

                return criteria.List<User>() as List<User>;
            }

            private Order GetOrder(string orderBy)
            {
                string[] orderTokens = orderBy.Split(' ');
                string orderTerm = orderTokens[0];
                string orderDirection = orderTokens[1];

                if (orderDirection == "ASC")
                    return Order.Asc(orderTerm);
                else
                    return Order.Desc(orderTerm);
            }
        }

        #endregion
          
    }
}
