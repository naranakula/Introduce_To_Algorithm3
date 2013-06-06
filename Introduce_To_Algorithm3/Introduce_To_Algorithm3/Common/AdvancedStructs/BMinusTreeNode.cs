using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.AdvancedStructs
{
    /// <summary>
    /// we call t=M/2 as the minimum degree of the B- tree.
    /// each non root non leaf node has [t-1,2t-1] keys
    /// </summary>
    public class BMinusTreeNode<K,V> where K : IComparable<K>, IEquatable<K>
    {
        /// <summary>
        /// the number of keys in current node it must between [t-1,2t-1]
        /// </summary>
        public int N { get; set; }
        /// <summary>
        /// the number of keys in current Node
        /// </summary>
        public int Count { get { return N; } }

        /// <summary>
        /// t-1 number keys , keys[i] &lt;= keys[i+1]
        /// </summary>
        public Tuple<K,V>[] KeyValues { get; internal set; }

        /// <summary>
        /// t number children.     keys[i-1] =&gt; children[i] &lt;= keys[i]
        /// </summary>
        public BMinusTreeNode<K,V>[] Children { get; internal set; }

        /// <summary>
        /// whether is leaf
        /// </summary>
        public bool IsLeaf { get; internal set; }
    }
}
