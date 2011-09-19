using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BVSoftware.Bvc5.Core;
using BVSoftware.Bvc5.Core.Orders;
using BVSoftware.Bvc5.Core.BusinessRules;
using BVSoftware.Bvc5.Core.BusinessRules.OrderTasks;

namespace BVSoftware.UnitTests
{
    /// <summary>
    /// Summary description for BOGOBugTest
    /// </summary>
    [TestClass]
    public class BOGOBugTest
    {
        public BOGOBugTest()
        {
            
        }

        private TestContext testContextInstance;

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
        public void OneItemShouldNotContainFreeItem()
        {
            Order o = new Order();
            LineItem li = new LineItem();
            li.BasePrice = 1.50m;
            li.Bvin = System.Guid.NewGuid().ToString();
            //o.AddItem(li);

            //Assert.AreEqual(1, o.Items.Count);

            Assert.IsTrue(false);
            //BVSoftware.Bvc5.Core.Marketing.OfferList offers = 
            //    new BVSoftware.Bvc5.Core.Marketing.OfferList();
            //offers.Add(new BVSoftware.Bvc5.Core.Marketing.BuyOneGetOneOfferTaskProcessor());

            //ApplyOffersStackedDiscounts processor =
            //    new ApplyOffersStackedDiscounts(offers);

        }
    }
}
