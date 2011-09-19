using MerchantTribe.Commerce.Catalog;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using MerchantTribe.Commerce.Content.Parts;

namespace MerchantTribe.Commerce.Tests
{
    
    
    /// <summary>
    ///This is a test class for CategoryTest and is intended
    ///to contain all CategoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CategoryTest
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
        public void CanAddPageVersionToCategory()
        {
            Category target = new Category();
            Assert.AreEqual(0, target.Versions.Count, "Should be No Page Versions Yet");

            string newPageName = "Unit Test Version";
            CategoryPageVersion v = new CategoryPageVersion() { AdminName = newPageName };
            target.Versions.Add(v);
            Assert.IsNotNull(target.Versions[0], "Page Version should not be null");
            Assert.AreEqual(newPageName, target.Versions[0].AdminName, "Admin name should match");
        }

        [TestMethod]
        public void CanStoreAndRecoverCategoryWithPageVersion()
        {
            Category target = new Category();
            target.Name = "Test Category";
            target.ParentId = "0";
            target.StoreId = 0;
            target.SourceType = CategorySourceType.FlexPage;

            CategoryRepository repo = CategoryRepository.InstantiateForMemory(new RequestContext());

            Assert.IsTrue(repo.Create(target), "Create should be true");

            string categoryId = target.Bvin;
            Assert.AreNotEqual(string.Empty, categoryId, "Category Bvin should not be empty.");

            Category found = repo.Find(categoryId);
            Assert.IsNotNull(found, "Found category should not be null");
            Assert.AreEqual(found.Name, target.Name, "Names should match");

            Assert.AreEqual(1, found.Versions.Count, "Versions count should be one.");

            CategoryPageVersion foundVersion = found.Versions[0];
            Assert.IsNotNull(foundVersion, "Found page version should not be null.");
            Assert.IsTrue(foundVersion.Id > 0, "Found version should have Id > 0 assigned.");

        }
    }
}
