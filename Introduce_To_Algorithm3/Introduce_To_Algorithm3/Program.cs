using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using Introduce_To_Algorithm3.Common.GraphEx;
using Introduce_To_Algorithm3.Common.Utils;
using Introduce_To_Algorithm3.Common.Structs;
using Introduce_To_Algorithm3.Common.Utils.sockets;
using System.Net.Sockets;
using System.Threading;

namespace Introduce_To_Algorithm3
{
    class Program
    {
        static void Main(string[] args)
        {
            int linenum = CodeCounter.GetCodeLines(@"C:\Users\v-chlu\Documents\GitHub\Introduce_To_Algorithm3\Introduce_To_Algorithm3\Introduce_To_Algorithm3");
            SBT<int, int> sbt = new SBT<int, int>();

            RBT<int, int> avl = new RBT<int, int>();
            Random rand = new Random(1);
            List<int> list = new List<int>();
            DateTime dateTime = DateTime.Now;
            for (int i = 0; i < 5; i++)
            {
                list.Add(rand.Next(1999900000));
            }

            Console.WriteLine(DateTime.Now - dateTime);
            dateTime = DateTime.Now;
           // list = list.Distinct().ToList();
            
            foreach (var item in list)
            {
                avl.Insert(item, item);
            }
            Console.WriteLine(DateTime.Now - dateTime);
            dateTime = DateTime.Now;
            foreach (var item in list)
            {
                var node = avl.Search_(item);
                avl.Delete(node);
            }
            Console.WriteLine("Count = "+avl.Count);
            Console.WriteLine(DateTime.Now - dateTime);
            int[] copy = list.ToArray();
            
            dateTime = DateTime.Now;
            Console.WriteLine("expect to run in 2 hours");
            for (int i = 0; i < copy.Length; i++)
            {
                list.Remove(copy[i]);
            }
            
            Console.WriteLine(DateTime.Now - dateTime);
            var query = from r in list where r > 10 group r by r into g select g;
            while (true)
            {
                Console.ReadLine();
            }
        }

        public static int Hash(int key, int slotNum)
        {
            double d = (Math.Sqrt(5) - 1) / 2;
            int slot = (int)(((key * d) - (int)(key * d)) * slotNum);
            return slot;
        }


        public static void WaitCallBackDelegate(object state)
        {
            try
            {
                if (state == null || !(state is Socket))
                {
                    return;
                }

                Socket socket = state as Socket;
                Console.WriteLine(SocketClient.GetClientIP(socket) + "sends");
                Console.WriteLine(SocketClient.Receive(socket, Encoding.UTF8));
                SocketClient.Close(socket);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
