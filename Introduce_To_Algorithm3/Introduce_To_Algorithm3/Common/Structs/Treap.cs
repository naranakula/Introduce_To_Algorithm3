using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Structs
{
    /// <summary>
    /// we assign node.priority which is a random number chosen independently for each node
    /// The nodes of the treap are ordered so that the keys obey the binary-search-tree property and the priorities obey the min-heap order property
    /// if v is a left child of u, then v.key &lt;= u.key
    /// if v is a right child of u, then v.key  &gt;= u.key
    /// if v is a child of u, the v.priority &gt;= u.priority
    /// 
    /// Then the resulting treap is the tree that would have been formed as if the nodes had been inserted into a normal binary search tree in the order given by their priorities. i.e., x.priority &lt; y.priority means that we had inserted x before y
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class Treap<K, V> where K : IComparable<K>, IEquatable<K>
    {
    }
}
