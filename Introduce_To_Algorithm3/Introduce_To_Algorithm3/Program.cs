using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Introduce_To_Algorithm3.Common.GraphEx;
using Introduce_To_Algorithm3.Common.Utils;

namespace Introduce_To_Algorithm3
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger logger = Logger.GetInstance(@"D:\v-chlu\log.txt", true, true);
            logger.Write("Hello");
            logger.WriteLine("Hello");
            logger.WriteLine("world");
            logger.Close();
        }
    }
}
