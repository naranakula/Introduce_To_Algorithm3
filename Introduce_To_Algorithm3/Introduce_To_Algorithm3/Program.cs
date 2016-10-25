
using System;

namespace Introduce_To_Algorithm3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            byte[] buffer = new byte[8192*16*1024*2];
            Console.ReadLine();
            Console.WriteLine(buffer.Length);
            for (int i = 0; i < buffer.Length; i++)
            {
                byte b = buffer[i];
            }
        }
    }
}
