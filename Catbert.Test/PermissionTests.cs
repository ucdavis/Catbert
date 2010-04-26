using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAESDO.Catbert.Test
{
    /// <summary>
    /// Summary description for PermissionTests
    /// </summary>
    [TestClass]
    public class PermissionTests
    {
        public PermissionTests()
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

        string role = "Reader";
        string user = "postit";

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
        public void CheckPermissions()
        {
            //First remove the user from the role
            bool result = CatbertManager.RemoveUserFromRole(user, role);

            Assert.IsTrue(result); //make sure that worked
                        
            result = CatbertManager.AddUserToRole(user, role); //now add the user to the role

            Assert.IsTrue(result); //make sure that works

            result = CatbertManager.AddUserToRole(user, role); //try to add the user to the role again

            Assert.IsFalse(result); //that shouldn't work
        }

        [TestMethod]
        public void EnsureReaderPermission()
        {
            bool result = CatbertManager.IsUserInRole(user, role);

            Assert.IsTrue(result);
        }

        public void FailFakePermission()
        {
            bool result = CatbertManager.IsUserInRole(user, "FAKE");

            Assert.IsFalse(result);
        }
    }
}
