using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAESDO.Catbert.BLL;

namespace CAESDO.Catbert.Test.BLLTests
{
    [TestClass]
    public class DirectoryServicesTests
    {
        [TestMethod]
        public void CanGetPostit()
        {
            var user = DirectoryServices.FindUser("postit");
            
            Assert.IsNotNull(user);
            Assert.AreEqual("srkirkland@ucdavis.edu", user.EmailAddress);
            Assert.AreEqual("Kirkland", user.LastName);
        }

        [TestMethod]
        public void CanGetExampleStudent()
        {
            var student = DirectoryServices.FindStudent("175580");

            Assert.IsNotNull(student);
            Assert.AreEqual("azxnguye", student.LoginID);
        }
    }
}
