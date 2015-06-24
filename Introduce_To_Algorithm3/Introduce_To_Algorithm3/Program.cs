using System;
using System.IO.Ports;
using System.Threading;
using Introduce_To_Algorithm3.Common.Utils;

namespace Introduce_To_Algorithm3
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            while (true)
            {
                PcFunctions.MonitorOff();
                Thread.Sleep(30000);
                PcFunctions.MonitorOn();
                Thread.Sleep(30000);
            }

            //SerialPort serialPort = new SerialPort();

            //Console.WriteLine(serialPort.PortName);


            //int count = CodeCounter.GetCodeLines(@"C:\Users\cmlu\Documents\GitHub\Introduce_To_Algorithm3\Introduce_To_Algorithm3\Introduce_To_Algorithm3");
            //Console.WriteLine(count);
        }
    }
}
