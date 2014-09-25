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
    public class SortedSkipLinkedListTests
    {
        [TestMethod()]
        public void SearchTest()
        {
            SortedSkipLinkedList<int,int> list = new SortedSkipLinkedList<int, int>();

            int[] array = new int[5]{8,7,4,9,10};

            array.ToList().ForEach(i=>list.Insert(i,i));


            list.Delete(7);
            list.Delete(9);
            list.Delete(9);
            list.Delete(4);
            list.Delete(10);
            list.Delete(8);
        }

    }
}
