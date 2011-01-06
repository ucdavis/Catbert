using System;
using System.Collections.Generic;
using System.Linq;
using Catbert4.Core.Domain;
using NHibernate.Criterion;
using UCDArch.Data.NHibernate;

namespace Catbert4.Services.UserManagement
{
    public class UnitService : IUnitService
    {
        Application _appAlias = null;
        User _userAlias = null;
        Unit _unitAlias = null;
        School _schoolAlias = null;

        /// <summary>
        /// Get all of the units associated with the given user, depending on role
        /// ManageAll: GetAllUnits
        /// ManageSchool: Get All Units which are associated with the user's schools
        /// ManageUnit: Get Just the units you are associated with
        /// </summary>
        public List<Unit> GetVisibleByUser(string login, string application)
        {
            return GetVisibleByUserCriteria(login, application)
                       .GetExecutableQueryOver(NHibernateSessionManager.Instance.GetSession())
                       .OrderBy(x => x.ShortName).Asc()
                       .List() as List<Unit>;
        }

        /// <summary>
        /// Get all of the units associated with the given user, depending on role
        /// ManageAll: GetAllUnits
        /// ManageSchool: Get All Units which are associated with the user's schools
        /// ManageUnit: Get Just the units you are associated with
        /// </summary>
        internal QueryOver<Unit> GetVisibleByUserCriteria(string login, string application)
        {
            //First we need to find out what kind of user management permissions the given user has in the application                
            var roles = new UserService().GetManagementRolesForUserInApplication(application, login);

            var unitAssociationRepo = new Repository<UnitAssociation>();
            var unitRepo = new Repository<Unit>();
            
            if (roles.Contains("ManageAll"))
            {
                return QueryOver.Of<Unit>();
            }
            else if (roles.Contains("ManageSchool"))
            {
                //Find all schools that the given user has in the application
                /*
                var schools = unitAssociationRepo.Queryable
                    .Where(x => x.Application.Name == application)
                    .Where(x => x.User.LoginId == login)
                    .Select(x => x.Unit.School.Id);
                */
                var schools = QueryOver.Of<UnitAssociation>()
                    .JoinAlias(x => x.Application, () => _appAlias)
                    .JoinAlias(x => x.User, () => _userAlias)
                    .JoinAlias(x => x.Unit, () => _unitAlias)
                    .JoinAlias(x => _unitAlias.School, () => _schoolAlias)
                    .Where(() => _appAlias.Name == application && _userAlias.LoginId == login)
                    .Select(x => _schoolAlias.Id);

                var result = schools.GetExecutableQueryOver(NHibernateSessionManager.Instance.GetSession())
                    .List<string>();
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
                var units = QueryOver.Of<Unit>()
                    .JoinQueryOver(x => x.School)
                    .WithSubquery.WhereProperty(x => x.Id).Eq(schools);
                
                return units;
            }
            else if (roles.Contains("ManageUnit"))
            {
                //Just get all units that the user has in this application
                /*
                var units = unitAssociationRepo.Queryable
                    .Where(x => x.Application.Name == application)
                    .Where(x => x.User.LoginId == login)
                    .Select(x=>x.Unit);
                */

                var units = QueryOver.Of<UnitAssociation>()
                    .JoinAlias(x => x.Application, () => _appAlias)
                    .JoinAlias(x => x.User, () => _userAlias)
                    .Where(() => _appAlias.Name == application && _userAlias.LoginId == login)
                    .Select(x => x.Unit).As<QueryOver<Unit>>();

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