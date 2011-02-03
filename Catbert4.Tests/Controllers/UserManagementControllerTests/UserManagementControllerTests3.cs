using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Catbert4.Core.Domain;
using Catbert4.Helpers;
using Catbert4.Models;
using Catbert4.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using Rhino.Mocks;

namespace Catbert4.Tests.Controllers.UserManagementControllerTests
{
    public partial class UserManagementControllerTests
    {
        #region LoadUser Tests


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestLoadUserThrowsExceptionIfUserNotFound()
        {
            try
            {
                #region Arrange
                ControllerRecordFakes.FakeUsers(3, UserRepository);
                #endregion Arrange

                #region Act
                Controller.LoadUser("blah", "LoginId4");
                #endregion Act
            }
            catch (Exception ex)
            {
                #region Assert
                Assert.IsNotNull(ex);
                Assert.AreEqual("Sequence contains no elements", ex.Message);
                #endregion Assert
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestLoadUserThrowsExceptionIfTwoUsersFound()
        {
            try
            {
                #region Arrange
                var users = new List<User>();
                users.Add(CreateValidEntities.User(2));
                ControllerRecordFakes.FakeUsers(3, UserRepository, users);
                #endregion Arrange

                #region Act
                Controller.LoadUser("blah", "LoginId2");
                #endregion Act
            }
            catch (Exception ex)
            {
                #region Assert
                Assert.IsNotNull(ex);
                Assert.AreEqual("Sequence contains more than one element", ex.Message);
                #endregion Assert
                throw;
            }
        }

        [TestMethod]
        public void TestLoadUserReturnsExpectedResult()
        {
            #region Arrange
            AutomapperConfig.Configure();
            ControllerRecordFakes.FakeUsers(3, UserRepository);
            ControllerRecordFakes.FakeApplications(3, ApplicationRepository);
            ControllerRecordFakes.FakeRoles(5, RoleRepository);
            ControllerRecordFakes.FakeUnits(6, UnitRepository);

            var permissions = new List<Permission>();
            for (int i = 0; i < 3; i++)
            {
                permissions.Add(CreateValidEntities.Permission(i+1));
                permissions[i].Application = ApplicationRepository.GetNullableById(1);
                permissions[i].User = UserRepository.GetNullableById(2);
                permissions[i].Role = RoleRepository.GetNullableById(i + 1);
            }
            permissions[2].Role = RoleRepository.Queryable.Last();
            ControllerRecordFakes.FakePermissions(4, PermissionRepository, permissions); //Total of 7, but only 3 that match

            var unitAssociations = new List<UnitAssociation>();
            for (int i = 0; i < 3; i++)
            {
                unitAssociations.Add(CreateValidEntities.UnitAssociation(i+1));
                unitAssociations[i].Application = ApplicationRepository.GetNullableById(1);
                unitAssociations[i].User = UserRepository.GetNullableById(2);
                unitAssociations[i].Unit = UnitRepository.GetNullableById(i + 1);
            }
            unitAssociations[0].Unit = UnitRepository.Queryable.Last();
            ControllerRecordFakes.FakeUnitAssociations(2, UnitAssociationRepository, unitAssociations); //Total of 5, 3 that match

            #endregion Arrange

            #region Act
            var result = Controller.LoadUser("Name1", "LoginId2")
                .AssertResultIs<JsonResult>();
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(JsonRequestBehavior.AllowGet, result.JsonRequestBehavior);

            var result2 = (UserShowModel)result.Data;
            Assert.IsNotNull(result2);
            Assert.AreEqual("test2@testy.com", result2.Email);
            Assert.AreEqual("FirstName2", result2.FirstName);
            Assert.AreEqual("FirstName2 LastName2 (LoginId2)", result2.FullNameAndLogin);
            Assert.AreEqual("LastName2", result2.LastName);
            Assert.AreEqual("LoginId2", result2.Login);
            Assert.AreEqual("+1 530 763 0395", result2.Phone);                                    

            #region Permissions Asserts
            Assert.AreEqual(3, result2.Permissions.Count);

            Assert.AreEqual(1, result2.Permissions[0].Id);
            //Assert.AreEqual(null, result2.Permissions[0].ApplicationName);
            Assert.AreEqual(1, result2.Permissions[0].RoleId);
            Assert.AreEqual("Name1", result2.Permissions[0].RoleName);

            Assert.AreEqual(2, result2.Permissions[1].Id);
            //Assert.AreEqual(null, result2.Permissions[1].ApplicationName);
            Assert.AreEqual(2, result2.Permissions[1].RoleId);
            Assert.AreEqual("Name2", result2.Permissions[1].RoleName);

            Assert.AreEqual(3, result2.Permissions[2].Id);
            //Assert.AreEqual(null, result2.Permissions[2].ApplicationName);
            Assert.AreEqual(5, result2.Permissions[2].RoleId);
            Assert.AreEqual("Name5", result2.Permissions[2].RoleName);
            #endregion Permissions Asserts

            #region UnitAssociations Asserts
            Assert.AreEqual(3, result2.UnitAssociations.Count);
            
            Assert.AreEqual(1, result2.UnitAssociations[0].Id);
            //Assert.AreEqual(null, result2.UnitAssociations[0].ApplicationName);
            Assert.AreEqual(6, result2.UnitAssociations[0].UnitId);
            Assert.AreEqual("ShortName6", result2.UnitAssociations[0].UnitName);

            Assert.AreEqual(2, result2.UnitAssociations[1].Id);
            //Assert.AreEqual(null, result2.UnitAssociations[1].ApplicationName);
            Assert.AreEqual(2, result2.UnitAssociations[1].UnitId);
            Assert.AreEqual("ShortName2", result2.UnitAssociations[1].UnitName);

            Assert.AreEqual(3, result2.UnitAssociations[2].Id);
            //Assert.AreEqual(null, result2.UnitAssociations[2].ApplicationName);
            Assert.AreEqual(3, result2.UnitAssociations[2].UnitId);
            Assert.AreEqual("ShortName3", result2.UnitAssociations[2].UnitName);
            #endregion UnitAssociations Asserts
            #endregion Assert
        }


        #endregion LoadUser Tests

        #region RemoveUnit Tests

        [TestMethod]
        public void TestRemoveUnit()
        {
            #region Arrange
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] {""});
            ControllerRecordFakes.FakeApplications(3, ApplicationRepository);
            ControllerRecordFakes.FakeUnits(3, UnitRepository);
            ControllerRecordFakes.FakeUsers(3, UserRepository);

            var unitAssociations = new List<UnitAssociation>();
            for (int i = 0; i < 3; i++)
            {
                unitAssociations.Add(CreateValidEntities.UnitAssociation(i+1));
                unitAssociations[i].Application = ApplicationRepository.GetNullableById(2);
                unitAssociations[i].User = UserRepository.GetNullableById(1);
                unitAssociations[i].Unit = UnitRepository.GetNullableById(i + 1);
            }
            ControllerRecordFakes.FakeUnitAssociations(2, UnitAssociationRepository, unitAssociations);
            UserService.Expect(a => a.CanUserManageGivenLogin("Name2", "UserName", "LoginId1")).Return(true).Repeat.Any();
            #endregion Arrange

            #region Act
            Controller.RemoveUnit("Name2", "LoginId1", 2);
            #endregion Act

            #region Assert
            UserService.AssertWasCalled(a => a.CanUserManageGivenLogin("Name2", "UserName", "LoginId1"));
            UnitAssociationRepository.AssertWasCalled(a => a.Remove(Arg<UnitAssociation>.Is.Anything));
            var args = (UnitAssociation) UnitAssociationRepository.GetArgumentsForCallsMadeOn(a => a.Remove(Arg<UnitAssociation>.Is.Anything))[0][0]; 
            Assert.AreEqual(2, args.Id);
            Assert.AreEqual("ShortName2", args.Unit.ShortName);
            Assert.AreEqual("LoginId1", args.User.LoginId);
            Assert.AreEqual("Name2", args.Application.ToString());
            #endregion Assert		
        }

        [TestMethod]
        [ExpectedException(typeof(UCDArch.Core.Utils.PreconditionException))]
        public void TestRemoveUnitThrowsExceptionIfCurrentUserDoesNotHaveAccess()
        {
            try
            {
                #region Arrange
                Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { "" });
                ControllerRecordFakes.FakeApplications(3, ApplicationRepository);
                ControllerRecordFakes.FakeUnits(3, UnitRepository);
                ControllerRecordFakes.FakeUsers(3, UserRepository);

                var unitAssociations = new List<UnitAssociation>();
                for (int i = 0; i < 3; i++)
                {
                    unitAssociations.Add(CreateValidEntities.UnitAssociation(i + 1));
                    unitAssociations[i].Application = ApplicationRepository.GetNullableById(2);
                    unitAssociations[i].User = UserRepository.GetNullableById(1);
                    unitAssociations[i].Unit = UnitRepository.GetNullableById(i + 1);
                }
                ControllerRecordFakes.FakeUnitAssociations(2, UnitAssociationRepository, unitAssociations);
                UserService.Expect(a => a.CanUserManageGivenLogin("Name2", "UserName", "LoginId1")).Return(false).Repeat.Any();
                #endregion Arrange

                #region Act
                Controller.RemoveUnit("Name2", "LoginId1", 2);
                #endregion Act
            }
            catch (Exception ex)
            {
                #region Assert
                Assert.IsNotNull(ex);
                Assert.AreEqual("UserName does not have access to manage LoginId1 within the Name2 application", ex.Message);
                UserService.AssertWasCalled(a => a.CanUserManageGivenLogin("Name2", "UserName", "LoginId1"));
                UnitAssociationRepository.AssertWasNotCalled(a => a.Remove(Arg<UnitAssociation>.Is.Anything));
                #endregion Assert
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestRemoveUnitThrowsExceptionIfRecordNotFound1()
        {
            try
            {
                #region Arrange
                Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { "" });
                ControllerRecordFakes.FakeApplications(3, ApplicationRepository);
                ControllerRecordFakes.FakeUnits(3, UnitRepository);
                ControllerRecordFakes.FakeUsers(3, UserRepository);

                var unitAssociations = new List<UnitAssociation>();
                for (int i = 0; i < 3; i++)
                {
                    unitAssociations.Add(CreateValidEntities.UnitAssociation(i + 1));
                    unitAssociations[i].Application = ApplicationRepository.GetNullableById(2);
                    unitAssociations[i].User = UserRepository.GetNullableById(1);
                    unitAssociations[i].Unit = UnitRepository.GetNullableById(i + 1);
                }
                ControllerRecordFakes.FakeUnitAssociations(2, UnitAssociationRepository, unitAssociations);
                UserService.Expect(a => a.CanUserManageGivenLogin("Name3", "UserName", "LoginId1")).Return(true).Repeat.Any();
                #endregion Arrange

                #region Act
                Controller.RemoveUnit("Name3", "LoginId1", 2);
                #endregion Act
            }
            catch (Exception ex)
            {
                #region Assert
                Assert.IsNotNull(ex);
                Assert.AreEqual("Sequence contains no elements", ex.Message);
                UserService.AssertWasCalled(a => a.CanUserManageGivenLogin("Name3", "UserName", "LoginId1"));
                UnitAssociationRepository.AssertWasNotCalled(a => a.Remove(Arg<UnitAssociation>.Is.Anything));
                #endregion Assert
                throw;
            }
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestRemoveUnitThrowsExceptionIfRecordNotFound2()
        {
            try
            {
                #region Arrange
                Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { "" });
                ControllerRecordFakes.FakeApplications(3, ApplicationRepository);
                ControllerRecordFakes.FakeUnits(3, UnitRepository);
                ControllerRecordFakes.FakeUsers(3, UserRepository);

                var unitAssociations = new List<UnitAssociation>();
                for (int i = 0; i < 3; i++)
                {
                    unitAssociations.Add(CreateValidEntities.UnitAssociation(i + 1));
                    unitAssociations[i].Application = ApplicationRepository.GetNullableById(2);
                    unitAssociations[i].User = UserRepository.GetNullableById(1);
                    unitAssociations[i].Unit = UnitRepository.GetNullableById(i + 1);
                }
                ControllerRecordFakes.FakeUnitAssociations(2, UnitAssociationRepository, unitAssociations);
                UserService.Expect(a => a.CanUserManageGivenLogin("Name2", "UserName", "LoginId2")).Return(true).Repeat.Any();
                #endregion Arrange

                #region Act
                Controller.RemoveUnit("Name2", "LoginId2", 2);
                #endregion Act
            }
            catch (Exception ex)
            {
                #region Assert
                Assert.IsNotNull(ex);
                Assert.AreEqual("Sequence contains no elements", ex.Message);
                UserService.AssertWasCalled(a => a.CanUserManageGivenLogin("Name2", "UserName", "LoginId2"));
                UnitAssociationRepository.AssertWasNotCalled(a => a.Remove(Arg<UnitAssociation>.Is.Anything));
                #endregion Assert
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestRemoveUnitThrowsExceptionIfRecordNotFound3()
        {
            try
            {
                #region Arrange
                Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { "" });
                ControllerRecordFakes.FakeApplications(3, ApplicationRepository);
                ControllerRecordFakes.FakeUnits(3, UnitRepository);
                ControllerRecordFakes.FakeUsers(3, UserRepository);

                var unitAssociations = new List<UnitAssociation>();
                for (int i = 0; i < 3; i++)
                {
                    unitAssociations.Add(CreateValidEntities.UnitAssociation(i + 1));
                    unitAssociations[i].Application = ApplicationRepository.GetNullableById(2);
                    unitAssociations[i].User = UserRepository.GetNullableById(1);
                    unitAssociations[i].Unit = UnitRepository.GetNullableById(i + 1);
                }
                ControllerRecordFakes.FakeUnitAssociations(2, UnitAssociationRepository, unitAssociations);
                UserService.Expect(a => a.CanUserManageGivenLogin("Name2", "UserName", "LoginId1")).Return(true).Repeat.Any();
                #endregion Arrange

                #region Act
                Controller.RemoveUnit("Name2", "LoginId1", 7);
                #endregion Act
            }
            catch (Exception ex)
            {
                #region Assert
                Assert.IsNotNull(ex);
                Assert.AreEqual("Sequence contains no elements", ex.Message);
                UserService.AssertWasCalled(a => a.CanUserManageGivenLogin("Name2", "UserName", "LoginId1"));
                UnitAssociationRepository.AssertWasNotCalled(a => a.Remove(Arg<UnitAssociation>.Is.Anything));
                #endregion Assert
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestRemoveUnitThrowsExceptionIfRecordNotFound4()
        {
            try
            {
                #region Arrange
                Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { "" });
                ControllerRecordFakes.FakeApplications(3, ApplicationRepository);
                ControllerRecordFakes.FakeUnits(3, UnitRepository);
                ControllerRecordFakes.FakeUsers(3, UserRepository);

                var unitAssociations = new List<UnitAssociation>();
                for (int i = 0; i < 3; i++)
                {
                    unitAssociations.Add(CreateValidEntities.UnitAssociation(i + 1));
                    unitAssociations[i].Application = ApplicationRepository.GetNullableById(2);
                    unitAssociations[i].User = UserRepository.GetNullableById(1);
                    unitAssociations[i].Unit = UnitRepository.GetNullableById(i + 1);
                }
                unitAssociations.Add(CreateValidEntities.UnitAssociation(4)); 
                unitAssociations[3].Application = ApplicationRepository.GetNullableById(2);
                unitAssociations[3].User = UserRepository.GetNullableById(1);
                unitAssociations[3].Unit = UnitRepository.GetNullableById(2); //Duplicate

                ControllerRecordFakes.FakeUnitAssociations(2, UnitAssociationRepository, unitAssociations);
                UserService.Expect(a => a.CanUserManageGivenLogin("Name2", "UserName", "LoginId1")).Return(true).Repeat.Any();
                #endregion Arrange

                #region Act
                Controller.RemoveUnit("Name2", "LoginId1", 2);
                #endregion Act
            }
            catch (Exception ex)
            {
                #region Assert
                Assert.IsNotNull(ex);
                Assert.AreEqual("Sequence contains more than one element", ex.Message);
                UserService.AssertWasCalled(a => a.CanUserManageGivenLogin("Name2", "UserName", "LoginId1"));
                UnitAssociationRepository.AssertWasNotCalled(a => a.Remove(Arg<UnitAssociation>.Is.Anything));
                #endregion Assert
                throw;
            }
        }


        #endregion RemoveUnit Tests
    }
}
