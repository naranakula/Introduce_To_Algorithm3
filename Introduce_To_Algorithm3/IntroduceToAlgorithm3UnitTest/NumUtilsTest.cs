using System;
using System.Text;
using System.Collections.Generic;
using Introduce_To_Algorithm3.Common.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntroduceToAlgorithm3UnitTest
{
    /// <summary>
    /// Summary description for NumUtilsTest
    /// </summary>
    [TestClass]
    public class NumUtilsTest
    {
        public NumUtilsTest()
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
        public void IsPrime()
        {
            //
            // TODO: Add test logic here
            //
            int num = 0;
            bool expected = true;
            bool result = false;
            result=NumUtils.IsPrime(num);

            Assert.AreEqual(expected, result);
        }
    }
}
