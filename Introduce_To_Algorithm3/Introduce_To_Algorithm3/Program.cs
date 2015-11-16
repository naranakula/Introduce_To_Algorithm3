using System;
using Introduce_To_Algorithm3.Common.Utils;

namespace Introduce_To_Algorithm3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int count = CodeCounter.GetCodeLines(@"E:\Github\Introduce_To_Algorithm3\Introduce_To_Algorithm3\Introduce_To_Algorithm3");
            Console.WriteLine(count);
        }
    }
}
