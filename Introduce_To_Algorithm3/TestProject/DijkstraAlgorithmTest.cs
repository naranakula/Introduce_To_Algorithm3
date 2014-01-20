using Introduce_To_Algorithm3.Common.GraphEx;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestProject
{
    /// <summary>
    ///This is a test class for DijkstraAlgorithmTest and is intended
    ///to contain all DijkstraAlgorithmTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DijkstraAlgorithmTest
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
        ///A test for DijkstraAlgorithm Constructor
        ///</summary>
        [TestMethod()]
        public void DijkstraAlgorithmConstructorTest()
        {
            EdgeWeightedDigraph g = new EdgeWeightedDigraph(5); // TODO: Initialize to an appropriate value
            g.AddEdge(new DirectedEdge(0, 1, 1));
            g.AddEdge(new DirectedEdge(0, 3, 3));
            g.AddEdge(new DirectedEdge(1, 2, 5));
            g.AddEdge(new DirectedEdge(1, 4, 2));
            g.AddEdge(new DirectedEdge(3, 2, 2));
            g.AddEdge(new DirectedEdge(3, 4, 1));
            int source = 0; // TODO: Initialize to an appropriate value
            DijkstraAlgorithm target = new DijkstraAlgorithm(g, source);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }
    }
}