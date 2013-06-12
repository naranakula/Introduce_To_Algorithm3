using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.AdvancedStructs
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="V"></typeparam>
    public class ProtovEB<V>
    {
        /// <summary>
        /// the universe size
        /// if u=2, it contains A[0..1] of two bits
        /// if u=2^2^k, this also contains a pointer named summary to a proto-vEB(U^0.5) and a array cluster[0,...,U^0.5-1] of U^0.5 pointer, each to a proto-vEB(U^0.5) structure.
        /// 
        /// </summary>
        public int U { get; set; }
    }
}
