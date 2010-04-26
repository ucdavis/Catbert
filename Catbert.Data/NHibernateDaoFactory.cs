using CAESDO.Catbert.Core.DataInterfaces;
using CAESDO.Catbert.Core.Domain;
using System.Collections.Generic;
using NHibernate;
using System.ComponentModel;
using System.Web;
using NHibernate.Criterion;
using System;

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

        public IUnitDao GetUnitDao()
        {
            return new UnitDao();
        }

        #endregion

        #region Inline DAO implementations

        public class GenericDao<T, IdT> : AbstractNHibernateDao<T, IdT>, IGenericDao<T, IdT> { }

        public class UserDao : AbstractNHibernateDao<User, int>, IUserDao
        {
            public List<User> GetByApplication(string application, string role, string unit, string searchToken, int page, int pageSize, string orderBy, out int totalUsers)
            {                
                //Now filter out all of the users in this list by the role search criteria 
                //Note: If no role is selected, we still need to filter and make sure the user has ANY role in the application
                ICriteria criteria = NHibernateSessionManager.Instance.GetSession().CreateCriteria(typeof(User))
                    .Add(
                        Expression.Disjunction()
                            .Add(Expression.Like("Email", searchToken, MatchMode.Anywhere))
                            .Add(Expression.Like("FirstName", searchToken, MatchMode.Anywhere))
                            .Add(Expression.Like("LastName", searchToken, MatchMode.Anywhere))
                            .Add(Expression.Like("LoginID", searchToken, MatchMode.Anywhere))
                        )
                    .Add(Subqueries.PropertyIn("id", GetUsersByApplicationRole(application, role)));

                //If the unit is specified, also filter by unit
                if (!string.IsNullOrEmpty(unit))
                    criteria = criteria.Add(Subqueries.PropertyIn("id", GetUsersByApplicationUnit(application, unit)));
                
                //Get the total rows returned from this query using a row count transformer
                totalUsers = CriteriaTransformer.TransformToRowCount(criteria).UniqueResult<int>(); // criteria.SetProjection(Projections.RowCount()).UniqueResult<int>();
             
                criteria = criteria
                            .SetFirstResult((page - 1) * pageSize)
                            .SetMaxResults(pageSize)
                            .AddOrder(GetOrder(orderBy));

                return criteria.List<User>() as List<User>;
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

            public List<string> GetRolesForUserInApplication(string login, string application)
            {
                //First we need to find out what kind of user management permissions the given user has in the application
                ICriteria permissionsCriteria = NHibernateSessionManager.Instance.GetSession().CreateCriteria(typeof(Permission))
                    .CreateAlias("Application", "Application")
                    .CreateAlias("User", "User")
                    .CreateAlias("Role", "Role")
                    .Add(Expression.Eq("Application.Name", application))
                    .Add(Expression.Eq("User.LoginID", login))
                    .Add(Expression.Eq("Inactive", false))
                    .Add(Expression.Like("Role.Name", "Manage", MatchMode.Start))
                    .SetProjection(Projections.Distinct(Projections.Property("Role.Name")))
                    .SetMaxResults(3); //ManageAll, ManageSchool, ManageUnit

                return permissionsCriteria.List<string>() as List<string>;
            }
        }

        public class UnitDao : AbstractNHibernateDao<Unit, int>, IUnitDao
        {
            /// <summary>
            /// Get all of the units associated with the given user, depending on role
            /// ManageAll: GetAllUnits
            /// ManageSchool: Get All Units which are associated with the user's schools
            /// ManageUnit: Get Just the units you are associated with
            /// </summary>
            public List<Unit> GetVisibleByUser(string login, string application)
            {
                //First we need to find out what kind of user management permissions the given user has in the application                
                var roles = new UserDao().GetRolesForUserInApplication(login, application);

                if (roles.Contains("ManageAll"))
                {
                    return GetAll("ShortName", true);
                }
                else if (roles.Contains("ManageSchool"))
                {
                    //Find all schools that the given user has in the application
                    DetachedCriteria schools = DetachedCriteria.For<UnitAssociation>()
                        .CreateAlias("Application", "Application")
                        .CreateAlias("User", "User")
                        .CreateAlias("Unit", "Unit")
                        .CreateAlias("Unit.School", "School")
                        .Add(Expression.Eq("Application.Name", application))
                        .Add(Expression.Eq("User.LoginID", login))
                        .Add(Expression.Eq("Inactive", false))
                        .SetProjection(Projections.Distinct(Projections.Property("School.id")));

                    //Now get all units that are associated with these schools
                    ICriteria units = NHibernateSessionManager.Instance.GetSession().CreateCriteria(typeof(Unit))
                        .CreateAlias("School", "School")
                        .Add(Subqueries.PropertyIn("School.id", schools));

                    return units.List<Unit>() as List<Unit>;
                }
                else if (roles.Contains("ManageUnit"))
                {
                    //Just get all units that the user has in this application
                    UnitAssociation ua = new UnitAssociation();

                    DetachedCriteria associatedUnitIds = DetachedCriteria.For<UnitAssociation>()
                        .CreateAlias("Application", "Application")
                        .CreateAlias("User", "User")
                        .CreateAlias("Unit", "Unit")
                        .Add(Expression.Eq("Application.Name", application))
                        .Add(Expression.Eq("User.LoginID", login))
                        .Add(Expression.Eq("Inactive", false))
                        .SetProjection(Projections.Property("Unit.id"));

                    ICriteria units = NHibernateSessionManager.Instance.GetSession().CreateCriteria(typeof(Unit))
                        .Add(Subqueries.PropertyIn("id", associatedUnitIds));
                    
                    return units.List<Unit>() as List<Unit>;                    
                }
                else //no roles
                {
                    throw new ArgumentException(string.Format("User {0} does not have access to this application", login));
                }
            }
        }
        #endregion          
    }
}