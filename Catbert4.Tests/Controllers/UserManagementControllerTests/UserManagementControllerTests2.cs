using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Catbert4.Core.Domain;
using Catbert4.Helpers;
using Catbert4.Models;
using Catbert4.Services;
using Catbert4.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using Rhino.Mocks;

namespace Catbert4.Tests.Controllers.UserManagementControllerTests
{
    public partial class UserManagementControllerTests
    {
        #region FindUser Tests

        [TestMethod]
        public void TestFindUser1()
        {
            #region Arrange
            const string searchTerm = "TestValue";
            DirectorySearchService.Expect(a => a.FindUser(searchTerm)).Return(null).Repeat.Any();
            #endregion Arrange

            #region Act
            var result = Controller.FindUser(searchTerm)
                .AssertResultIs<JsonResult>();
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.Data);
            Assert.AreEqual(JsonRequestBehavior.AllowGet, result.JsonRequestBehavior);
            DirectorySearchService.AssertWasCalled(a => a.FindUser(searchTerm));
            #endregion Assert		
        }

        [TestMethod]
        public void TestFindUser2()
        {
            #region Arrange
            const string searchTerm = "TestValue";
            AutomapperConfig.Configure();
            var directoryUser = new DirectoryUser();
            directoryUser.EmailAddress = "bobby@bill.com";
            directoryUser.EmployeeId = "EmployeeId";
            directoryUser.FirstName = "FirstName";
            directoryUser.LastName = "LastName";
            directoryUser.FullName = "FullName";
            directoryUser.LoginId = "LoginId";
            directoryUser.PhoneNumber = "1 555 555 5555";
            
            DirectorySearchService.Expect(a => a.FindUser(searchTerm)).Return(directoryUser).Repeat.Any();
            #endregion Arrange

            #region Act
            var result = Controller.FindUser(searchTerm)
                .AssertResultIs<JsonResult>();
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
            var result2 = (ServiceUser)result.Data;
            Assert.IsNotNull(result2);
            Assert.AreEqual("bobby@bill.com", result2.Email);
            Assert.AreEqual("FirstName", result2.FirstName);
            Assert.IsNull(result2.FullNameAndLogin);
            Assert.AreEqual("LastName", result2.LastName);
            Assert.AreEqual("LoginId", result2.Login);
            Assert.AreEqual("1 555 555 5555", result2.Phone);
            Assert.IsNull(result2.Roles);
            Assert.IsNull(result2.Units);

            Assert.AreEqual(JsonRequestBehavior.AllowGet, result.JsonRequestBehavior);
            DirectorySearchService.AssertWasCalled(a => a.FindUser(searchTerm));
            #endregion Assert
        }

        #endregion FindUser Tests

        #region InsertNewUser Tests

        [TestMethod]
        [ExpectedException(typeof(UCDArch.Core.Utils.PreconditionException))]
        public void TestInsertNewUserThrowsExceptionIfNewUserIsNotValid()
        {
            const string applicationName = "SomeApp";
            ControllerRecordFakes.FakeUsers(3, UserRepository);
            AutomapperConfig.Configure();            
            try
            {
                #region Arrange
                var serviceUser = new ServiceUser();
                serviceUser.Login = null;
                serviceUser.LastName = null;
                serviceUser.FirstName = "FirstName";
                #endregion Arrange

                #region Act
                Controller.InsertNewUser(applicationName, serviceUser, 1, 1);
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
                Assert.AreEqual("User not valid: may not be null or empty", ex.Message);
                throw;
            }	
        }

        /// <summary>
        /// User not found so added
        /// permissions not found so added
        /// unit associations not found so added
        /// </summary>
        [TestMethod]
        public void TestInsertNewUserReturnsExpectedValue1()
        {
            #region Arrange
            const string applicationName = "Name2";
            ControllerRecordFakes.FakeUsers(3, UserRepository);
            AutomapperConfig.Configure();
            var serviceUser = CreateValidEntities.ServiceUser(4);
            ControllerRecordFakes.FakeApplications(3, ApplicationRepository);
            ControllerRecordFakes.FakeRoles(3, RoleRepository);
            ControllerRecordFakes.FakeUnits(3, UnitRepository);
            ControllerRecordFakes.FakePermissions(3, PermissionRepository);
            ControllerRecordFakes.FakeUnitAssociations(3, UnitAssociationRepository);
            #endregion Arrange

            #region Act
            var result = Controller.InsertNewUser(applicationName, serviceUser, 3, 2)
                .AssertResultIs<JsonResult>();
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            var result2 = (ServiceUser)result.Data;
            Assert.IsNotNull(result2);
            Assert.AreEqual("test4@testy.com", result2.Email);
            Assert.AreEqual("FirstName4", result2.FirstName);
            Assert.AreEqual("LastName4", result2.LastName);
            Assert.AreEqual("LoginId4", result2.Login);
            Assert.AreEqual("+1 530 755 7777", result2.Phone);

            UserRepository.AssertWasCalled(a => a.EnsurePersistent(Arg<User>.Is.Anything));
            var userArgs = (User)UserRepository.GetArgumentsForCallsMadeOn(a => a.EnsurePersistent(Arg<User>.Is.Anything))[0][0];
            Assert.AreEqual("test4@testy.com", userArgs.Email);
            Assert.AreEqual("FirstName4", userArgs.FirstName);
            Assert.IsFalse(userArgs.Inactive);
            Assert.AreEqual("LastName4", userArgs.LastName);
            Assert.AreEqual("LoginId4", userArgs.LoginId);
            Assert.AreEqual("+1 530 755 7777", userArgs.Phone);

            PermissionRepository.AssertWasCalled(a => a.EnsurePersistent(Arg<Permission>.Is.Anything));
            var permissionArgs = (Permission) PermissionRepository.GetArgumentsForCallsMadeOn(a => a.EnsurePersistent(Arg<Permission>.Is.Anything))[0][0];
            Assert.IsNotNull(permissionArgs);
            Assert.AreEqual(applicationName, permissionArgs.Application.ToString());
            Assert.AreEqual("Name3", permissionArgs.Role.Name);
            Assert.IsFalse(permissionArgs.Inactive);
            Assert.AreEqual(userArgs.LoginId, permissionArgs.User.LoginId);

            UnitAssociationRepository.AssertWasCalled(a => a.EnsurePersistent(Arg<UnitAssociation>.Is.Anything));
            var unitAssociationArgs = (UnitAssociation)UnitAssociationRepository.GetArgumentsForCallsMadeOn(a => a.EnsurePersistent(Arg<UnitAssociation>.Is.Anything))[0][0]; 
            Assert.IsNotNull(unitAssociationArgs);
            Assert.AreEqual(applicationName, unitAssociationArgs.Application.ToString());
            Assert.AreEqual("ShortName2", unitAssociationArgs.Unit.ShortName);
            Assert.IsFalse(unitAssociationArgs.Inactive);
            Assert.AreEqual(userArgs.LoginId, unitAssociationArgs.User.LoginId);

            #endregion Assert		
        }

        /// <summary>
        /// User found, but permissions and units not so those are added
        /// </summary>
        [TestMethod]
        public void TestInsertNewUserReturnsExpectedValue2()
        {
            #region Arrange
            const string applicationName = "Name2";
            ControllerRecordFakes.FakeUsers(5, UserRepository);  //Now user will be found
            AutomapperConfig.Configure();
            var serviceUser = CreateValidEntities.ServiceUser(4);
            ControllerRecordFakes.FakeApplications(3, ApplicationRepository);
            ControllerRecordFakes.FakeRoles(3, RoleRepository);
            ControllerRecordFakes.FakeUnits(3, UnitRepository);
            ControllerRecordFakes.FakePermissions(3, PermissionRepository);
            ControllerRecordFakes.FakeUnitAssociations(3, UnitAssociationRepository);
            #endregion Arrange

            #region Act
            var result = Controller.InsertNewUser(applicationName, serviceUser, 3, 2)
                .AssertResultIs<JsonResult>();
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            var result2 = (ServiceUser)result.Data;
            Assert.IsNotNull(result2);
            Assert.AreEqual("test4@testy.com", result2.Email);
            Assert.AreEqual("FirstName4", result2.FirstName);
            Assert.AreEqual("LastName4", result2.LastName);
            Assert.AreEqual("LoginId4", result2.Login);
            Assert.AreEqual("+1 530 755 7777", result2.Phone);

            UserRepository.AssertWasNotCalled(a => a.EnsurePersistent(Arg<User>.Is.Anything));

            PermissionRepository.AssertWasCalled(a => a.EnsurePersistent(Arg<Permission>.Is.Anything));
            var permissionArgs = (Permission)PermissionRepository.GetArgumentsForCallsMadeOn(a => a.EnsurePersistent(Arg<Permission>.Is.Anything))[0][0];
            Assert.IsNotNull(permissionArgs);
            Assert.AreEqual(applicationName, permissionArgs.Application.ToString());
            Assert.AreEqual("Name3", permissionArgs.Role.Name);
            Assert.IsFalse(permissionArgs.Inactive);
            Assert.AreEqual("LoginId4", permissionArgs.User.LoginId);

            UnitAssociationRepository.AssertWasCalled(a => a.EnsurePersistent(Arg<UnitAssociation>.Is.Anything));
            var unitAssociationArgs = (UnitAssociation)UnitAssociationRepository.GetArgumentsForCallsMadeOn(a => a.EnsurePersistent(Arg<UnitAssociation>.Is.Anything))[0][0];
            Assert.IsNotNull(unitAssociationArgs);
            Assert.AreEqual(applicationName, unitAssociationArgs.Application.ToString());
            Assert.AreEqual("ShortName2", unitAssociationArgs.Unit.ShortName);
            Assert.IsFalse(unitAssociationArgs.Inactive);
            Assert.AreEqual("LoginId4", unitAssociationArgs.User.LoginId);

            #endregion Assert
        }

        /// <summary>
        /// User, permissions, and unit associations found so not added, and because the test populates these values the are returned in the result
        /// </summary>
        [TestMethod]
        public void TestInsertNewUserReturnsExpectedValue3()
        {
            #region Arrange
            const string applicationName = "Name2";
            var users = new List<User>();
            users.Add(CreateValidEntities.User(4));
            ControllerRecordFakes.FakeUsers(0, UserRepository, users);

            AutomapperConfig.Configure();
            var serviceUser = CreateValidEntities.ServiceUser(4);

            ControllerRecordFakes.FakeApplications(3, ApplicationRepository);
            ControllerRecordFakes.FakeRoles(3, RoleRepository);
            ControllerRecordFakes.FakeUnits(3, UnitRepository);

            var permissions = new List<Permission>();
            permissions.Add(CreateValidEntities.Permission(1));
            permissions[0].Application = ApplicationRepository.GetNullableById(2);
            permissions[0].User = users[0];
            permissions[0].Role = RoleRepository.GetNullableById(3);
            permissions.Add(CreateValidEntities.Permission(2));
            permissions[1].Application = ApplicationRepository.GetNullableById(2);
            permissions[1].User = users[0];
            permissions[1].Role = RoleRepository.GetNullableById(1);
            ControllerRecordFakes.FakePermissions(0, PermissionRepository, permissions);

            var unitAssociations = new List<UnitAssociation>();
            unitAssociations.Add(CreateValidEntities.UnitAssociation(1));
            unitAssociations[0].Application = ApplicationRepository.GetNullableById(2);
            unitAssociations[0].User = users[0];
            unitAssociations[0].Unit = UnitRepository.GetNullableById(2);
            unitAssociations.Add(CreateValidEntities.UnitAssociation(2));
            unitAssociations[1].Application = ApplicationRepository.GetNullableById(2);
            unitAssociations[1].User = users[0];
            unitAssociations[1].Unit = UnitRepository.GetNullableById(1);
            ControllerRecordFakes.FakeUnitAssociations(0, UnitAssociationRepository, unitAssociations);
            #endregion Arrange

            #region Act
            var result = Controller.InsertNewUser(applicationName, serviceUser, 3, 2)
                .AssertResultIs<JsonResult>();
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            var result2 = (ServiceUser)result.Data;
            Assert.IsNotNull(result2);
            Assert.AreEqual("test4@testy.com", result2.Email);
            Assert.AreEqual("FirstName4", result2.FirstName);
            Assert.AreEqual("LastName4", result2.LastName);
            Assert.AreEqual("LoginId4", result2.Login);
            Assert.AreEqual("+1 530 755 7777", result2.Phone);
            Assert.AreEqual("Name1, Name3", result2.Roles);
            Assert.AreEqual("ShortName1, ShortName2", result2.Units);

            UserRepository.AssertWasNotCalled(a => a.EnsurePersistent(Arg<User>.Is.Anything));
            PermissionRepository.AssertWasNotCalled(a => a.EnsurePersistent(Arg<Permission>.Is.Anything));
            UnitAssociationRepository.AssertWasNotCalled(a => a.EnsurePersistent(Arg<UnitAssociation>.Is.Anything));


            #endregion Assert
        }

        #endregion InsertNewUser Tests
    }
}
