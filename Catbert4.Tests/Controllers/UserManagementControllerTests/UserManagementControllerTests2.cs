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


        [TestMethod]
        public void TestInsertNewUserReturnsExpectedValue1()
        {
            #region Arrange
            Assert.Inconclusive("Finish this test");
            const string applicationName = "Name2";
            ControllerRecordFakes.FakeUsers(3, UserRepository);
   
            var serviceUser = CreateValidEntities.ServiceUser(4);
            ControllerRecordFakes.FakeApplications(3, ApplicationRepository);
            #endregion Arrange

            #region Act

            Controller.InsertNewUser(applicationName, serviceUser, 1, 1);

            #endregion Act

            #region Assert

            #endregion Assert		
        }


        #endregion InsertNewUser Tests
    }
}
