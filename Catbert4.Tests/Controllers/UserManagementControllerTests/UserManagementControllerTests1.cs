using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Catbert4.Controllers;
using Catbert4.Core.Domain;
using Catbert4.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Catbert4.Tests.Core.Extensions;
using MvcContrib.TestHelper;
using UCDArch.Web.Attributes;
using Rhino;
using Rhino.Mocks;

namespace Catbert4.Tests.Controllers.UserManagementControllerTests
{
    public partial class UserManagementControllerTests
    {
        #region Manage Tests

        [TestMethod]
        public void TestManageReturnsView()
        {
            #region Arrange
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { "" });
            ControllerRecordFakes.FakeUnits(3, UnitRepository);
            UnitService.Expect(a => a.GetVisibleByUser("appName", "UserName")).Return(UnitRepository.GetAll().AsQueryable()).Repeat.Any();
            ControllerRecordFakes.FakeRoles(3, RoleRepository);
            RoleService.Expect(a => a.GetVisibleByUser("appName", "UserName")).Return(RoleRepository.GetAll().AsQueryable()).Repeat.Any();
            ControllerRecordFakes.FakeUsers(3, UserRepository);
            UserService.Expect(a => a.GetByApplication("appName", "UserName")).Return(UserRepository.Queryable).Repeat.Any();
            var application = CreateValidEntities.Application(1);
            application.Name = "appName";
            var permissions = new List<Permission>();
            for (int i = 0; i < 4; i++)
            {
                permissions.Add(CreateValidEntities.Permission(i+1));
                permissions[i].Application = application;
                permissions[i].User = UserRepository.Queryable.First();
                permissions[i].Role = RoleRepository.Queryable.First();
            }
            ControllerRecordFakes.FakePermissions(0, PermissionRepository, permissions);
            var unitAssociations = new List<UnitAssociation>();
            for (int i = 0; i < 4; i++)
            {
                unitAssociations.Add(CreateValidEntities.UnitAssociation(i+1));
                unitAssociations[i].Application = application;
                unitAssociations[i].User = UserRepository.Queryable.First();
                unitAssociations[i].Unit = UnitRepository.Queryable.First();
            }
            ControllerRecordFakes.FakeUnitAssociations(0, UnitAssociationRepository, unitAssociations);
            #endregion Arrange

            #region Act
            var result = Controller.Manage("appName")
                .AssertViewRendered()
                .WithViewData<UserManagementViewModel>();
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("appName", result.Application);
            Assert.AreEqual(3, result.Roles.Count);
            Assert.AreEqual("Name2", result.Roles[1].Value);
            Assert.AreEqual(3, result.Units.Count);
            Assert.AreEqual("ShortName2", result.Units[1].Value);
            Assert.AreEqual(3, result.UserShowModel.Count);
            Assert.AreEqual("FirstName2", result.UserShowModel[1].FirstName);
            Assert.AreEqual("LastName2", result.UserShowModel[1].LastName);
            Assert.AreEqual("test2@testy.com", result.UserShowModel[1].Email);
            Assert.AreEqual("LoginId2", result.UserShowModel[1].Login);
            Assert.AreEqual(null, result.UserShowModel[1].Phone);
            Assert.AreEqual(4, result.UserShowModel[0].Permissions.Count);
            Assert.AreEqual("Name1", result.UserShowModel[0].Permissions[1].RoleName);
            Assert.AreEqual(4, result.UserShowModel[0].UnitAssociations.Count);
            Assert.AreEqual("ShortName1", result.UserShowModel[0].UnitAssociations[1].UnitName);
            Assert.AreEqual(0, result.UserShowModel[1].Permissions.Count);
            Assert.AreEqual(0, result.UserShowModel[1].UnitAssociations.Count);
            Assert.AreEqual(0, result.UserShowModel[2].Permissions.Count);
            Assert.AreEqual(0, result.UserShowModel[2].UnitAssociations.Count);

            #endregion Assert		
        }


        #endregion Manage Tests
    }
}
