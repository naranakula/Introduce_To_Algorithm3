﻿using System;
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
            string logFile = @"D:\v-chlu\123.txt";
            Logger logger = Logger.GetInstance(logFile, true, true);
            logger.WriteLine(CodeCounter.GetCodeLines(@""));
            logger.Close();
        }
    }
}
