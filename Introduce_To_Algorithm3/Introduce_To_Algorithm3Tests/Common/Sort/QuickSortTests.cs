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
    public class QuickSortTests
    {
        [TestMethod()]
        public void SortPromoteTest()
        {

        }

        [TestMethod()]
        public void SortTest()
        {
            int[] arr = {10,6,5,49,10,100,23,34,23,9,10,26,68,57,34,49,10,10};
            int m, n;
            QuickSort<int>.Partition3(arr, 0, arr.Length - 1,out m,out n);
            QuickSort<int>.Sort(arr);
        }
    }
}
