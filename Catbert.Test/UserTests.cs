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
            bool verified = CatbertManager.VerifyUser(TestHelper.TestUser);

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
            var result = CatbertManager.SearchNewUsers(null, null, null, TestHelper.TestUser);

            Assert.IsNotNull(result);
            Assert.AreEqual<int>(1, result.Count());
            Assert.AreEqual<string>(TestHelper.TestUser, result[0].Login);
        }

        [TestMethod]
        public void SetEmail()
        {
            //First get the user and check the email
            var user = CatbertManager.GetUser(TestHelper.TestUser);
            Assert.AreEqual(TestHelper.TestEmail, user.Email);

            //Now set the email address 
            var result = CatbertManager.SetEmail(TestHelper.TestUser, "fake@fake.com");
            Assert.IsTrue(result);

            //Get the user again and make sure the email has changed
            user = CatbertManager.GetUser(TestHelper.TestUser);
            Assert.AreEqual("fake@fake.com", user.Email);

            //Now set the email address back
            result = CatbertManager.SetEmail(TestHelper.TestUser, TestHelper.TestEmail);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SetEmailBadUser()
        {
            var result = CatbertManager.SetEmail("nonexistant", TestHelper.TestEmail);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void SetEmailBadAddress()
        {
            var result = CatbertManager.SetEmail(TestHelper.TestUser, "poorlyformatted@");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void SetPhone()
        {
            //set the phone number
            var result = CatbertManager.SetPhone(TestHelper.TestUser, "555-5555");
            Assert.IsTrue(result);

            //Now set the email address back
            result = CatbertManager.SetPhone(TestHelper.TestUser, TestHelper.TestPhone);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SetPhoneBadUser()
        {
            var result = CatbertManager.SetPhone("nonexistant", TestHelper.TestPhone);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void SetPhoneBadNumber()
        {
            var result = CatbertManager.SetPhone(TestHelper.TestUser, "545-asd4");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void InsertNewUser()
        {
            Assert.Inconclusive("Test Not Developed");
        }

    }
}
