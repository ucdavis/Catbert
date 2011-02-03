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
        #region AddPermission Tests

        [TestMethod]
        public void TestAddPermission()
        {
            #region Arrange
            Assert.Inconclusive("Continue these tests");
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { "" });
            ControllerRecordFakes.FakeRoles(5, RoleRepository);
            ControllerRecordFakes.FakeApplications(3, ApplicationRepository);
            ControllerRecordFakes.FakeUsers(3, UserRepository);

            UserService.Expect(a => a.CanUserManageGivenLogin("Name2", "UserName", "LoginId1")).Return(true).Repeat.Any();
            RoleService.Expect(a => a.GetVisibleByUser("Name2", "UserName")).Return(RoleRepository.Queryable).Repeat.Any();
            #endregion Arrange

            #region Act

            #endregion Act

            #region Assert

            #endregion Assert		
        }


        #endregion AddPermission Tests
    }
}
