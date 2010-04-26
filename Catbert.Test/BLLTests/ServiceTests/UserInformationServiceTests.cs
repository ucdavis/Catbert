using CAESArch.Core.Utils;
using CAESDO.Catbert.Test.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAESDO.Catbert.BLL.Service;
using CAESDO.Catbert.BLL;

namespace CAESDO.Catbert.Test.BLLTests.ServiceTests
{
    [TestClass]
    public class UserInformationServiceTests : DatabaseTestBase
    {
        private const string ValidLoginId = "login0";

        [TestMethod]
        [ExpectedException(typeof(PreconditionException))]
        public void GettingUserInformationShouldThrowPreconditionExceptionIfNoLoginIdGiven()
        {
            UserInformationServiceBLL.GetInformationByLoginId(null);
        }

        [TestMethod]
        [ExpectedException(typeof(PreconditionException))]
        public void GettingUserInformationShouldThrowPreconditionExceptionIfLoginIdGivenIsEmpty()
        {
            UserInformationServiceBLL.GetInformationByLoginId(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(PreconditionException))]
        public void GettingUserInformationForInvalidUserThrowsPreconditionException()
        {
            const string invalidUserId = "BADUSER";

            UserInformationServiceBLL.GetInformationByLoginId(invalidUserId);
        }

        [TestMethod]
        public void GettingUserInformationForValidUserSetsUserIdProperty()
        {
            var userinfo = UserInformationServiceBLL.GetInformationByLoginId(ValidLoginId);

            Assert.AreEqual(ValidLoginId, userinfo.LoginId);
        }

        [TestMethod]
        public void GettingUserInformationPopulatesPermissionAssociationsList()
        {
            var userinfo = UserInformationServiceBLL.GetInformationByLoginId(ValidLoginId);

            Assert.AreEqual(4, userinfo.PermissionAssociations.Count);
        }

        [TestMethod]
        public void GettingUserInformationPopulatesUnitAssociationsList()
        {
            var userinfo = UserInformationServiceBLL.GetInformationByLoginId(ValidLoginId);

            Assert.AreEqual(3, userinfo.UnitAssociations.Count);
        }

        [TestInitialize]
        public void AddUnitAndPermissionAssociations()
        {
            var user = UserBLL.GetUser(ValidLoginId);
            const string appName = "App0";

            //Associate with two units in app0
            UnitAssociationBLL.AssociateUnit(user.LoginID, appName, "Uni0", user.LoginID);
            UnitAssociationBLL.AssociateUnit(user.LoginID, appName, "Uni1", user.LoginID);

            //Associate with two roles in app0
            PermissionBLL.InsertPermission(appName, "Role0", user.LoginID, user.LoginID);
            PermissionBLL.InsertPermission(appName, "Role1", user.LoginID, user.LoginID);

            //Now associate with a unit and 2 roles in another app
            const string appName2 = "App1";

            UnitAssociationBLL.AssociateUnit(user.LoginID, appName2, "Uni0", user.LoginID);
            
            PermissionBLL.InsertPermission(appName2, "Role1", user.LoginID, user.LoginID);
            PermissionBLL.InsertPermission(appName2, "Role2", user.LoginID, user.LoginID);
        }
    }
}
