using BVSoftware.Search;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace BVSoftware.Search.Tests
{


    /// <summary>
    ///This is a test class for QueryParserTest and is intended
    ///to contain all QueryParserTest Unit Tests
    ///</summary>
    [TestClass()]
    public class QueryParserTest
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


        private void TestPhrase(string raw, string expected)
        {
            string actual = TextParser_Accessor.ReplaceNonAlphaNumeric(raw);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [DeploymentItem("BVSoftware.Search.dll")]
        public void ReplaceNonAlphaNumericTest()
        {
            TestPhrase("\"This is a test.\"",
                       " this is a test  ");
            TestPhrase("This is a test.",
                       "this is a test ");
            TestPhrase("This is a Façade.",
                       "this is a façade ");
            TestPhrase("This \"is a\" TeSt_oNe.",
                       "this  is a  test_one ");
        }

        private void AssertListsAreEqual(List<string> expected, List<string> actual)
        {
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i], actual[i], "Word didn't match: " + actual[i] + " expected " + expected[i]);
            }
        }

        [TestMethod()]
        public void CanParseQueries()
        {
            string input = "\"Red is a test.\"";

            List<string> expected = new List<string>();
            expected.Add("red");
            expected.Add("test");

            List<string> actual = TextParser.ParseText(input);

            AssertListsAreEqual(expected, actual);
        }

        [TestMethod()]
        public void CanParseQueriesWithoutStopWords()
        {
            string input = "\"Red  test.\"";

            List<string> expected = new List<string>();
            expected.Add("red");
            expected.Add("test");

            List<string> actual = TextParser.ParseText(input);

            AssertListsAreEqual(expected, actual);
        }

        [TestMethod()]
        public void CanParseQueriesWithUnderscores()
        {
            string input = "Red \"is a\" TeSt_oNe.";

            List<string> expected = new List<string>();
            expected.Add("red");
            expected.Add("test_on");

            List<string> actual = TextParser.ParseText(input);

            AssertListsAreEqual(expected, actual);
        }


    }
}
