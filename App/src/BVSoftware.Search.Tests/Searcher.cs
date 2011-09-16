using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BVSoftware.Search.Tests
{
    /// <summary>
    /// Summary description for SearchObjectIndex
    /// </summary>
    [TestClass]
    public class Searcher
    {
        public Searcher()
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
        public void CanAddObject()
        {
            BVSoftware.Search.SearchObject s = new BVSoftware.Search.SearchObject();
            s.ObjectType = 0;
            s.ObjectId = "1234";

            BVSoftware.Search.Lexicon lexicon = new BVSoftware.Search.Lexicon(new BVSoftware.Search.Data.StaticLexicon());
            BVSoftware.Search.Searcher searcher = new BVSoftware.Search.Searcher(lexicon, new BVSoftware.Search.Data.StaticSearcher());

            long id = searcher.ObjectIndexAddOrUpdate(s);

            Assert.IsTrue(id > 0);

        }

        [TestMethod]
        public void CanFindObjectById()
        {
            BVSoftware.Search.SearchObject s = new BVSoftware.Search.SearchObject();
            s.ObjectType = 0;
            s.ObjectId = "1234";

            BVSoftware.Search.Lexicon lexicon = new BVSoftware.Search.Lexicon(new BVSoftware.Search.Data.StaticLexicon());
            BVSoftware.Search.Searcher searcher = new BVSoftware.Search.Searcher(lexicon, new BVSoftware.Search.Data.StaticSearcher());

            long id = searcher.ObjectIndexAddOrUpdate(s);

            Assert.IsTrue(id > 0);

            BVSoftware.Search.SearchObject actual = searcher.ObjectIndexFind(id);
            Assert.IsNotNull(actual);
            Assert.AreEqual(id, actual.Id);
            Assert.AreEqual(s.ObjectType, actual.ObjectType);
            Assert.AreEqual(s.ObjectId, actual.ObjectId);
        }

        [TestMethod]
        public void ReturnsNullWhenCantFindObject()
        {
            BVSoftware.Search.Lexicon lexicon = new BVSoftware.Search.Lexicon(new BVSoftware.Search.Data.StaticLexicon());
            BVSoftware.Search.Searcher searcher = new BVSoftware.Search.Searcher(lexicon, new BVSoftware.Search.Data.StaticSearcher());
            BVSoftware.Search.SearchObject actual = searcher.ObjectIndexFind(99);
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void DoesntAddObjectsToIndexTwice()
        {
            BVSoftware.Search.SearchObject s = new BVSoftware.Search.SearchObject();
            s.ObjectType = 0;
            s.ObjectId = "1234";

            BVSoftware.Search.Lexicon lexicon = new BVSoftware.Search.Lexicon(new BVSoftware.Search.Data.StaticLexicon());
            BVSoftware.Search.Searcher searcher = new BVSoftware.Search.Searcher(lexicon, new BVSoftware.Search.Data.StaticSearcher());

            long id = searcher.ObjectIndexAddOrUpdate(s);

            Assert.IsTrue(id > 0);

            long secondId = searcher.ObjectIndexAddOrUpdate(s);

            Assert.AreEqual(id, secondId);
        }

        [TestMethod]
        public void CanDeleteFromIndex()
        {
            BVSoftware.Search.SearchObject s = new BVSoftware.Search.SearchObject();
            s.ObjectType = 0;
            s.ObjectId = "1234";

            BVSoftware.Search.Lexicon lexicon = new BVSoftware.Search.Lexicon(new BVSoftware.Search.Data.StaticLexicon());
            BVSoftware.Search.Searcher searcher = new BVSoftware.Search.Searcher(lexicon, new BVSoftware.Search.Data.StaticSearcher());

            long id = searcher.ObjectIndexAddOrUpdate(s);

            Assert.IsTrue(id > 0);

            BVSoftware.Search.SearchObject actual = searcher.ObjectIndexFind(id);
            Assert.IsNotNull(actual);
            Assert.AreEqual(id, actual.Id);
            Assert.AreEqual(s.ObjectType, actual.ObjectType);
            Assert.AreEqual(s.ObjectId, actual.ObjectId);

            bool actual2 = searcher.ObjectIndexDelete(id);
            SearchObject existingAfterDelete = searcher.ObjectIndexFind(id);
            Assert.IsNull(existingAfterDelete);
        }

        [TestMethod]
        public void CanRecordObjectWord()
        {
            // Setup 
            BVSoftware.Search.SearchObject s = new BVSoftware.Search.SearchObject();
            s.ObjectType = 0;
            s.ObjectId = "1234";

            BVSoftware.Search.Lexicon lexicon = new BVSoftware.Search.Lexicon(new BVSoftware.Search.Data.StaticLexicon());
            long wordId = lexicon.AddOrCreateWord("thi");
            Assert.IsTrue(wordId > 0);

            BVSoftware.Search.Searcher searcher = new BVSoftware.Search.Searcher(lexicon, new BVSoftware.Search.Data.StaticSearcher());
            long id = searcher.ObjectIndexAddOrUpdate(s);
            Assert.IsTrue(id > 0);

            // Test
            BVSoftware.Search.SearchObjectWord w = new BVSoftware.Search.SearchObjectWord();
            w.SearchObjectId = id;
            w.WordId = wordId;
            w.Score = 1;

            bool actual = searcher.ObjectWordIndexUpdate(w);
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void CanFindObjectsForWord()
        {
            long siteId = 0;

            // Setup Words
            BVSoftware.Search.Lexicon lexicon = new BVSoftware.Search.Lexicon(new BVSoftware.Search.Data.StaticLexicon());
            long thi_id = lexicon.AddOrCreateWord("thi");
            long test_id = lexicon.AddOrCreateWord("test");
            long sampl_id = lexicon.AddOrCreateWord("sampl");

            // Setup Search Objects
            BVSoftware.Search.SearchObject s1 = new BVSoftware.Search.SearchObject();
            s1.ObjectType = 0;
            s1.ObjectId = "1";
            BVSoftware.Search.SearchObject s2 = new BVSoftware.Search.SearchObject();
            s2.ObjectType = 0;
            s2.ObjectId = "2";

            // Setup Searcher
            BVSoftware.Search.Searcher searcher = new BVSoftware.Search.Searcher(lexicon, new BVSoftware.Search.Data.StaticSearcher());

            // Record Objects
            long sid1 = searcher.ObjectIndexAddOrUpdate(s1);
            long sid2 = searcher.ObjectIndexAddOrUpdate(s2);

            // Index Words for Objects
            BVSoftware.Search.SearchObjectWord w1_1 = new BVSoftware.Search.SearchObjectWord() { SearchObjectId = s1.Id, WordId = thi_id, Score = 1, SiteId = siteId };
            BVSoftware.Search.SearchObjectWord w1_2 = new BVSoftware.Search.SearchObjectWord() { SearchObjectId = s1.Id, WordId = test_id, Score = 1, SiteId = siteId };
            BVSoftware.Search.SearchObjectWord w2_1 = new BVSoftware.Search.SearchObjectWord() { SearchObjectId = s2.Id, WordId = thi_id, Score = 1, SiteId = siteId };
            BVSoftware.Search.SearchObjectWord w2_2 = new BVSoftware.Search.SearchObjectWord() { SearchObjectId = s2.Id, WordId = sampl_id, Score = 1, SiteId = siteId };
            searcher.ObjectWordIndexUpdate(w1_1);
            searcher.ObjectWordIndexUpdate(w1_2);
            searcher.ObjectWordIndexUpdate(w2_1);
            searcher.ObjectWordIndexUpdate(w2_2);

            // Test
            List<SearchObjectWord> actual = searcher.ObjectWordIndexFindByWordId(siteId, thi_id);
            Assert.IsNotNull(actual);
            Assert.AreEqual(2, actual.Count);

            List<SearchObjectWord> actual2 = searcher.ObjectWordIndexFindByWordId(siteId, test_id);
            Assert.IsNotNull(actual2);
            Assert.AreEqual(1, actual2.Count);

            List<SearchObjectWord> actual3 = searcher.ObjectWordIndexFindByWordId(siteId, 99);
            Assert.IsNotNull(actual3);
            Assert.AreEqual(0, actual3.Count);
        }

        [TestMethod]
        public void CanDoOneWordSearch()
        {
            long siteId = 0;

            // Setup Objects
            BVSoftware.Search.Lexicon l = new BVSoftware.Search.Lexicon(new Data.StaticLexicon());
            BVSoftware.Search.Searcher s = new BVSoftware.Search.Searcher(l, new Data.StaticSearcher());
            BVSoftware.Search.Indexers.SimpleTextIndexer indexer = new BVSoftware.Search.Indexers.SimpleTextIndexer(s);

            indexer.Index(siteId, "1", 0, "Document Red", "Red is the first document. Red is a test.");
            indexer.Index(siteId, "2", 0, "Document Blue", "Blue is the second document not Red like the first. Blue document is a sample");
            indexer.Index(siteId, "3", 0, "Shakespeare: Julius Ceasar", "Et tu brute?");
            indexer.Index(siteId, "4", 0, "Shakespeare: To Be or Not To Be", "To be or not to be");
            indexer.Index(siteId, "5", 0, "Doc Brown", "The Flux Capacitor is what enables time travel. I fell off my toilet and hit my head. When I woke up, I drew this...");

            string query = "document";
            
            int total = 0;
            List<SearchObject> results = s.DoSearch(query, 1, 10, ref total);
            Assert.IsNotNull(results);
            Assert.AreEqual(2, results.Count);
        }

        [TestMethod]
        public void CanDoTwoWordSearch()
        {
            long siteId = 0;
            // Setup Objects
            BVSoftware.Search.Lexicon l = new BVSoftware.Search.Lexicon(new Data.StaticLexicon());
            BVSoftware.Search.Searcher s = new BVSoftware.Search.Searcher(l, new Data.StaticSearcher());
            BVSoftware.Search.Indexers.SimpleTextIndexer indexer = new BVSoftware.Search.Indexers.SimpleTextIndexer(s);

            indexer.Index(siteId, "1", 0, "Document Red", "Red is the first document. Red is a test.");
            indexer.Index(siteId, "2", 0, "Document Blue", "Blue is the second document not Red like the first. Blue Blue document is a sample");
            indexer.Index(siteId, "3", 0, "Shakespeare: Julius Ceasar", "Et tu brute?");
            indexer.Index(siteId, "4", 0, "Shakespeare: To Be or Not To Be", "To be or not to be Red Red Red Red Red Red Red");
            indexer.Index(siteId, "5", 0, "Doc Brown", "The Flux Capacitor is what enables time travel. I fell off my toilet and hit my head. When I woke up, I drew this...");

            string query = "document blue";
            int total = 0;
            List<SearchObject> results = s.DoSearch(query, 1, 10, ref total);
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void CanScoreDocumentsSearch()
        {
            long siteId = 0;
            // Setup Objects
            BVSoftware.Search.Lexicon l = new BVSoftware.Search.Lexicon(new Data.StaticLexicon());
            BVSoftware.Search.Searcher s = new BVSoftware.Search.Searcher(l, new Data.StaticSearcher());
            BVSoftware.Search.Indexers.SimpleTextIndexer indexer = new BVSoftware.Search.Indexers.SimpleTextIndexer(s);

            indexer.Index(siteId, "1", 0, "Document Red", "Red is the first document. Red is a test.");
            indexer.Index(siteId, "2", 0, "Document Blue", "Blue is the second document not Red like the first. Blue Blue document is a sample");
            indexer.Index(siteId, "3", 0, "Shakespeare: Julius Ceasar", "Et tu brute?");
            indexer.Index(siteId, "4", 0, "Shakespeare: To Be or Not To Be", "To be or not to be document Red Red Red Red Red Red Red");
            indexer.Index(siteId, "5", 0, "Doc Brown", "The Flux Capacitor is what enables time travel. I fell off my toilet and hit my head. When I woke up, I drew this...");

            string query = "document red";
            int total = 0;
            List<SearchObject> results = s.DoSearch(query, 1, 10, ref total);
            Assert.IsNotNull(results);
            Assert.AreEqual(3, results.Count);
            Assert.AreEqual(4, results[0].Id, "First Document should be #4");
            Assert.AreEqual(1, results[1].Id, "Second Document should be #1");
            Assert.AreEqual(2, results[2].Id, "Last Document should be #2");
        }

        [TestMethod]
        public void CanSearchSeparateSites()
        {
            long siteId = 99;
            long siteId2 = 1;

            // Setup Objects
            BVSoftware.Search.Lexicon l = new BVSoftware.Search.Lexicon(new Data.StaticLexicon());
            BVSoftware.Search.Searcher s = new BVSoftware.Search.Searcher(l, new Data.StaticSearcher());
            BVSoftware.Search.Indexers.SimpleTextIndexer indexer = new BVSoftware.Search.Indexers.SimpleTextIndexer(s);

            indexer.Index(siteId, "1", 0, "Document Red", "Red is the first document. Red is a test.");
            indexer.Index(siteId, "2", 0, "Document Blue", "Blue is the second document not Red like the first. Blue document is a sample");
            indexer.Index(siteId, "3", 0, "Shakespeare: Julius Ceasar", "Et tu brute?");
            indexer.Index(siteId2, "4", 0, "Shakespeare: To Be or Not To Be", "To be or not to be");
            indexer.Index(siteId2, "5", 0, "Doc Brown", "The Flux Capacitor is what enables time travel. I fell off my toilet and hit my head. When I woke up, I drew this...");

            string query = "Shakespeare";
            int total = 0;
            List<SearchObject> results = s.DoSearch(siteId, query, 1, 10, ref total);
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("3", results[0].ObjectId);

            List<SearchObject> results2 = s.DoSearch(siteId2, query, 1, 10, ref total);
            Assert.IsNotNull(results2);
            Assert.AreEqual(1, results2.Count);
            Assert.AreEqual("4", results2[0].ObjectId);
        }
    }
}
