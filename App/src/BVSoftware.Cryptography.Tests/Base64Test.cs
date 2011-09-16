using BVSoftware.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace BVSoftware.Cryptography.Tests
{
    
    
    /// <summary>
    ///This is a test class for Base64Test and is intended
    ///to contain all Base64Test Unit Tests
    ///</summary>
    [TestClass()]
    public class Base64Test
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
        ///A test for ConvertFromBase64
        ///</summary>
        [TestMethod()]
        public void AutoTests()
        {
            TestBase64("This is my test string to encode !@#$%^", "VGhpcyBpcyBteSB0ZXN0IHN0cmluZyB0byBlbmNvZGUgIUAjJCVe");
            TestBase64("", "");
            TestBase64("test", "dGVzdA==");
            TestBase64("!", "IQ==");
            TestBase64(" ", "IA==");
            TestBase64("E", "RQ==");
            TestBase64("e", "ZQ==");            
        }

        [TestMethod()]
        public void HighAsciiTest()
        {
            TestBase64("הּ", "76y0");
        }

        [TestMethod()]
        public void SimpleTest()
        {
            TestBase64("AB 1", "QUIgMQ==");
        }
        
        private void TestBase64(string plaintext, string expectedEncoding)
        {
            string encoded = Base64.ConvertToBase64(Conversion.StringToBytes(plaintext));
            Assert.AreEqual(expectedEncoding, encoded, "Encoding did match expected value");

            byte[] decodedBytes = Base64.ConvertFromBase64(expectedEncoding);
            byte[] expectedDecodedBytes = Conversion.StringToBytes(plaintext);
            Assert.IsNotNull(decodedBytes);
            for (int i = 0; i < decodedBytes.Length; i++)
            {
                Assert.AreEqual(expectedDecodedBytes[i], decodedBytes[i],"a decoded byte didn't match");
            }     
        }
    }
}
