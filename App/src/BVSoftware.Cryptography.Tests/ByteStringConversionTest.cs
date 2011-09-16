using BVSoftware.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace BVSoftware.Cryptography.Tests
{
    
    
    /// <summary>
    ///This is a test class for ByteStringConversionTest and is intended
    ///to contain all ByteStringConversionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ByteStringConversionTest
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

        private void ConvertAndBack(string input)
        {
            byte[] converted = Conversion.StringToBytes(input);
            string decoded = Conversion.BytesToString(converted);
            Assert.AreEqual(input, decoded);
        }

        [TestMethod()]
        public void ConvertToByteArrayTest()
        {
            string input = "Ǝ"; //&#398
            ConvertAndBack(input);            
        }
       
        [TestMethod()]
        public void ConvertToByteArrayTest2()
        {
            ConvertAndBack("AB 1");                        
        }

        [TestMethod]
        public void BunchOfInput()
        {
            ConvertAndBack("");
            ConvertAndBack("!@#$%^&*()_+=-<>.,/?';:\"{[]}\\|");
            ConvertAndBack(" ");
            ConvertAndBack("My name is test guy!");
            ConvertAndBack("The quick brown fox jumped over the lazy yellow dog.");
            ConvertAndBack("1234567890`~");
        }

    }
}
