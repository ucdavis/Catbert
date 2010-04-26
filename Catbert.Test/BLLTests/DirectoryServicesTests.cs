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
        public void CanGetExampleStudentPostit()
        {
            var student = DirectoryServices.FindStudent("991333530");

            Assert.IsNotNull(student);
            Assert.AreEqual("postit", student.LoginID);
        }

        [TestMethod]
        public void CanGetExampleStudentAnlai()
        {
            var student = DirectoryServices.FindStudent("992631147");

            Assert.IsNotNull(student);
            Assert.AreEqual("anlai", student.LoginID);
        }

        [TestMethod]
        public void FakeStudentIdReturnsNull()
        {
            var student = DirectoryServices.FindStudent("999999999");

            Assert.IsNull(student);
        }

        [TestMethod]
        public void NullStudentIdReturnsNull()
        {
            var student = DirectoryServices.FindStudent(null);

            Assert.IsNull(student);
        }
    }
}
