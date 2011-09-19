using MerchantTribe.Commerce;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace MerchantTribe.Commerce.Tests
{
    
    
    /// <summary>
    ///This is a test class for MerchantTribeApplicationTest and is intended
    ///to contain all MerchantTribeApplicationTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MerchantTribeApplicationTest
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


        [TestMethod()]
        public void CanAddSampleProductsToStore()
        {
            RequestContext c = new RequestContext();
            MerchantTribeApplication target = MerchantTribeApplication.InstantiateForMemory(c);
            target.AddSampleProductsToStore();

            int totalCount = target.CatalogServices.Products.CountOfAll();
            Assert.AreEqual(6, totalCount, "Six Products should have been created.");
            List<Catalog.CategorySnapshot> cats = target.CatalogServices.Categories.FindAll();
            Assert.IsNotNull(cats);
            Assert.AreEqual(4, cats.Count, "Four categories should have been created!");            
        }
    }
}
