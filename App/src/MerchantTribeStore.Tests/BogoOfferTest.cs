using BVSoftware.Commerce.Marketing.Offers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;

namespace BVSoftware.UnitTests
{
    
    
    /// <summary>
    ///This is a test class for BogoOfferTest and is intended
    ///to contain all BogoOfferTest Unit Tests
    ///</summary>
    [TestClass()]
    public class BogoOfferTest
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
        ///A test for QualifyingQuantity
        ///</summary>
        [TestMethod()]
        public void QualifyingQuantityTest()
        {
            BogoSettings settings = new BogoSettings();
            settings.DiscountAmount = 0;
            settings.DiscountAmountType = AmountType.Percent;
            settings.QualifyingProducts.Add(new PurchasableItem() { ProductId = "ABC123" });
            settings.QualifyingProducts.Add(new PurchasableItem() { ProductId = "DEF456" });

            List<PurchasableItem> originalItems = new List<PurchasableItem>();
            originalItems.Add(new PurchasableItem() { ProductId = "ABC123", Quantity = 2 });

            Decimal expected = 2;
            Decimal actual;
            actual = BogoOffer.QualifyingQuantity(settings, originalItems);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void QualifyingQuantityTestNoQualify()
        {
            BogoSettings settings = new BogoSettings();
            settings.DiscountAmount = 0;
            settings.DiscountAmountType = AmountType.Percent;
            settings.QualifyingProducts.Add(new PurchasableItem() { ProductId = "ABC123" });
            settings.QualifyingProducts.Add(new PurchasableItem() { ProductId = "DEF456" });

            List<PurchasableItem> originalItems = new List<PurchasableItem>();
            originalItems.Add(new PurchasableItem() { ProductId = "ABCAAAAA123", Quantity = 2 });

            Decimal expected = 0;
            Decimal actual;
            actual = BogoOffer.QualifyingQuantity(settings, originalItems);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void QualifyingQuantityTestSmaller()
        {
            BogoSettings settings = new BogoSettings();
            settings.DiscountAmount = 0;
            settings.DiscountAmountType = AmountType.Percent;
            settings.QualifyingProducts.Add(new PurchasableItem() { ProductId = "ABC123" });
            settings.QualifyingProducts.Add(new PurchasableItem() { ProductId = "DEF456" });

            List<PurchasableItem> originalItems = new List<PurchasableItem>();
            originalItems.Add(new PurchasableItem() { ProductId = "ABC123", Quantity = 2 });
            originalItems.Add(new PurchasableItem() { ProductId = "DEF456", Quantity = 1 });

            Decimal expected = 1;
            Decimal actual;
            actual = BogoOffer.QualifyingQuantity(settings, originalItems);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void QualifyingQuantityTestMultiLineSmaller()
        {
            BogoSettings settings = new BogoSettings();
            settings.DiscountAmount = 0;
            settings.DiscountAmountType = AmountType.Percent;
            settings.QualifyingProducts.Add(new PurchasableItem() { ProductId = "ABC123" });
            settings.QualifyingProducts.Add(new PurchasableItem() { ProductId = "DEF456" });

            List<PurchasableItem> originalItems = new List<PurchasableItem>();
            originalItems.Add(new PurchasableItem() { ProductId = "ABC123", Quantity = 2 });
            originalItems.Add(new PurchasableItem() { ProductId = "DEF456", Quantity = 1 });
            originalItems.Add(new PurchasableItem() { ProductId = "DEF456", Quantity = 2 });

            Decimal expected = 2;
            Decimal actual;
            actual = BogoOffer.QualifyingQuantity(settings, originalItems);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void QualifyingQuantityTestMultiLine()
        {
            BogoSettings settings = new BogoSettings();
            settings.DiscountAmount = 0;
            settings.DiscountAmountType = AmountType.Percent;
            settings.QualifyingProducts.Add(new PurchasableItem() { ProductId = "ABC123" });
            settings.QualifyingProducts.Add(new PurchasableItem() { ProductId = "DEF456" });

            List<PurchasableItem> originalItems = new List<PurchasableItem>();
            originalItems.Add(new PurchasableItem() { ProductId = "DEF456", Quantity = 1 });
            originalItems.Add(new PurchasableItem() { ProductId = "DEF456", Quantity = 2 });

            Decimal expected = 3;
            Decimal actual;
            actual = BogoOffer.QualifyingQuantity(settings, originalItems);
            Assert.AreEqual(expected, actual);
        }

    }
}
