using Catbert4.Controllers;
using Catbert4.Tests.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Catbert4.Tests.Controllers.UserManagementControllerTests
{
    public partial class UserManagementControllerTests
    {
        #region Mapping Tests


        [TestMethod]
        public void TestManageMapping()
        {
            "~/UserManagement/Manage/".ShouldMapTo<UserManagementController>(a => a.Manage("application"), true);
        }

        [TestMethod]
        public void TestFindUserMapping()
        {
            "~/UserManagement/FindUser/".ShouldMapTo<UserManagementController>(a => a.FindUser("tester"), true);
        }

        #endregion Mapping Tests
    }
}
