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
    public class BSTTests
    {
        [TestMethod()]
        public void MaxHeightTest()
        {
            BST<int, int> rbt = new BST<int, int>();
            for (int i = 0; i < 10; i++)
            {
                rbt.Insert(i + 1, i + 1);
            }

            int height = rbt.MaxHeight();
            height = rbt.MinHeight();
            Console.WriteLine(height);
        }
    }
}
