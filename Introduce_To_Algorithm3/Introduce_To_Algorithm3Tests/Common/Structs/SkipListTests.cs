using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.Common.Structs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Introduce_To_Algorithm3.Common.Structs.Tests
{
    [TestClass()]
    public class SkipListTests
    {
        [TestMethod()]
        public void InsertTest()
        {
            SkipList<int,int> skipList = new SkipList<int, int>();

            for (int i = 0; i < 100; i++)
            {
                skipList.Insert(i,i);
            }

            for (int i = 0; i < 100; i++)
            {
                skipList.Delete(i);
            }
        }
    }
}
