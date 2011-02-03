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

        /// <summary>
        /// #4
        /// </summary>
        [TestMethod]
        public void TestLoadUserMapping()
        {
            "~/UserManagement/LoadUser/".ShouldMapTo<UserManagementController>(a => a.LoadUser("appname", "loginId"), true);
        }

        /// <summary>
        /// #5
        /// </summary>
        [TestMethod]
        public void TestRemoveUnitMapping()
        {
            "~/UserManagement/RemoveUnit/".ShouldMapTo<UserManagementController>(a => a.RemoveUnit("appName", "login", 5), true);
        }
        #endregion Mapping Tests
    }
}
