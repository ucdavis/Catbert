using System;
using System.Linq;
using Catbert4.Core.Domain;
using Microsoft.Practices.ServiceLocation;
using UCDArch.Core.PersistanceSupport;

namespace Catbert4.Services.Wcf
{
    public class RoleService : IRoleService
    {
        public bool IsUserInRole(string application, string user, string role)
        {
            var permissions = from p in RepositoryFactory.PermissionRepository.Queryable
                    where p.Application.Name == application
                          && p.User.LoginId == user
                          && p.Role.Name == role
                    select p;

            return permissions.Any(); //Return if any permissions match the criteria
        }

        public string[] GetAllRoles(string application)
        {
            var roles = from applicationRole in RepositoryFactory.ApplicationRoleRepository.Queryable
                    where applicationRole.Application.Name == application
                    select applicationRole.Role.Name;

            return roles.ToArray();
        }

        public string[] GetRolesForUser(string application, string user)
        {
            var roles = from p in RepositoryFactory.PermissionRepository.Queryable
                              where p.Application.Name == application
                                    && p.User.LoginId == user
                              select p.Role.Name;

            return roles.ToArray();
        }

        public string[] GetUsersInRole(string application, string roleName)
        {
            var users = from p in RepositoryFactory.PermissionRepository.Queryable
                        where p.Application.Name == application
                              && p.Role.Name == roleName
                        select p.User.LoginId;

            return users.ToArray();
        }

        public bool RoleExists(string application, string role)
        {
            var roles = from applicationRole in RepositoryFactory.ApplicationRoleRepository.Queryable
                        where applicationRole.Application.Name == application
                                && applicationRole.Role.Name == role
                        select applicationRole;

            return roles.Any();
        }
    }

    public class RepositoryFactory
    {
        // Private constructor prevents instantiation from other classes
        private RepositoryFactory() { }

        private static class PermissionSingletonHolder
        {
            public static readonly IRepository<Permission> Instance = ServiceLocator.Current.GetInstance<IRepository<Permission>>();
        }

        private static class ApplicationRoleSingletonHolder
        {
            public static readonly IRepository<ApplicationRole> Instance = ServiceLocator.Current.GetInstance<IRepository<ApplicationRole>>();
        }
        
        public static IRepository<Permission> PermissionRepository
        {
            get
            {
                return PermissionSingletonHolder.Instance;
            }
        }

        public static IRepository<ApplicationRole> ApplicationRoleRepository
        {
            get
            {
                return ApplicationRoleSingletonHolder.Instance;
            }
        }

    }
}
