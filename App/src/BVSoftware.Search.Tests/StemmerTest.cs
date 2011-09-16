using BVSoftware.Search;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace BVSoftware.Search.Tests
{


    /// <summary>
    ///This is a test class for StemmerTest and is intended
    ///to contain all StemmerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class StemmerTest
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
        ///A test for stem
        ///</summary>
        [TestMethod()]
        public void stemTest()
        {
            TestWord("samples", "sampl");
            TestWord("sampl", "sampl");
            TestWord("sample", "sampl");
            TestWord("sampling", "sampl");
            TestWord("Façade", "façad");
        }

        private void TestWord(string raw, string expected)
        {
            string actual = Stemmer.StemSingleWord(raw);
            Assert.AreEqual(expected, actual);
        }

        private void StopWordTest(string raw, bool expected)
        {
            string stemmed = Stemmer.StemSingleWord(raw);
            bool actual = TextParser.IsStopWord(stemmed);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsStopWord
        ///</summary>
        [TestMethod()]
        public void IsStopWordTest()
        {
            StopWordTest("it", true);
            StopWordTest("monkey", false);
            StopWordTest("this", true);
            StopWordTest("one", true);
            StopWordTest("two", false);
            StopWordTest("and", true);
        }
    }
}
