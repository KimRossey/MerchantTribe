using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BVSoftware.Search;
using BVSoftware.Search.Data;

namespace BVSoftware.Search.Tests
{
    /// <summary>
    /// Summary description for SimpleIndexer
    /// </summary>
    [TestClass]
    public class SimpleTextIndexer
    {
        public SimpleTextIndexer()
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
        public void CanIndexAnObject()
        {
            string message = "Red is a sample or red is a test.";

            BVSoftware.Search.Lexicon l = new BVSoftware.Search.Lexicon(new Data.StaticLexicon());
            BVSoftware.Search.Searcher s = new BVSoftware.Search.Searcher(l, new Data.StaticSearcher());

            BVSoftware.Search.Indexers.SimpleTextIndexer indexer =
                new BVSoftware.Search.Indexers.SimpleTextIndexer(s);

            indexer.Index(0, "1234", 0, "", message);

            SearchObject actual = s.ObjectIndexFindByTypeAndId(0, 0, "1234");
            Assert.IsNotNull(actual);

            List<SearchObjectWord> words = s.ObjectWordIndexFindAll();
            Assert.IsNotNull(words);

            List<SearchObjectWord> expected = new List<SearchObjectWord>();
            expected.Add(new SearchObjectWord() { SearchObjectId = actual.Id, WordId = l.FindWordId("red"), Score = 2 });
            expected.Add(new SearchObjectWord() { SearchObjectId = actual.Id, WordId = l.FindWordId("sampl"), Score = 1 });
            expected.Add(new SearchObjectWord() { SearchObjectId = actual.Id, WordId = l.FindWordId("test"), Score = 1 });

            Assert.AreEqual(expected.Count, words.Count);
            Assert.AreEqual(expected[0].WordId, words[0].WordId);
            Assert.AreEqual(expected[1].WordId, words[1].WordId);
            Assert.AreEqual(expected[2].WordId, words[2].WordId);
            Assert.AreEqual(expected[0].Score, words[0].Score);
            Assert.AreEqual(expected[1].Score, words[1].Score);
            Assert.AreEqual(expected[2].Score, words[2].Score);

        }
    }
}
