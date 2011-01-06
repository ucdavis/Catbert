using System;
using System.Collections.Generic;
using System.Linq;
using Catbert4.Core.Domain;
using NHibernate;
using NHibernate.Criterion;
using UCDArch.Data.NHibernate;

namespace Catbert4.Services.UserManagement
{
    public class UserService : IUserService
    {
        public List<User> GetByCriteria(string application, string searchToken, int page, int pageSize, string orderBy, out int totalUsers)
        {
            //Now filter out all of the users in this list by the search criteria 
            ICriteria criteria = NHibernateSessionManager.Instance.GetSession().CreateCriteria(typeof(User))
                .Add(Expression.Eq("Inactive", false))
                .Add(
                    Expression.Disjunction()
                        .Add(Expression.Like("Email", searchToken, MatchMode.Anywhere))
                        .Add(Expression.Like("FirstName", searchToken, MatchMode.Anywhere))
                        .Add(Expression.Like("LastName", searchToken, MatchMode.Anywhere))
                        .Add(Expression.Like("LoginID", searchToken, MatchMode.Anywhere))
                );

            //Only filter on application if one is given
            if (!string.IsNullOrEmpty(application))
            {
                criteria.Add(Subqueries.PropertyIn("id", GetUsersInAnyRoleInApplication(application))); //Include just users within an application
            }

            totalUsers = CriteriaTransformer.TransformToRowCount(criteria).UniqueResult<int>();

            criteria = criteria
                .SetFirstResult((page - 1) * pageSize)
                .SetMaxResults(pageSize)
                .AddOrder(GetOrder(orderBy));

            return criteria.List<User>() as List<User>;
        }

        public List<User> GetByApplication(string application, string currentLogin, string role, string unit, string searchToken, int page, int pageSize, string orderBy, out int totalUsers)
        {
            throw new NotImplementedException();
            /*
            //Now filter out all of the users in this list by the role search criteria 
            //Note: If no role is selected, we still need to filter and make sure the user has ANY role in the application
            ICriteria criteria = NHibernateSessionManager.Instance.GetSession().CreateCriteria(typeof(User))
                .Add(Expression.Eq("Inactive", false))
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

            //Also only allow those users who intersect with the currentLogin's unit list
            DetachedCriteria usersInUnits = new UnitService().GetVisibleByUserCriteria(currentLogin, application).DetachedCriteria;

            criteria = criteria
                .Add(Subqueries.PropertyIn("id", GetUsersInUnits(usersInUnits, application)));

            //Get the total rows returned from this query using a row count transformer
            totalUsers = CriteriaTransformer.TransformToRowCount(criteria).UniqueResult<int>(); // criteria.SetProjection(Projections.RowCount()).UniqueResult<int>();

            criteria = criteria
                .SetFirstResult((page - 1) * pageSize)
                .SetMaxResults(pageSize)
                .AddOrder(GetOrder(orderBy));

            return criteria.List<User>() as List<User>;
             * */
        }

        /// <summary>
        /// Returns true if the given current user has the proper permissions to be managing the loginToManage.
        /// </summary>
        public bool CanUserManageGivenLogin(string application, string currentUserLogin, string loginToManage)
        {
            throw new NotImplementedException();
            /*
            var unitsCurrentUserCanManage =
                new UnitService().GetVisibleByUserCriteria(currentUserLogin, application)
                    .Select(x => x.Id);

            //Now create a query to find the loginToManage's units in this app
            ICriteria unitsForLoginToManageIntersectWithUnitsCurrentUserCanManage = NHibernateSessionManager.Instance.GetSession().CreateCriteria(
                typeof(UnitAssociation))
                .CreateAlias("User", "User")
                .CreateAlias("Unit", "Unit")
                .CreateAlias("Application", "Application")
                .Add(Expression.Eq("Application.Name", application))
                .Add(Expression.Eq("User.LoginID", loginToManage))
                .Add(Expression.Eq("Inactive", false))
                .SetProjection(Projections.Property("Unit.id"))
                .Add(Subqueries.PropertyIn("Unit.id", unitsCurrentUserCanManage.DetachedCriteria));

            var matchedUnits = unitsForLoginToManageIntersectWithUnitsCurrentUserCanManage.List().Count;

            return matchedUnits > 0; //true if there are any matched units
             * */
        }

        private static DetachedCriteria GetUsersInAnyRoleInApplication(string application)
        {
            Permission p = new Permission();

            DetachedCriteria usersInApplication = DetachedCriteria.For(typeof(Permission))
                .Add(Expression.Eq("Inactive", false))
                .CreateAlias("Application", "Application")
                .CreateAlias("User", "User")
                .Add(Expression.Eq("Application.Name", application))
                .SetProjection(Projections.Distinct(Projections.Property("User.id")));

            return usersInApplication;
        }

        /// <summary>
        /// Returns a detached criteria which queries for all of the userIDs that are associated with the units given
        /// </summary>
        private static DetachedCriteria GetUsersInUnits(DetachedCriteria units, string application)
        {
            DetachedCriteria unitAssociations = DetachedCriteria.For(typeof(UnitAssociation))
                .Add(Expression.Eq("Inactive", false))
                .CreateAlias("Application", "Application")
                .CreateAlias("Unit", "Unit")
                .CreateAlias("User", "User")
                .Add(Expression.Eq("Application.Name", application))
                .Add(
                    Subqueries.PropertyIn(
                        "Unit.FISCode",
                        units.SetProjection(Projections.Property("FISCode"))
                        )
                )
                .SetProjection(Projections.Distinct(Projections.Property("User.id")));

            return unitAssociations;
        }

        private static DetachedCriteria GetUsersByApplicationUnit(string application, string unit)
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

        private static DetachedCriteria GetUsersByApplicationRole(string application, string role)
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

        private static Order GetOrder(string orderBy)
        {
            string[] orderTokens = orderBy.Split(' ');
            string orderTerm = orderTokens[0];
            string orderDirection = orderTokens[1];

            if (orderDirection == "ASC")
                return Order.Asc(orderTerm);
            else
                return Order.Desc(orderTerm);
        }

        public List<string> GetManagementRolesForUserInApplication(string application, string login)
        {
            var permRepo = new Repository<Permission>();

            //First we need to find out what kind of user management permissions the given user has in the application
            var permissions = permRepo.Queryable
                .Where(x => x.Application.Name == application)
                .Where(x => x.User.LoginId == login)
                .Select(x=>x.Role.Name);

            /*
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
            */

            return permissions.ToList();
        }
    }
}