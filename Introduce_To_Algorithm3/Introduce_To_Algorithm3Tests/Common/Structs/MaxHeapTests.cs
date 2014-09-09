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
    public class MaxHeapTests
    {
        [TestMethod()]
        public void IsMaxHeapTest()
        {
            int[] arr = {23, 17, 14, 6, 13, 10, 1, 5, 7, 12};
            bool result = MaxHeap<int>.IsMaxHeap(arr, arr.Length);
            Console.WriteLine(result);
        }
    }
}
