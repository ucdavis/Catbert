using System.Linq;
using System.Security.Authentication;
using Catbert4.Core.Domain;
using UCDArch.Core.PersistanceSupport;

namespace Catbert4.Services.UserManagement
{
    public class UnitService : IUnitService
    {
        private readonly IRoleService _roleService;
        private readonly IRepository<School> _schoolRepository;
        private readonly IRepository<Unit> _unitRepository;
        private readonly IRepository<UnitAssociation> _unitAssociationRespository;
        private readonly IRepository<ApplicationUnit> _applicationUnit;

        public UnitService(IRoleService roleService,
            IRepository<School> schoolRepository, 
            IRepository<Unit> unitRepository, 
            IRepository<UnitAssociation> unitAssociationRespository, 
            IRepository<ApplicationUnit> applicationUnit)
        {
            _roleService = roleService;
            _applicationUnit = applicationUnit;
            _schoolRepository = schoolRepository;
            _unitRepository = unitRepository;
            _unitAssociationRespository = unitAssociationRespository;
            _applicationUnit = applicationUnit;
        }

        /// <summary>
        /// Get all of the units associated with the given user, depending on role
        /// ManageAll: GetAllUnits
        /// ManageSchool: Get All Units which are associated with the user's schools
        /// ManageUnit: Get Just the units you are associated with
        /// </summary>
        public IQueryable<Unit> GetVisibleByUser(string application, string login)
        {
            return GetVisibleByUserCore(application, login).Cache();
        }

        public IQueryable<Unit> GetAssociatedVisibleByApplication(string application, string login)
        {
            var associatedUnits = GetAssociatedUnits(application).Cache().ToList();
            var visibleByUser = GetVisibleByUserCore(application, login).Cache();

            if (associatedUnits.Count() == 0)
            {
                return visibleByUser;
            }
            else
            {
                var visibleUnitIds = visibleByUser.Select(x => x.Id).ToList();
                //if we have associated units, return those that the user has access to)
                return associatedUnits.Where(x => visibleUnitIds.Contains(x.Id)).AsQueryable();
                //return visibleByUser.Intersect(associatedUnits);
            }
        }

        private IQueryable<Unit> GetAssociatedUnits(string application)
        {
            return _applicationUnit.Queryable.Where(x => x.Application.Name == application).Select(x => x.Unit);
        }

        private IQueryable<Unit> GetVisibleByUserCore(string application, string login)
        {
            //First we need to find out what kind of user management permissions the given user has in the application                
            var roles = _roleService.GetManagementRolesForUserInApplication(application, login);

            if (roles.Contains(UserManagementResources.Permission_ManageAll))
            {
                return _unitRepository.Queryable;
            }
            else if (roles.Contains(UserManagementResources.Permission_ManageSchool))
            {
                //Find all schools that the given user has in the application, then choose the units from those schools
                return (from s in _schoolRepository.Queryable
                        join u in _unitRepository.Queryable on s.Id equals u.School.Id
                        where u.UnitAssociations.Any(x => x.Application.Name == application && x.User.LoginId == login)
                        select s).SelectMany(x => x.Units, (x, y) => y).Distinct();
               
            }
            else if (roles.Contains(UserManagementResources.Permission_ManageUnit))
            {
                //Just get all units that the user has in this application

                return from ua in _unitAssociationRespository.Queryable
                       where ua.Application.Name == application && ua.User.LoginId == login
                       select ua.Unit;
            }
            else //no roles
            {
                throw new AuthenticationException(string.Format("User {0} does not have access to this application", login));
            }
        }
    }
}