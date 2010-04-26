using CAESDO.Catbert.Test.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAESDO.Catbert.BLL;
using System.Linq;

namespace CAESDO.Catbert.Test.BLLTests
{
    [TestClass]
    public class UserManagementTests : DatabaseTestBase
    {
        [TestMethod]
        public void CanGetListOfAllUsers()
        {
            var users = UserBLL.GetAll();

            Assert.AreEqual(20, users.Count);
        }

        [TestMethod]
        public void CanGetAllActiveUsersOnly()
        {
            var activeUsers = UserBLL.GetAllActive();

            Assert.AreEqual(15, activeUsers.Count());

            foreach (var user in activeUsers)
            {
                Assert.IsFalse(user.Inactive);
            }
        }
    }
}
