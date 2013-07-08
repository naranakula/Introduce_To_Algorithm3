using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Utils
{
    public class CodeCounter
    {
        /// <summary>
        /// get line count
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static int GetCodeLines(string dir,string pattern = "*.cs")
        {
            DirectoryInfo di = new DirectoryInfo(dir);
            if (!di.Exists)
            {
                return 0;
            }

            FileInfo[] fis = di.GetFiles(pattern, SearchOption.AllDirectories);

            int count = 0;
            foreach (FileInfo fi in fis)
            {
                using (StreamReader sr = fi.OpenText())
                {
                    while (sr.ReadLine() != null)
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }
}
