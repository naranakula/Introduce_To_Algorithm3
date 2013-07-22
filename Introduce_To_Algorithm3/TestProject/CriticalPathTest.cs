using Introduce_To_Algorithm3.Common.GraphEx;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TestProject
{
    
    
    /// <summary>
    ///This is a test class for CriticalPathTest and is intended
    ///to contain all CriticalPathTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CriticalPathTest
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
        ///A test for Cp
        ///</summary>
        [TestMethod()]
        public void CpTest()
        {
            List<Job> jobs = new List<Job>(); // TODO: Initialize to an appropriate value
            Job job0 = new Job(){JobID = 0,Duration = 5,Constaints = new List<Job>()};
            Job job1 = new Job(){JobID = 1,Duration = 10,Constaints = new List<Job>()};
            Job job2 = new Job(){JobID = 2,Duration = 20,Constaints = new List<Job>()};
            Job job3 = new Job(){JobID = 3,Duration = 30,Constaints = new List<Job>()};
            Job job4 = new Job(){JobID = 4,Duration = 40,Constaints = new List<Job>()};
            job4.Constaints.Add(job3);
            job4.Constaints.Add(job2);
            job4.Constaints.Add(job1);
            job3.Constaints.Add(job2);
            job3.Constaints.Add(job1);
            job2.Constaints.Add(job0);
            jobs.AddRange(new Job[]{job0,job1,job2,job3,job4});
            double expected = 95F; // TODO: Initialize to an appropriate value
            double actual;
            actual = CriticalPath.Cp(jobs);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
