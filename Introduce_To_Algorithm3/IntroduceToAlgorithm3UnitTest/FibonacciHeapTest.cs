using Introduce_To_Algorithm3.Common.AdvancedStructs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace IntroduceToAlgorithm3UnitTest
{
    
    
    /// <summary>
    ///This is a test class for FibonacciHeapTest and is intended
    ///to contain all FibonacciHeapTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FibonacciHeapTest
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

        [TestMethod()]
        public void InsertTest()
        {
            FibonacciHeap<int,int> heap = new FibonacciHeap<int, int>();
            Random r = new Random();
            for (int i = 0; i < 6; i++)
            {
                int key = r.Next(100);
                heap.Insert(key,key);
            }
            Console.WriteLine(heap);
        }


        [TestMethod()]
        public void UnionTest()
        {
            FibonacciHeap<int, int> heap1 = new FibonacciHeap<int, int>();
            FibonacciHeap<int, int> heap2 = new FibonacciHeap<int, int>();
            heap1.Insert(1,1);
            heap1.Insert(3,3);
            heap2.Insert(2,2);
            heap2.Insert(0,0);
            heap2.Insert(4,4);
            var f = FibonacciHeap<int, int>.Union(heap1, heap2);
            Console.WriteLine(f);
        }
    }
}
