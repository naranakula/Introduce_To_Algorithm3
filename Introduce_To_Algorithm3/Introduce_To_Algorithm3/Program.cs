

using System;
using System.Data.Entity;
using System.Linq;
using Introduce_To_Algorithm3.Common.Utils.sqls.EF2;

namespace Introduce_To_Algorithm3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (EfDbContext context = new EfDbContext())
            {
                var result = context.Set<Person>().Where(r => r.Name == "Hack").Include(r=>r.Phones);
                foreach (var item in result)
                {
                    Console.WriteLine(item.CreateTime);
                    Console.WriteLine(item.Phones.Count);
                }
            }
        }
    }
}
