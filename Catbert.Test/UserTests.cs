using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAESDO.Catbert.Test
{
    /// <summary>
    /// Summary description for UserTests
    /// </summary>
    [TestClass]
    public class UserTests
    {
        public UserTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void VerifyUserFound()
        {
            bool verified = CatbertManager.VerifyUser("postit");

            Assert.IsTrue(verified);
        }

        [TestMethod]
        public void VerifyUserFalse()
        {
            bool verified = CatbertManager.VerifyUser("fake");

            Assert.IsFalse(verified);
        }

        [TestMethod]
        public void SearchNewUser()
        {
            var result = CatbertManager.SearchNewUsers(null, null, null, "postit");

            Assert.IsNotNull(result);
            Assert.AreEqual<int>(1, result.Count());
            Assert.AreEqual<string>("postit", result[0].Login);
        }

        [TestMethod]
        public void InsertNewUser()
        {
            Assert.Inconclusive("Test Not Developed");
        }

    }
}
