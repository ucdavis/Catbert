using System.Collections.Generic;
using System.Linq;
using Catbert4.Core.Domain;
using Catbert4.Services.UserManagement;
using Catbert4.Tests.Core.Helpers;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Core.PersistanceSupport;
using Rhino.Mocks;
using UCDArch.Web.IoC;

namespace Catbert4.Tests.Controllers.UserManagementControllerTests
{
    public partial class UserManagementControllerTests
    {
        #region CanUserManageGivenLogin Tests

        [TestMethod]
        public void TestCanUserManageGivenLogin()
        {
            #region Arrange
            UserService = new UserService(UnitService, UnitAssociationRepository, PermissionRepository);
            ControllerRecordFakes.FakeUnits(6, UnitRepository);
            ControllerRecordFakes.FakeRoles(6, RoleRepository);
            ControllerRecordFakes.FakeApplications(3, ApplicationRepository);
            ControllerRecordFakes.FakeUsers(6, UserRepository);

            //These are the units that the current user has if he has the correct access
            UnitService.Expect(a => a.GetVisibleByUser("Name1", "UserName")).Return(UnitRepository.Queryable.Where(a => a.Id == 2 || a.Id == 4)).Repeat.Any();

            var unitAssociations = new List<UnitAssociation>();
            for (int i = 0; i < 5; i++)
            {
                unitAssociations.Add(CreateValidEntities.UnitAssociation(i+1));
                unitAssociations[i].User = UserRepository.GetNullableById(i+1);
                unitAssociations[i].Application = ApplicationRepository.Queryable.First();
                unitAssociations[i].Unit = UnitRepository.GetNullableById(i + 1);
            }
            ControllerRecordFakes.FakeUnitAssociations(0, UnitAssociationRepository, unitAssociations);

            //QueryExtension.Expect(a => a.ToFuture<Unit>(UnitRepository.Queryable)).Return(UnitRepository.Queryable as IEnumerable<Unit>).Repeat.Any();
           
           

            #endregion Arrange

            #region Act

            //UserService.CanUserManageGivenLogin("Name1", "UserName", "LoginId2");

            var unitsForLoginToManage = from u in UnitAssociationRepository.Queryable
                                        where u.Application.Name == "Name1"
                                              && u.User.LoginId == "LoginId2"
                                        select u.Unit;

            var numIntersectingUnits1 = unitsForLoginToManage.ToFuture();

            //The linq provider can't handle Intersect() so we need to turn them into Enumerable first.
            //Using to future does this and makes the queries more efficient by batching them
            var numIntersectingUnits = unitsForLoginToManage.ToFuture().Intersect(UnitRepository.Queryable.Where(a => a.Id == 2 || a.Id == 4).ToFuture()).Count();
            #endregion Act

            #region Assert

            #endregion Assert		
        }


        #endregion CanUserManageGivenLogin Tests
    }
}
