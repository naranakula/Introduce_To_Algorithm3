using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Utils
{
    public static class DateTimeUtils
    {
        /// <summary>
        /// get a now string represent
        /// </summary>
        public static String NowString
        {
            get { return DateTime.Now.ToString(@"yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffffK"); }
        }
    }
}
