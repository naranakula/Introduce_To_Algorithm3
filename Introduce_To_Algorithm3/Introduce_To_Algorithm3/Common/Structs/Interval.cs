using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Structs
{
    /// <summary>
    /// A closed interval is an ordered pair of real numbers [t1,t2], with t1 &lt;= t2.
    /// </summary>
    public class Interval
    {
        public int Low { get; set; }
        public int High { get; set; }


        public bool Overlap(Interval interval)
        {
            System.Diagnostics.Debug.Assert(interval != null);
            return Low <= interval.High && interval.Low <= High;
        }
    }
}
