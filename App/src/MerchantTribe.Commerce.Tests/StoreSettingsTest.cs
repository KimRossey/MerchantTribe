using MerchantTribe.Commerce.Accounts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MerchantTribe.Commerce.Tests
{
    
    
    /// <summary>
    ///This is a test class for StoreSettingsTest and is intended
    ///to contain all StoreSettingsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class StoreSettingsTest
    {


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
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for AllowApiToClearUntil
        ///</summary>
        [TestMethod()]
        public void DateTimeUtcStringTester()
        {
            DateTime current = DateTime.UtcNow;            

            string serialized = current.ToString("u");

            DateTime actual = DateTime.UtcNow.AddDays(-1);
            DateTime.TryParse(serialized, out actual);
            actual = actual.ToUniversalTime();

            Assert.AreEqual(current.ToString(), actual.ToString(), "String output doesn't match");            
        }
    }
}
