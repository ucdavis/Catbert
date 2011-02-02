using Catbert4.Controllers;
using Catbert4.Models;
using Catbert4.Tests.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Catbert4.Tests.Controllers.UserManagementControllerTests
{
    public partial class UserManagementControllerTests
    {
        #region Mapping Tests

        /// <summary>
        /// #1
        /// </summary>
        [TestMethod]
        public void TestManageMapping()
        {
            "~/UserManagement/Manage/".ShouldMapTo<UserManagementController>(a => a.Manage("application"), true);
        }

        /// <summary>
        /// #2
        /// </summary>
        [TestMethod]
        public void TestFindUserMapping()
        {
            "~/UserManagement/FindUser/".ShouldMapTo<UserManagementController>(a => a.FindUser("tester"), true);
        }

        /// <summary>
        /// #3
        /// </summary>
        [TestMethod]
        public void TestInsertNewUserMapping()
        {
            "~/UserManagement/InsertNewUser/".ShouldMapTo<UserManagementController>(a => a.InsertNewUser("appName", new ServiceUser(),1, 1), true);
        }

        #endregion Mapping Tests
    }
}
