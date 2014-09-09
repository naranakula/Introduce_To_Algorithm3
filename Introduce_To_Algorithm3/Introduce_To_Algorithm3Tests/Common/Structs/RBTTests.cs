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
    public class RBTTests
    {
        [TestMethod()]
        public void HeightTest()
        {
            RBT<int,int> rbt = new RBT<int, int>();
            for (int i = 0; i < 10; i++)
            {
                rbt.Insert(i+1,i+1);
                rbt.Delete(rbt.Search_(i+1));
            }


            int height = rbt.MaxHeight();
            height = rbt.MinHeight();
            height = rbt.BlackHeight();
            Console.WriteLine(height);
        }

        [TestMethod()]
        public void MinHeightTest()
        {

        }

        [TestMethod()]
        public void BlackHeightTest()
        {

        }
    }
}
