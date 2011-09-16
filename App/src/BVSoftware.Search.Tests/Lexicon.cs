using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BVSoftware.Search.Tests
{
    /// <summary>
    /// Summary description for WordLibrary
    /// </summary>
    [TestClass]
    public class Lexicon
    {
        public Lexicon()
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
        public void CanAddAWord()
        {
            string input = "thi";

            BVSoftware.Search.Lexicon lex = new BVSoftware.Search.Lexicon(new BVSoftware.Search.Data.StaticLexicon());

            long id = lex.AddOrCreateWord(input);

            Assert.IsTrue(id > 0);
        }

        [TestMethod]
        public void CanFindWord()
        {
            string input = "thi";

            BVSoftware.Search.Lexicon lex = new BVSoftware.Search.Lexicon(new BVSoftware.Search.Data.StaticLexicon());
            long expected = lex.AddOrCreateWord(input);
            Assert.IsTrue(expected > 0);

            long actual = lex.FindWordId(input);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanShowWordIsMissing()
        {
            string input = "thi";

            BVSoftware.Search.Lexicon lex = new BVSoftware.Search.Lexicon(new BVSoftware.Search.Data.StaticLexicon());

            long actual = lex.FindWordId(input);
            Assert.AreEqual(0, actual);
        }

        [TestMethod]
        public void CanFindTwoWords()
        {
            string input = "thi";
            string input2 = "test";

            BVSoftware.Search.Lexicon lex = new BVSoftware.Search.Lexicon(new BVSoftware.Search.Data.StaticLexicon());
            long expected = lex.AddOrCreateWord(input);
            Assert.IsTrue(expected > 0);
            long expected2 = lex.AddOrCreateWord(input2);
            Assert.IsTrue(expected2 > 0);

            long actual2 = lex.FindWordId(input2);
            Assert.AreEqual(expected2, actual2);

            long actual = lex.FindWordId(input);
            Assert.AreEqual(expected, actual);
        }
    }
}
