using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Catbert4.Controllers;
using Catbert4.Core.Domain;
using Catbert4.Helpers;
using Catbert4.Models;
using Catbert4.Services;
using Catbert4.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Catbert4.Tests.Core.Extensions;
using MvcContrib.TestHelper;
using UCDArch.Web.ActionResults;
using UCDArch.Web.Attributes;
using Rhino;
using Rhino.Mocks;

namespace Catbert4.Tests.Controllers.UserManagementControllerTests
{
    public partial class UserManagementControllerTests
    {
        #region AddUnit Tests

        [TestMethod]
        public void TestAddUnit()
        {
            #region Arrange
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] {""});
            ControllerRecordFakes.FakeApplications(3, ApplicationRepository);
            ControllerRecordFakes.FakeUnits(3, UnitRepository);
            ControllerRecordFakes.FakeUsers(3, UserRepository);
            ControllerRecordFakes.FakeUnitAssociations(1, UnitAssociationRepository);

            UserService.Expect(a => a.CanUserManageGivenLogin("Name2", "UserName", "LoginId1")).Return(true).Repeat.Any();
            UnitAssociationRepository.Expect(a => a.EnsurePersistent(Arg<UnitAssociation>.Is.Anything)).Repeat.Any();
            #endregion Arrange

            #region Act
            Controller.AddUnit("Name2", "LoginId1", 3);
            #endregion Act

            #region Assert
            UserService.AssertWasCalled(a => a.CanUserManageGivenLogin("Name2", "UserName", "LoginId1"));
            UnitAssociationRepository.AssertWasCalled(a => a.EnsurePersistent(Arg<UnitAssociation>.Is.Anything));
            var args = (UnitAssociation) UnitAssociationRepository.GetArgumentsForCallsMadeOn(a => a.EnsurePersistent(Arg<UnitAssociation>.Is.Anything))[0][0]; 
            Assert.IsNotNull(args);
            Assert.AreEqual("Name2", args.Application.ToString());
            Assert.AreEqual("LoginId1", args.User.LoginId);
            Assert.AreEqual("ShortName3", args.Unit.ShortName);
            Assert.IsFalse(args.Inactive);
            #endregion Assert		
        }


        [TestMethod]
        [ExpectedException(typeof(UCDArch.Core.Utils.PreconditionException))]
        public void TestAddUnitThrowsExceptionIfCurrentUserDoesNotHaveRights()
        {
            try
            {
                #region Arrange
                Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { "" });
                ControllerRecordFakes.FakeApplications(3, ApplicationRepository);
                ControllerRecordFakes.FakeUnits(3, UnitRepository);
                ControllerRecordFakes.FakeUsers(3, UserRepository);
                ControllerRecordFakes.FakeUnitAssociations(1, UnitAssociationRepository);

                UserService.Expect(a => a.CanUserManageGivenLogin("Name2", "UserName", "LoginId1")).Return(false).Repeat.Any();
                UnitAssociationRepository.Expect(a => a.EnsurePersistent(Arg<UnitAssociation>.Is.Anything)).Repeat.Any();
                #endregion Arrange

                #region Act
                Controller.AddUnit("Name2", "LoginId1", 3);
                #endregion Act
            }
            catch (Exception ex)
            {
                #region Assert
                Assert.IsNotNull(ex);
                Assert.AreEqual("UserName does not have access to manage LoginId1 within the Name2 application", ex.Message);
                UserService.AssertWasCalled(a => a.CanUserManageGivenLogin("Name2", "UserName", "LoginId1"));
                UnitAssociationRepository.AssertWasNotCalled(a => a.EnsurePersistent(Arg<UnitAssociation>.Is.Anything));
                #endregion Assert
                throw;
            }		
        }

        /// <summary>
        /// This would probably be caught by the UserService before the "Single()", but maybe not if there was a duplicate
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestAddUnitThrowsExceptionIfApplicationNotFound()
        {
            try
            {
                #region Arrange
                Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { "" });
                ControllerRecordFakes.FakeApplications(3, ApplicationRepository);
                ControllerRecordFakes.FakeUnits(3, UnitRepository);
                ControllerRecordFakes.FakeUsers(3, UserRepository);
                ControllerRecordFakes.FakeUnitAssociations(1, UnitAssociationRepository);

                UserService.Expect(a => a.CanUserManageGivenLogin("Name4", "UserName", "LoginId1")).Return(true).Repeat.Any();
                UnitAssociationRepository.Expect(a => a.EnsurePersistent(Arg<UnitAssociation>.Is.Anything)).Repeat.Any();
                #endregion Arrange

                #region Act
                Controller.AddUnit("Name4", "LoginId1", 3);
                #endregion Act
            }
            catch (Exception ex)
            {
                #region Assert
                Assert.IsNotNull(ex);
                Assert.AreEqual("Sequence contains no elements", ex.Message);
                UserService.AssertWasCalled(a => a.CanUserManageGivenLogin("Name4", "UserName", "LoginId1"));
                UnitAssociationRepository.AssertWasNotCalled(a => a.EnsurePersistent(Arg<UnitAssociation>.Is.Anything));
                #endregion Assert
                throw;
            }
        }

        /// <summary>
        /// This would probably be caught by the UserService before the "Single()", but maybe not if there was a duplicate
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestAddUnitThrowsExceptionIfloginIdDuplicate()
        {
            try
            {
                #region Arrange
                Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { "" });
                ControllerRecordFakes.FakeApplications(3, ApplicationRepository);
                ControllerRecordFakes.FakeUnits(3, UnitRepository);
                var users = new List<User>();
                users.Add(CreateValidEntities.User(1));
                users.Add(CreateValidEntities.User(1));
                ControllerRecordFakes.FakeUsers(3, UserRepository, users);
                ControllerRecordFakes.FakeUnitAssociations(1, UnitAssociationRepository);

                UserService.Expect(a => a.CanUserManageGivenLogin("Name2", "UserName", "LoginId1")).Return(true).Repeat.Any();
                UnitAssociationRepository.Expect(a => a.EnsurePersistent(Arg<UnitAssociation>.Is.Anything)).Repeat.Any();
                #endregion Arrange

                #region Act
                Controller.AddUnit("Name2", "LoginId1", 3);
                #endregion Act
            }
            catch (Exception ex)
            {
                #region Assert
                Assert.IsNotNull(ex);
                Assert.AreEqual("Sequence contains more than one element", ex.Message);
                UserService.AssertWasCalled(a => a.CanUserManageGivenLogin("Name2", "UserName", "LoginId1"));
                UnitAssociationRepository.AssertWasNotCalled(a => a.EnsurePersistent(Arg<UnitAssociation>.Is.Anything));
                #endregion Assert
                throw;
            }
        }

        [TestMethod]
        public void TestAddUnitWhereUnitAssociationAlreadyExists()
        {
            #region Arrange
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { "" });
            ControllerRecordFakes.FakeApplications(3, ApplicationRepository);
            ControllerRecordFakes.FakeUnits(3, UnitRepository);
            ControllerRecordFakes.FakeUsers(3, UserRepository);
            var unitAssociations = new List<UnitAssociation>();
            unitAssociations.Add(CreateValidEntities.UnitAssociation(1));
            unitAssociations[0].Application = ApplicationRepository.GetNullableById(2);
            unitAssociations[0].User = UserRepository.GetNullableById(1);
            unitAssociations[0].Unit = UnitRepository.GetNullableById(3);
            ControllerRecordFakes.FakeUnitAssociations(1, UnitAssociationRepository, unitAssociations);

            UserService.Expect(a => a.CanUserManageGivenLogin("Name2", "UserName", "LoginId1")).Return(true).Repeat.Any();
            UnitAssociationRepository.Expect(a => a.EnsurePersistent(Arg<UnitAssociation>.Is.Anything)).Repeat.Any();
            #endregion Arrange

            #region Act
            Controller.AddUnit("Name2", "LoginId1", 3);
            #endregion Act

            #region Assert
            UserService.AssertWasCalled(a => a.CanUserManageGivenLogin("Name2", "UserName", "LoginId1"));
            UnitAssociationRepository.AssertWasNotCalled(a => a.EnsurePersistent(Arg<UnitAssociation>.Is.Anything));

            #endregion Assert
        }


        #endregion AddUnit Tests
    }
}
