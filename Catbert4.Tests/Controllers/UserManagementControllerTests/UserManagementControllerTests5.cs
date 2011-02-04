using System;
using System.Collections.Generic;
using System.Linq;
using Catbert4.Core.Domain;
using Catbert4.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace Catbert4.Tests.Controllers.UserManagementControllerTests
{
    public partial class UserManagementControllerTests
    {
        #region AddPermission Tests

        [TestMethod]
        public void TestAddPermission()
        {
            #region Arrange
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { "" });
            ControllerRecordFakes.FakeRoles(5, RoleRepository);
            ControllerRecordFakes.FakeApplications(3, ApplicationRepository);
            ControllerRecordFakes.FakeUsers(3, UserRepository);
            ControllerRecordFakes.FakePermissions(1, PermissionRepository);

            UserService.Expect(a => a.CanUserManageGivenLogin("Name3", "UserName", "LoginId3")).Return(true).Repeat.Any();
            RoleService.Expect(a => a.GetVisibleByUser("Name3", "UserName")).Return(RoleRepository.Queryable).Repeat.Any();
            PermissionRepository.Expect(a => a.EnsurePersistent(Arg<Permission>.Is.Anything)).Repeat.Any();
            #endregion Arrange
           
            #region Act
            Controller.AddPermission("Name3", "LoginId3", 2);
            #endregion Act

            #region Assert
            UserService.AssertWasCalled(a => a.CanUserManageGivenLogin("Name3", "UserName", "LoginId3"));
            RoleService.AssertWasCalled(a => a.GetVisibleByUser("Name3", "UserName"));
            PermissionRepository.AssertWasCalled(a => a.EnsurePersistent(Arg<Permission>.Is.Anything));
            var args = (Permission) PermissionRepository.GetArgumentsForCallsMadeOn(a => a.EnsurePersistent(Arg<Permission>.Is.Anything))[0][0]; 
            Assert.IsNotNull(args);
            Assert.AreEqual("Name2", args.Role.Name);
            Assert.AreEqual("LoginId3", args.User.LoginId);
            Assert.AreEqual("Name3", args.Application.ToString());
            Assert.IsFalse(args.Inactive);
            #endregion Assert		
        }

        [TestMethod]
        public void TestAddPermissionWhenPermissionAlreadyExists()
        {
            #region Arrange
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { "" });
            ControllerRecordFakes.FakeRoles(5, RoleRepository);
            ControllerRecordFakes.FakeApplications(3, ApplicationRepository);
            ControllerRecordFakes.FakeUsers(3, UserRepository);
            var permissions = new List<Permission> {CreateValidEntities.Permission(1)};
            permissions[0].Application = ApplicationRepository.GetNullableById(3);
            permissions[0].User = UserRepository.GetNullableById(3);
            permissions[0].Role = RoleRepository.GetNullableById(2);

            ControllerRecordFakes.FakePermissions(1, PermissionRepository, permissions);

            UserService.Expect(a => a.CanUserManageGivenLogin("Name3", "UserName", "LoginId3")).Return(true).Repeat.Any();
            RoleService.Expect(a => a.GetVisibleByUser("Name3", "UserName")).Return(RoleRepository.Queryable).Repeat.Any();
            PermissionRepository.Expect(a => a.EnsurePersistent(Arg<Permission>.Is.Anything)).Repeat.Any();
            #endregion Arrange

            #region Act
            Controller.AddPermission("Name3", "LoginId3", 2);
            #endregion Act

            #region Assert
            UserService.AssertWasCalled(a => a.CanUserManageGivenLogin("Name3", "UserName", "LoginId3"));
            RoleService.AssertWasCalled(a => a.GetVisibleByUser("Name3", "UserName"));
            PermissionRepository.AssertWasNotCalled(a => a.EnsurePersistent(Arg<Permission>.Is.Anything));

            #endregion Assert
        }

        [TestMethod]
        [ExpectedException(typeof(UCDArch.Core.Utils.PreconditionException))]
        public void TestAddPermissionThrowsExceptionIfCurrentUserDoesNotHaveRights1()
        {
            try
            {
                #region Arrange
                Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { "" });
                ControllerRecordFakes.FakeApplications(3, ApplicationRepository);
                ControllerRecordFakes.FakeRoles(5, RoleRepository);
                ControllerRecordFakes.FakeUsers(3, UserRepository);
                ControllerRecordFakes.FakePermissions(1, PermissionRepository);

                UserService.Expect(a => a.CanUserManageGivenLogin("Name3", "UserName", "LoginId3")).Return(false).Repeat.Any();
                PermissionRepository.Expect(a => a.EnsurePersistent(Arg<Permission>.Is.Anything)).Repeat.Any();
                #endregion Arrange

                #region Act
                Controller.AddPermission("Name3", "LoginId3", 3);
                #endregion Act
            }
            catch (Exception ex)
            {
                #region Assert
                Assert.IsNotNull(ex);
                Assert.AreEqual("UserName does not have access to manage LoginId3 within the Name3 application", ex.Message);
                UserService.AssertWasCalled(a => a.CanUserManageGivenLogin("Name3", "UserName", "LoginId3"));
                PermissionRepository.AssertWasNotCalled(a => a.EnsurePersistent(Arg<Permission>.Is.Anything));
                #endregion Assert
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(UCDArch.Core.Utils.PreconditionException))]
        public void TestAddPermissionThrowsExceptionIfCurrentUserDoesNotHaveRights2()
        {
            try
            {
                #region Arrange
                Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { "" });
                ControllerRecordFakes.FakeApplications(3, ApplicationRepository);
                ControllerRecordFakes.FakeRoles(5, RoleRepository);
                ControllerRecordFakes.FakeUsers(3, UserRepository);
                ControllerRecordFakes.FakePermissions(1, PermissionRepository);

                RoleService.Expect(a => a.GetVisibleByUser("Name3", "UserName")).Return(RoleRepository.Queryable.Where(a => a.Id >= 4)).Repeat.Any();

                UserService.Expect(a => a.CanUserManageGivenLogin("Name3", "UserName", "LoginId3")).Return(true).Repeat.Any();
                PermissionRepository.Expect(a => a.EnsurePersistent(Arg<Permission>.Is.Anything)).Repeat.Any();
                #endregion Arrange

                #region Act
                Controller.AddPermission("Name3", "LoginId3", 3);
                #endregion Act
            }
            catch (Exception ex)
            {
                #region Assert
                Assert.IsNotNull(ex);
                Assert.AreEqual("UserName does not have access to manage the given role", ex.Message);
                UserService.AssertWasCalled(a => a.CanUserManageGivenLogin("Name3", "UserName", "LoginId3"));
                PermissionRepository.AssertWasNotCalled(a => a.EnsurePersistent(Arg<Permission>.Is.Anything));
                #endregion Assert
                throw;
            }
        }

        /// <summary>
        /// This would probably be caught by the UserService before the "Single()", but maybe not if there was a duplicate
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestAddPermissionThrowsExceptionIfApplicationNotFound()
        {
            try
            {
                #region Arrange
                Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { "" });
                ControllerRecordFakes.FakeApplications(3, ApplicationRepository);
                ControllerRecordFakes.FakeRoles(5, RoleRepository);
                ControllerRecordFakes.FakeUsers(3, UserRepository);
                ControllerRecordFakes.FakePermissions(1, PermissionRepository);

                RoleService.Expect(a => a.GetVisibleByUser("Name4", "UserName")).Return(RoleRepository.Queryable).Repeat.Any();
                UserService.Expect(a => a.CanUserManageGivenLogin("Name4", "UserName", "LoginId3")).Return(true).Repeat.Any();
                PermissionRepository.Expect(a => a.EnsurePersistent(Arg<Permission>.Is.Anything)).Repeat.Any();
                #endregion Arrange

                #region Act
                Controller.AddPermission("Name4", "LoginId3", 3);
                #endregion Act
            }
            catch (Exception ex)
            {
                #region Assert
                Assert.IsNotNull(ex);
                Assert.AreEqual("Sequence contains no elements", ex.Message);
                UserService.AssertWasCalled(a => a.CanUserManageGivenLogin("Name4", "UserName", "LoginId3"));
                RoleService.AssertWasCalled(a => a.GetVisibleByUser("Name4", "UserName"));
                PermissionRepository.AssertWasNotCalled(a => a.EnsurePersistent(Arg<Permission>.Is.Anything));
                #endregion Assert
                throw;
            }
        }

        /// <summary>
        /// This would probably be caught by the UserService before the "Single()", but maybe not if there was a duplicate
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestAddPermissionThrowsExceptionIfloginIdDuplicate()
        {
            try
            {
                #region Arrange
                Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { "" });
                ControllerRecordFakes.FakeApplications(3, ApplicationRepository);
                ControllerRecordFakes.FakeRoles(5, RoleRepository);
                var users = new List<User> {CreateValidEntities.User(1), CreateValidEntities.User(1)};
                ControllerRecordFakes.FakeUsers(3, UserRepository, users);
                ControllerRecordFakes.FakePermissions(1, PermissionRepository);

                RoleService.Expect(a => a.GetVisibleByUser("Name2", "UserName")).Return(RoleRepository.Queryable).Repeat.Any();
                UserService.Expect(a => a.CanUserManageGivenLogin("Name2", "UserName", "LoginId1")).Return(true).Repeat.Any();
                PermissionRepository.Expect(a => a.EnsurePersistent(Arg<Permission>.Is.Anything)).Repeat.Any();
                #endregion Arrange

                #region Act
                Controller.AddPermission("Name2", "LoginId1", 3);
                #endregion Act
            }
            catch (Exception ex)
            {
                #region Assert
                Assert.IsNotNull(ex);
                Assert.AreEqual("Sequence contains more than one element", ex.Message);
                UserService.AssertWasCalled(a => a.CanUserManageGivenLogin("Name2", "UserName", "LoginId1"));
                RoleService.AssertWasCalled(a => a.GetVisibleByUser("Name2", "UserName"));
                PermissionRepository.AssertWasNotCalled(a => a.EnsurePersistent(Arg<Permission>.Is.Anything));
                #endregion Assert
                throw;
            }
        }

        #endregion AddPermission Tests
    }
}
