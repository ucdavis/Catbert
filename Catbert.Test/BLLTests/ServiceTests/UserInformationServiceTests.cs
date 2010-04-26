using CAESArch.Core.Utils;
using CAESDO.Catbert.Test.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAESDO.Catbert.BLL.Service;

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
    }
}
