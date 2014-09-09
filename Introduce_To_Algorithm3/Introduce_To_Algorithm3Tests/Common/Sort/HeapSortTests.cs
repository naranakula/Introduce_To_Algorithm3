using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.Common.Sort;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Introduce_To_Algorithm3.Common.Sort.Tests
{
    [TestClass()]
    public class HeapSortTests
    {
        [TestMethod()]
        public void SortTest()
        {
            var a = HeapSort<int>.Sort(1, 2, 3, 4, 5, 6, 10, 9, 8);
            a.ToList().ForEach(Console.WriteLine);
        }
    }
}
