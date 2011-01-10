using System;
using System.Linq;
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

        public UnitService(IRoleService roleService,
            IRepository<School> schoolRepository, 
            IRepository<Unit> unitRepository, 
            IRepository<UnitAssociation> unitAssociationRespository)
        {
            _roleService = roleService;
            _schoolRepository = schoolRepository;
            _unitRepository = unitRepository;
            _unitAssociationRespository = unitAssociationRespository;
        }

        /// <summary>
        /// Get all of the units associated with the given user, depending on role
        /// ManageAll: GetAllUnits
        /// ManageSchool: Get All Units which are associated with the user's schools
        /// ManageUnit: Get Just the units you are associated with
        /// </summary>
        public IQueryable<Unit> GetVisibleByUser(string login, string application)
        {
            //First we need to find out what kind of user management permissions the given user has in the application                
            var roles = _roleService.GetManagementRolesForUserInApplication(application, login);

            if (roles.Contains("ManageAll"))
            {
                return _unitRepository.Queryable;
            }
            else if (roles.Contains("ManageSchool"))
            {
                //Find all schools that the given user has in the application
                var units = (from s in _schoolRepository.Queryable
                         join u in _unitRepository.Queryable on s.Id equals u.School.Id
                         where u.UnitAssociations.Any(x => x.Application.Name == application && x.User.LoginId == login)
                         select s).SelectMany(x => x.Units, (x, y) => y);
                /*
                var schools = QueryOver.Of<UnitAssociation>()
                    .JoinAlias(x => x.Application, () => _appAlias)
                    .JoinAlias(x => x.User, () => _userAlias)
                    .JoinAlias(x => x.Unit, () => _unitAlias)
                    .JoinAlias(x => _unitAlias.School, () => _schoolAlias)
                    .Where(() => _appAlias.Name == application && _userAlias.LoginId == login)
                    .Select(x => _schoolAlias.Id);
                */
                /*
                DetachedCriteria schools = DetachedCriteria.For<UnitAssociation>()
                    .CreateAlias("Application", "Application")
                    .CreateAlias("User", "User")
                    .CreateAlias("Unit", "Unit")
                    .CreateAlias("Unit.School", "School")
                    .Add(Restrictions.Eq("Application.Name", application))
                    .Add(Restrictions.Eq("User.LoginID", login))
                    .Add(Restrictions.Eq("Inactive", false))
                    .SetProjection(Projections.Distinct(Projections.Property("School.id")));
                */
                //Now get all units that are associated with these schools
                //var units = unitRepo.Queryable
                //    .Where(u => unitAssociationRepo.Queryable
                //                    .Where(x => x.Application.Name == application)
                //                    .Where(x => x.User.LoginId == login)
                //                    .Select(x => x.Unit.School.Id)
                //                    .Contains(u.School.Id));

                /*
                DetachedCriteria units = DetachedCriteria.For<Unit>()
                    .CreateAlias("School", "School")
                    .Add(Subqueries.PropertyIn("School.id", schools));
                */
                
                return units;
            }
            else if (roles.Contains("ManageUnit"))
            {
                //Just get all units that the user has in this application

                var units = from ua in _unitAssociationRespository.Queryable
                            where ua.Application.Name == "HelpRequest" && ua.User.LoginId == "postit"
                            select ua.Unit;
                
                /*
                var units = QueryOver.Of<UnitAssociation>()
                    .JoinAlias(x => x.Application, () => _appAlias)
                    .JoinAlias(x => x.User, () => _userAlias)
                    .Where(() => _appAlias.Name == application && _userAlias.LoginId == login)
                    .Select(x => x.Unit).As<QueryOver<Unit>>();
                */
                /*
                DetachedCriteria associatedUnitIds = DetachedCriteria.For<UnitAssociation>()
                    .CreateAlias("Application", "Application")
                    .CreateAlias("User", "User")
                    .CreateAlias("Unit", "Unit")
                    .Add(Expression.Eq("Application.Name", application))
                    .Add(Expression.Eq("User.LoginID", login))
                    .Add(Expression.Eq("Inactive", false))
                    .SetProjection(Projections.Property("Unit.id"));
                */
                
                /*
                DetachedCriteria units = DetachedCriteria.For<Unit>()
                    .Add(Subqueries.PropertyIn("id", associatedUnitIds));
                */

                return units;
            }
            else //no roles
            {
                throw new ArgumentException(string.Format("User {0} does not have access to this application", login));
            }
        }
    }
}