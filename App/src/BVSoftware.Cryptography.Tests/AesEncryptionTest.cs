using BVSoftware.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace BVSoftware.Cryptography.Tests
{
    
    
    /// <summary>
    ///This is a test class for AesEncryptionTest and is intended
    ///to contain all AesEncryptionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AesEncryptionTest
    {


        private TestContext testContextInstance;

        public const string StaticKey = "5C-0D-67-8A-47-9E-B4-A3-AD-12-4B-EB-B7-37-73-B3-98-DC-72-AF-25-93-04-1B-3D-85-2E-29-23-92-8D-E1";

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

        private void EncodeDecode(string message)
        {
            EncodeDecode(message, KeyGenerator.Generate256bitKey());
        }

        private void EncodeDecode(string message, string key)
        {                        
            string encoded = AesEncryption.Encode(message, key);
            Assert.IsNotNull(encoded);
            Assert.AreNotEqual("", encoded);
            string decoded = AesEncryption.Decode(encoded, key);
            Assert.AreEqual(message, decoded);
        }


        [TestMethod()]
        public void EncodeDecode1()
        {
            EncodeDecode("This is my message.", StaticKey);            
        }

        [TestMethod()]
        public void EncodeDecodeBlank()
        {
            EncodeDecode("", StaticKey);  
        }

        [TestMethod()]
        public void EncodeDecodeSingleCharacter()
        {
            EncodeDecode("A", StaticKey);  
        }

        [TestMethod()]
        public void EncodeDecodeHighAscii()
        {
            EncodeDecode("הּ", StaticKey);  
        }

        [TestMethod()]
        public void EncodeDecodeWithRandomKey()
        {
            EncodeDecode("This is my message for a random key");
        }

        [TestMethod()]
        public void EncodeDecodeWithBlankKey()
        {

            try
            {
                EncodeDecode("4111111111111111", string.Empty);
                Assert.Fail();
            }
            catch (System.Exception ex)
            {
                Assert.IsNotNull(ex, "Should have thrown an exception!");
            }                                    
        }
    }
}
