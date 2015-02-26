using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.Common.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Introduce_To_Algorithm3Tests.Common.Utils
{
    [TestClass]
    public class BitConverterUtilsTests
    {
        [TestMethod]
        public void NetworkHostTest()
        {
            for (int i = 0; i < 1000000000; i++)
            {
                int result = BitConverterUtils.NetworkToHostOrder(BitConverterUtils.HostToNetworkOrder(i));

                if (result != i)
                {
                    Console.WriteLine(i);
                }
            }
        }

    }
}
