using Introduce_To_Algorithm3.Common.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestProject
{
    
    
    /// <summary>
    ///This is a test class for FileUtilsTest and is intended
    ///to contain all FileUtilsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FileUtilsTest
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
        ///A test for Compress
        ///</summary>
        [TestMethod()]
        public void CompressTest()
        {
            string source = @"D:\123.txt"; // TODO: Initialize to an appropriate value
            string target = @"D:\v-chlu\123.txt.gz"; // TODO: Initialize to an appropriate value
            FileUtils.Compress(source, target);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Decompress
        ///</summary>
        [TestMethod()]
        public void DecompressTest()
        {
            string source = @"D:\v-chlu\123.txt.gz"; // TODO: Initialize to an appropriate value
            string target = @"D:\v-chlu\123.txt"; // TODO: Initialize to an appropriate value
            FileUtils.Decompress(source, target);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for IsUnicode
        ///</summary>
        [TestMethod()]
        public void IsUnicodeTest()
        {
            string filename = @"D:\v-chlu\123.txt"; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = FileUtils.IsUnicode(filename);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for IsFileContentSame
        ///</summary>
        [TestMethod()]
        public void IsFileContentSameTest()
        {
            string source = @"D:\v-chlu\123.txt"; // TODO: Initialize to an appropriate value
            string target = @"D:\123.txt"; // TODO: Initialize to an appropriate value
            //int bufferSize = 1024; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = FileUtils.IsFileContentSame(source, target);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for LineCount
        ///</summary>
        [TestMethod()]
        public void LineCountTest()
        {
            string filename = @"D:\v-chlu\123.txt"; // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            actual = FileUtils.LineCount(filename);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
