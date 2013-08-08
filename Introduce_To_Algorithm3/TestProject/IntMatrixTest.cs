using Introduce_To_Algorithm3.Common.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestProject
{
    
    
    /// <summary>
    ///This is a test class for IntMatrixTest and is intended
    ///to contain all IntMatrixTest Unit Tests
    ///</summary>
    [TestClass()]
    public class IntMatrixTest
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
        ///A test for IntMatrix Constructor
        ///</summary>
        [TestMethod()]
        public void IntMatrixConstructorTest()
        {
            int row = 2; // TODO: Initialize to an appropriate value
            int column = 4; // TODO: Initialize to an appropriate value
            IntMatrix target = new IntMatrix(row, column);
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    target.Set(i,j,i*j+i);
                }
            }
            Matrix<int> transpose = target.Transpose();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }
    }
}
