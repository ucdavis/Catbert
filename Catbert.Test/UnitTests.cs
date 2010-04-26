using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAESDO.Catbert.Test
{
    /// <summary>
    /// Summary description for UnitTests
    /// </summary>
    [TestClass]
    public class UnitTests
    {
        public UnitTests()
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
        public void AddRemoveUnit()
        {
            Assert.Inconclusive("Not implemented");
        }

        [TestMethod]
        public void GetUnits()
        {
            //Get the units in order for easier comparison later.
            var units = CatbertManager.GetUnitsForUser(TestHelper.TestUser).OrderBy(u => u.UnitFIS).ToList();
            
            Assert.AreEqual<int>(3, units.Count());
            Assert.AreEqual("AANS", units[0].UnitFIS);
            Assert.AreEqual("ADNO", units[1].UnitFIS);
            Assert.AreEqual("APLS", units[2].UnitFIS);
        }

        [TestMethod]
        public void GetAllUnits()
        {
            var units = CatbertManager.GetAllUnits().OrderBy(unit => unit.UnitFIS).ToList();

            Assert.IsTrue(units.Count > 45, "There should be at least 45 units");
        }
    }
}
