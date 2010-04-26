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
        }
    }
}
