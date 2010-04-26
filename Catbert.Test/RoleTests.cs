using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAESDO.Catbert.Test
{
    /// <summary>
    /// Summary description for RoleTests
    /// </summary>
    [TestClass]
    public class RoleTests
    {
        public RoleTests()
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
        public void GetCatbertRoles()
        {
            var roles = CatbertManager.GetRoles();

            Assert.AreEqual<int>(1, roles.Count());
            Assert.AreEqual("Admin", roles[0].Name);
        }

        [TestMethod]
        public void GetRolesForUser()
        {
            var roles = CatbertManager.GetRolesByUser(TestHelper.TestUser);

            Assert.AreEqual<int>(2, roles.Count());
            Assert.AreEqual("Admin", roles[0].Name);
            Assert.AreEqual("Reader", roles[1].Name);
        }
    }
}
