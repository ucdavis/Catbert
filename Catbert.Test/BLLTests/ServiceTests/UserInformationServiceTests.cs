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
        [TestMethod]
        [ExpectedException(typeof(PreconditionException))]
        public void GettingUserInformationShouldThrowPreconditionExceptionIfNoLoginIdGiven()
        {
            var userinfo = UserInformationServiceBLL.GetInformationByLoginId(null);
        }

        [TestMethod]
        [ExpectedException(typeof(PreconditionException))]
        public void GettingUserInformationShouldThrowPreconditionExceptionIfLoginIdGivenIsEmpty()
        {
            var userinfo = UserInformationServiceBLL.GetInformationByLoginId(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(PreconditionException))]
        public void GettingUserInformationForInvalidUserThrowsPreconditionException()
        {
            const string invalidUserId = "BADUSER";

            var userinfo = UserInformationServiceBLL.GetInformationByLoginId(invalidUserId);
        }

        [TestInitialize]
        public void AddUnitAndPermissionAssociations()
        {
            var user = UserBLL.GetByID(1);
            const string appName = "App0";

            //Associate with two units in app0
            UnitAssociationBLL.AssociateUnit(user.LoginID, appName, "Uni0", user.LoginID);
            UnitAssociationBLL.AssociateUnit(user.LoginID, appName, "Uni1", user.LoginID);

            //Associate with two roles in app0
            PermissionBLL.InsertPermission(appName, "Role0", user.LoginID, user.LoginID);
            PermissionBLL.InsertPermission(appName, "Role1", user.LoginID, user.LoginID);

            //Now associate with a unit and role in another app
            const string appName2 = "App1";

            UnitAssociationBLL.AssociateUnit(user.LoginID, appName2, "Uni0", user.LoginID);
            PermissionBLL.InsertPermission(appName2, "Role1", user.LoginID, user.LoginID);
        }
    }
}
