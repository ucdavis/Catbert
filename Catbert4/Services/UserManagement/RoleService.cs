using System.Collections.Generic;
using System.Linq;
using Catbert4.Core.Domain;
using UCDArch.Core.PersistanceSupport;

namespace Catbert4.Services.UserManagement
{
	public class RoleService : IRoleService
	{
		private readonly IRepository<Permission> _permissionRepository;
		private readonly IRepository<ApplicationRole> _applicationRoleRepository;

		public RoleService(IRepository<Permission> permissionRepository, IRepository<ApplicationRole> applicationRoleRepository)
		{
			_permissionRepository = permissionRepository;
			_applicationRoleRepository = applicationRoleRepository;
		}

		public IQueryable<Role> GetVisibleByUser(string application, string login)
		{
			var userRoles = GetRolesForUser(application, login); //Get all of the user roles

		    var additionalManageableRoles = GetManageableApplicationRoles(application, login);
            
            //visible roles = actual user roles + additional manageable roles
            //Use to future to batch together the queries
            var visibleRoles = userRoles.ToFuture().Union(additionalManageableRoles.ToFuture()).OrderBy(x=>x.Name);
            
		    return visibleRoles.AsQueryable();
		}

        /// <summary>
        /// Return what kind of user management permissions the given user has in the application.
        /// </summary>
        public List<string> GetManagementRolesForUserInApplication(string application, string login)
        {
            var permissions = _permissionRepository.Queryable
                .Where(x => x.Application.Name == application)
                .Where(x => x.User.LoginId == login)
                .Where(x=>x.Role.Name.StartsWith(UserManagementResources.Permission_Manage_Prefix))
                .Select(x => x.Role.Name);

            return permissions.ToList();
        }

        /// <summary>
        /// Get all of the additionally manageable roles that the user has.  This is found by taking the largest
        /// role level the user has and then finding all roles above that level.
        /// </summary>
		private IQueryable<Role> GetManageableApplicationRoles(string application, string login)
		{
            var manageableRoles = from ar in _applicationRoleRepository.Queryable
                                  where ar.Application.Name == application &&
                                        ar.Level > (
                                                       (from p in _permissionRepository.Queryable
                                                        join a in _applicationRoleRepository.Queryable on
                                                            new { Role = p.Role.Id, App = p.Application.Id }
                                                            equals new { Role = a.Role.Id, App = a.Application.Id }
                                                        where p.Application.Name == application &&
                                                              p.User.LoginId == login &&
                                                              a.Level != null
                                                        select a.Level).Max()
                                                   )
                                  select ar.Role;
            
			return manageableRoles;
		}
        
		private IQueryable<Role> GetRolesForUser(string application, string login)
		{
			var allRoles = from p in _permissionRepository.Queryable
						   where p.Application.Name == application
								 && p.User.LoginId == login
						   select p.Role;

			return allRoles;
		}
	}
}