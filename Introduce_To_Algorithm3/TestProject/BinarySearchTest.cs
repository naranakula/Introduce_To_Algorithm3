using System;
using Introduce_To_Algorithm3.Common.Search;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
    /// <summary>
    ///     This is a test class for BinarySearchTest and is intended
    ///     to contain all BinarySearchTest Unit Tests
    /// </summary>
    [TestClass]
    public class BinarySearchTest
    {
        /// <summary>
        ///     Gets or sets the test context which provides
        ///     information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

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
        ///     A test for Search
        /// </summary>
        public void SearchTestHelper<T>()
            where T : IComparable<T>
        {
            T[] arr = null; // TODO: Initialize to an appropriate value
            T key = default(T); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            actual = BinarySearch<T>.Search(arr, key);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod]
        public void SearchTest()
        {
            Assert.Inconclusive("No appropriate type parameter is found to satisfies the type constraint(s) of T. " +
                                "Please call SearchTestHelper<T>() with appropriate type parameters.");
        }
    }
}