using System.Linq;
using Catbert4.Core.Domain;
using UCDArch.Core.PersistanceSupport;

namespace Catbert4.Services.UserManagement
{
    public class UserService : IUserService
    {
        private readonly IUnitService _unitService;
        private readonly IRepository<UnitAssociation> _unitAssociationRepository;
        private readonly IRepository<Permission> _permissionRepository;

        public UserService(IUnitService unitService, IRepository<UnitAssociation> unitAssociationRepository, IRepository<Permission> permissionRepository)
        {
            _unitService = unitService;
            _unitAssociationRepository = unitAssociationRepository;
            _permissionRepository = permissionRepository;
        }

        /// <summary>
        /// Return all of the users that match the given criteria, and who are visible to the current login
        /// </summary>
        public IQueryable<User> GetByApplication(string application, string currentLogin, string role = null, string unit = null)
        {
            //Only allow users who intersect with the current login's unit list
            //Need to get the unitIds for the linq provider
            var allowedUnitIds = _unitService.GetVisibleByUser(application, currentLogin).ToList().Select(x => x.Id).ToList();
            
            //Get everyone with perms, possibly filtered by role and unit
            var usersWithPermissions = from p in _permissionRepository.Queryable
                                       join u in _unitAssociationRepository.Queryable on
                                           new {User = p.User.Id, App = p.Application.Id}
                                           equals new {User = u.User.Id, App = u.Application.Id}
                                       where p.Application.Name == application
                                       where p.User.UnitAssociations.Any(a => allowedUnitIds.Contains(a.Unit.Id))
                                       select new {Permissions = p, UnitAssociations = u};
           
            if (!string.IsNullOrWhiteSpace(role))
                usersWithPermissions = usersWithPermissions.Where(x => x.Permissions.Role.Name == role);

            if (!string.IsNullOrWhiteSpace(unit))
                usersWithPermissions = usersWithPermissions.Where(x => x.UnitAssociations.Unit.FisCode == unit);

            return usersWithPermissions.Select(x=>x.UnitAssociations.User).Distinct();    
        }

        /// <summary>
        /// Returns true if the given current user has the proper permissions to be managing the loginToManage.
        /// </summary>
        public bool CanUserManageGivenLogin(string application, string currentUserLogin, string loginToManage)
        {
            var unitsCurrentUserCanManage = _unitService.GetVisibleByUser(application, currentUserLogin);

            //Now create a query to find the loginToManage's units in this app
            var unitsForLoginToManage = from u in _unitAssociationRepository.Queryable
                                        where u.Application.Name == application
                                              && u.User.LoginId == loginToManage
                                        select u.Unit;
            
            //The linq provider can't handle Intersect() so we need to turn them into Enumerable first.
            //Using to future does this and makes the queries more efficient by batching them
            var numIntersectingUnits = unitsForLoginToManage.ToFuture().Intersect(unitsCurrentUserCanManage.ToFuture()).Count();

            return numIntersectingUnits > 0;
        }
    }
}