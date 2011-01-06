using System.Collections.Generic;
using Catbert4.Core.Domain;
using NHibernate;
using NHibernate.Criterion;
using UCDArch.Data.NHibernate;

namespace Catbert4.Services.UserManagement
{
    public class RoleService : IRoleService
    {
        public List<Role> GetVisibleByUser(string application, string login)
        {
            var userRoles = GetRolesForUser(application, login); //Get all of the user roles

            //Take the min role level for this application and then get all application roles with a higher level than this min
            var minLevel = GetMinApplicationRole(userRoles, application);
            var lowerApplicationRoles = GetApplicationRolesUnderLevel(minLevel, application);

            //Now get all roles that either the user has or are in the lowerApplicationRoles
            ICriteria roles = NHibernateSessionManager.Instance.GetSession().CreateCriteria(typeof(Role))
                .Add(
                    Expression.Or(
                        Subqueries.PropertyIn("id", userRoles),
                        Subqueries.PropertyIn("id", lowerApplicationRoles)
                        )
                )
                .AddOrder(Order.Asc("Name"));

            return roles.List<Role>() as List<Role>;
        }

        private static DetachedCriteria GetApplicationRolesUnderLevel(DetachedCriteria minLevel, string application)
        {
            DetachedCriteria criteria = DetachedCriteria.For<ApplicationRole>()
                .Add(Expression.IsNotNull("Level"))
                .CreateAlias("Application", "Application")
                .CreateAlias("Role", "Role")
                .Add(Expression.Eq("Application.Name", application))
                .Add(Subqueries.PropertyGt("Level", minLevel))
                .SetProjection(Projections.Property("Role.id"));

            return criteria;
        }

        private static DetachedCriteria GetMinApplicationRole(DetachedCriteria roles, string application)
        {
            DetachedCriteria criteria = DetachedCriteria.For<ApplicationRole>()
                .Add(Expression.IsNotNull("Level"))
                .CreateAlias("Application", "Application")
                .CreateAlias("Role", "Role")
                .Add(Expression.Eq("Application.Name", application))
                .Add(Subqueries.PropertyIn("Role.id", roles)) //this role must be in the given roles list
                .SetProjection(Projections.Min("Level"));

            return criteria; //Returns the minimum level of these application roles
        }

        private static DetachedCriteria GetRolesForUser(string application, string login)
        {
            DetachedCriteria criteria = DetachedCriteria.For<Permission>()
                .Add(Expression.Eq("Inactive", false))
                .CreateAlias("Application", "Application")
                .CreateAlias("Role", "Role")
                .CreateAlias("User", "User")
                .Add(Expression.Eq("Application.Name", application))
                .Add(Expression.Eq("User.LoginID", login))
                .SetProjection(Projections.Property("Role.id"));

            return criteria;
        }
    }
}