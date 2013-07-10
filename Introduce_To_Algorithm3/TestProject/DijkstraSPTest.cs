using Introduce_To_Algorithm3.Common.Graph;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TestProject
{
    
    
    /// <summary>
    ///This is a test class for DijkstraSPTest and is intended
    ///to contain all DijkstraSPTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DijkstraSPTest
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
        ///A test for DijkstraSP Constructor
        ///</summary>
        [TestMethod()]
        public void DijkstraSPConstructorTest()
        {
            EdgeWeightedDigraph g = null; // TODO: Initialize to an appropriate value
            int s = 0; // TODO: Initialize to an appropriate value
            DijkstraSP target = new DijkstraSP(g, s);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for Check
        ///</summary>
        [TestMethod()]
        public void CheckTest()
        {
            EdgeWeightedDigraph g = null; // TODO: Initialize to an appropriate value
            int s = 0; // TODO: Initialize to an appropriate value
            DijkstraSP target = new DijkstraSP(g, s); // TODO: Initialize to an appropriate value
            EdgeWeightedDigraph g1 = null; // TODO: Initialize to an appropriate value
            int s1 = 0; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Check(g1, s1);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for DistTo
        ///</summary>
        [TestMethod()]
        public void DistToTest()
        {
            EdgeWeightedDigraph g = null; // TODO: Initialize to an appropriate value
            int s = 0; // TODO: Initialize to an appropriate value
            DijkstraSP target = new DijkstraSP(g, s); // TODO: Initialize to an appropriate value
            int v = 0; // TODO: Initialize to an appropriate value
            double expected = 0F; // TODO: Initialize to an appropriate value
            double actual;
            actual = target.DistTo(v);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for HasPathTo
        ///</summary>
        [TestMethod()]
        public void HasPathToTest()
        {
            EdgeWeightedDigraph g = null; // TODO: Initialize to an appropriate value
            int s = 0; // TODO: Initialize to an appropriate value
            DijkstraSP target = new DijkstraSP(g, s); // TODO: Initialize to an appropriate value
            int v = 0; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.HasPathTo(v);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for PathTo
        ///</summary>
        [TestMethod()]
        public void PathToTest()
        {
            EdgeWeightedDigraph g = null; // TODO: Initialize to an appropriate value
            int s = 0; // TODO: Initialize to an appropriate value
            DijkstraSP target = new DijkstraSP(g, s); // TODO: Initialize to an appropriate value
            int v = 0; // TODO: Initialize to an appropriate value
            IEnumerable<DirectedEdge> expected = null; // TODO: Initialize to an appropriate value
            IEnumerable<DirectedEdge> actual;
            actual = target.PathTo(v);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for relax
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Introduce_To_Algorithm3.exe")]
        public void relaxTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            DijkstraSP_Accessor target = new DijkstraSP_Accessor(param0); // TODO: Initialize to an appropriate value
            DirectedEdge e = null; // TODO: Initialize to an appropriate value
            target.relax(e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}
