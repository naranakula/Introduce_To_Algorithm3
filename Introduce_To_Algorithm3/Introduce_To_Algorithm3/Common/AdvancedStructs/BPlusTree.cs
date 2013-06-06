using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.AdvancedStructs
{
    /// <summary>
    /// B+ tree
    /// <remarks>
    /// B+ tree is variant of B- tree.
    /// 1) non child node's keys don't key value, just pointer or index, all the data keeps in leaf node
    /// 2)leaf nodes contains all the keys information and the keys are sorted
    /// 3)node contains the max(or min) value of child tree
    /// 
    /// 
    /// it's better for file index system
    /// </remarks>
    /// </summary>
    public class BPlusTree<K,V> where K:IComparable<K>,IEquatable<K>
    {
    }
}
