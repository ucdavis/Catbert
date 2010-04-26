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
                //Now filter out all of the users in this list by the search criteria 
                ICriteria criteria = NHibernateSessionManager.Instance.GetSession().CreateCriteria(typeof(User))
                    .Add(
                        Expression.Disjunction()
                            .Add(Expression.Like("Email", searchToken, MatchMode.Anywhere))
                            .Add(Expression.Like("FirstName", searchToken, MatchMode.Anywhere))
                            .Add(Expression.Like("LastName", searchToken, MatchMode.Anywhere))
                            .Add(Expression.Like("LoginID", searchToken, MatchMode.Anywhere))
                        )
                    .Add(Expression.InG<int>("id", GetUsersByApplicationRoleAndUnit(application, role, unit) ));

                //Get the total rows returned from this query using a row count transformer
                totalUsers = CriteriaTransformer.TransformToRowCount(criteria).UniqueResult<int>(); // criteria.SetProjection(Projections.RowCount()).UniqueResult<int>();
             
                criteria = criteria
                            .SetFirstResult((page - 1) * pageSize)
                            .SetMaxResults(pageSize)
                            .AddOrder(GetOrder(orderBy));

                return criteria.List<User>() as List<User>;
            }

            private HashSet<int> GetUsersByApplicationRoleAndUnit(string application, string role, string unit)
            {
                //First get the users by application role
                var userIdsByPermission = GetUsersByApplicationRole(application, role).GetExecutableCriteria(NHibernateSessionManager.Instance.GetSession()).List<int>();

                //Now we have all the userIDs that have permission to this application
                HashSet<int> userIds = new HashSet<int>(userIdsByPermission);

                //If we have a unit, lets get those IDs
                if (!string.IsNullOrEmpty(unit))
                {
                    var userIdsByUnit = GetUsersByApplicationUnit(application, unit).GetExecutableCriteria(NHibernateSessionManager.Instance.GetSession()).List<int>();

                    //Now create a hashed set with only the ids that are common
                    userIds.IntersectWith(userIdsByUnit);
                }

                return userIds;
            }

            private DetachedCriteria GetUsersByApplicationUnit(string application, string unit)
            {
                DetachedCriteria unitAssociations = DetachedCriteria.For(typeof(UnitAssociation))
                    .Add(Expression.Eq("Inactive", false))
                    .CreateAlias("Application", "Application")
                    .CreateAlias("Unit", "Unit")
                    .CreateAlias("User", "User")
                    .Add(Expression.Eq("Application.Name", application));

                if (!string.IsNullOrEmpty(unit))
                {
                    unitAssociations = unitAssociations.Add(Expression.Eq("Unit.FISCode", unit));
                }

                return unitAssociations.SetProjection(Projections.Distinct(Projections.Property("User.id")));
            }

            private DetachedCriteria GetUsersByApplicationRole(string application, string role)
            {
                DetachedCriteria permissions = DetachedCriteria.For(typeof(Permission))
                    .Add(Expression.Eq("Inactive", false))
                    .CreateAlias("Application", "Application")
                    .CreateAlias("Role", "Role")
                    .CreateAlias("User", "User")
                    .Add(Expression.Eq("Application.Name", application));

                if (!string.IsNullOrEmpty(role))
                {
                    permissions = permissions.Add(Expression.Eq("Role.Name", role));
                }

                return permissions.SetProjection(Projections.Distinct(Projections.Property("User.id")));
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
