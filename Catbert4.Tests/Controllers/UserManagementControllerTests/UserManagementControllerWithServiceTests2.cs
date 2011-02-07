﻿using System.Collections.Generic;
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
        public void TestCanUserManageGivenLogin1()
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
            #endregion Arrange

            #region Act
            var result = UserService.CanUserManageGivenLogin("Name1", "UserName", "LoginId2");

            //var unitsForLoginToManage = from u in UnitAssociationRepository.Queryable
            //                            where u.Application.Name == "Name1"
            //                                  && u.User.LoginId == "LoginId2"
            //                            select u.Unit;
            //var numIntersectingUnits1 = unitsForLoginToManage.ToFuture();
            ////The linq provider can't handle Intersect() so we need to turn them into Enumerable first.
            ////Using to future does this and makes the queries more efficient by batching them
            //var numIntersectingUnits = unitsForLoginToManage.ToFuture().Intersect(UnitRepository.Queryable.Where(a => a.Id == 2 || a.Id == 4).ToFuture()).Count();
            #endregion Act

            #region Assert
            Assert.IsTrue(result);
            #endregion Assert		
        }

        [TestMethod]
        public void TestCanUserManageGivenLogin2()
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
                unitAssociations.Add(CreateValidEntities.UnitAssociation(i + 1));
                unitAssociations[i].User = UserRepository.GetNullableById(2);
                unitAssociations[i].Application = ApplicationRepository.Queryable.First();
                unitAssociations[i].Unit = UnitRepository.GetNullableById(i + 1);
            }
            ControllerRecordFakes.FakeUnitAssociations(0, UnitAssociationRepository, unitAssociations);
            #endregion Arrange

            #region Act
            var result = UserService.CanUserManageGivenLogin("Name1", "UserName", "LoginId2");
            #endregion Act

            #region Assert
            Assert.IsTrue(result);
            #endregion Assert
        }

        [TestMethod]
        public void TestCanUserManageGivenLogin3()
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
                unitAssociations.Add(CreateValidEntities.UnitAssociation(i + 1));
                unitAssociations[i].User = UserRepository.GetNullableById(3); //Different LoginId
                unitAssociations[i].Application = ApplicationRepository.Queryable.First();
                unitAssociations[i].Unit = UnitRepository.GetNullableById(i + 1);
            }
            ControllerRecordFakes.FakeUnitAssociations(0, UnitAssociationRepository, unitAssociations);
            #endregion Arrange

            #region Act
            var result = UserService.CanUserManageGivenLogin("Name1", "UserName", "LoginId2");
            #endregion Act

            #region Assert
            Assert.IsFalse(result);
            #endregion Assert
        }

        [TestMethod]
        public void TestCanUserManageGivenLogin4()
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
                unitAssociations.Add(CreateValidEntities.UnitAssociation(i + 1));
                unitAssociations[i].User = UserRepository.GetNullableById(i+1); 
                unitAssociations[i].Application = ApplicationRepository.Queryable.First();
                unitAssociations[i].Unit = UnitRepository.GetNullableById(3); //Different unit (not 2 or 4)
            }
            ControllerRecordFakes.FakeUnitAssociations(0, UnitAssociationRepository, unitAssociations);
            #endregion Arrange

            #region Act
            var result = UserService.CanUserManageGivenLogin("Name1", "UserName", "LoginId2");
            #endregion Act

            #region Assert
            Assert.IsFalse(result);
            #endregion Assert
        }

        [TestMethod]
        public void TestCanUserManageGivenLogin5()
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
                unitAssociations.Add(CreateValidEntities.UnitAssociation(i + 1));
                unitAssociations[i].User = UserRepository.GetNullableById(i + 1);
                unitAssociations[i].Application = ApplicationRepository.GetNullableById(2); //Different app
                unitAssociations[i].Unit = UnitRepository.GetNullableById(2); 
            }
            ControllerRecordFakes.FakeUnitAssociations(0, UnitAssociationRepository, unitAssociations);
            #endregion Arrange

            #region Act
            var result = UserService.CanUserManageGivenLogin("Name1", "UserName", "LoginId2");
            #endregion Act

            #region Assert
            Assert.IsFalse(result);
            #endregion Assert
        }

        #endregion CanUserManageGivenLogin Tests
    }
}
