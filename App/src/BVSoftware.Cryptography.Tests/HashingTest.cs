using BVSoftware.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BVSoftware.Cryptography.Tests
{
    
    
    /// <summary>
    ///This is a test class for HashingTest and is intended
    ///to contain all HashingTest Unit Tests
    ///</summary>
    [TestClass()]
    public class HashingTest
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
        ///A test for Md5HashToBytes
        ///</summary>
        [TestMethod()]
        public void Md5HashToBytesTest()
        {
            string message = "test";
            byte[] expected = {0x09, 0x8f, 0x6b, 0xcd, 0x46, 0x21, 0xd3, 0x73, 0xca, 0xde, 0x4e, 0x83, 0x26, 0x27, 0xb4, 0xf6};
            byte[] actual;
            actual = Hashing.Md5HashToBytes(message);            
            Assert.AreEqual(BitConverter.ToString(expected), BitConverter.ToString(actual));            
        }
        
        [TestMethod()]
        public void Md5Hash()
        {            
            string message = "test";
            string expected = Base64.ConvertToBase64(new byte[] { 0x09, 0x8f, 0x6b, 0xcd, 0x46, 0x21, 0xd3, 0x73, 0xca, 0xde, 0x4e, 0x83, 0x26, 0x27, 0xb4, 0xf6 });
            string actual;
            actual = Hashing.Md5Hash(message);
            Assert.AreEqual(expected, actual);
        }

    }
}
