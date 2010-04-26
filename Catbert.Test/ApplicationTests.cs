using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAESDO.Catbert.Test
{
    /// <summary>
    /// Summary description for ApplicationTests
    /// </summary>
    [TestClass]
    public class ApplicationTests
    {
        public ApplicationTests()
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
        public void CheckCatbertUsers()
        {
            var users = CatbertManager.GetUsersInApplication().OrderBy(u => u.Login).ToList();

            Assert.AreEqual<int>(5, users.Count); //5 users with 7 roles

            Assert.AreEqual("adam", users[0].Login);
            Assert.AreEqual("anlai", users[1].Login);
            Assert.AreEqual("pgang", users[2].Login);
            Assert.AreEqual("postit", users[3].Login);
            Assert.AreEqual("taylorkj", users[4].Login);

            //anlai and postit should have multiple roles
            Assert.AreEqual<int>(2, users[1].Roles.Count());
            Assert.AreEqual<int>(2, users[3].Roles.Count());

            //Quick spot check
            Assert.AreEqual<int>(1, users[0].Roles.Count());
            Assert.AreEqual("Admin", users[0].Roles[0].Name);
        }
    }
}
