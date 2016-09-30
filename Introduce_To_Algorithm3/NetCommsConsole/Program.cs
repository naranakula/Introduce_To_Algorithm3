using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetCommsConsole.Utils;

namespace NetCommsConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            HashSet<Guid> list = new HashSet<Guid>();
            int count = 100000;
            for (int i = 0; i < count; i++)
            {
                Guid guid = Guid.NewGuid();
                list.Add(guid);
            }

            Console.WriteLine(list.Count);
            Console.WriteLine(count);
        }
    }
}
