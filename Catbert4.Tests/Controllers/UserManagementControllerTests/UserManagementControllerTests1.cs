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

        [TestMethod, Ignore]
        public void TestManageReturnsView()
        {
            #region Arrange
            //Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { "" });
            ControllerRecordFakes.FakeUnits(3, UnitRepository);
            UnitService.Expect(a => a.GetVisibleByUser("appName", "UserName")).Return(UnitRepository.GetAll().AsQueryable()).Repeat.Any();

            #endregion Arrange

            #region Act
            var result = Controller.Manage("appName", "SomeRole", "SomeUnit")
                .AssertViewRendered()
                .WithViewData<UserManagementViewModel>();
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            #endregion Assert		
        }


        #endregion Manage Tests
    }
}
