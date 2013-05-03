using Introduce_To_Algorithm3.Common.Sort;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestProject
{
    
    
    /// <summary>
    ///这是 HeapSortTest 的测试类，旨在
    ///包含所有 HeapSortTest 单元测试
    ///</summary>
    [TestClass()]
    public class HeapSortTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，上下文提供
        ///有关当前测试运行及其功能的信息。
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

        #region 附加测试特性
        // 
        //编写测试时，还可使用以下特性:
        //
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //使用 ClassCleanup 在运行完类中的所有测试后再运行代码
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //使用 TestInitialize 在运行每个测试前先运行代码
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //使用 TestCleanup 在运行完每个测试后运行代码
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///Sort 的测试
        ///</summary>
        public void SortTestHelper<T>()
            where T : IComparable<T>
        {
            int [] arr = {1,-1,2,3,-8}; // TODO: 初始化为适当的值
            int[] expected = null; // TODO: 初始化为适当的值
            int[] actual;
            actual = HeapSort<int>.Sort(arr);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void SortTest()
        {
            int[] arr = { 1, -1, 2, 3, -8 }; // TODO: 初始化为适当的值
            int[] expected = null; // TODO: 初始化为适当的值
            int[] actual;
            actual = HeapSort<int>.Sort(arr);
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 SortTestHelper<T>()。");
        }
    }
}
