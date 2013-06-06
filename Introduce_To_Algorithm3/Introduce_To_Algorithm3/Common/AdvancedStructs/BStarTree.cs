using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.AdvancedStructs
{
    /// <summary>
    /// B* tree is a variant of B+ tree.
    /// 
    /// B* tree add pointer to non root non leaf node to his brother
    /// it use disk more effective than B+ tree
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class BStarTree<K,V> where K:IComparable<K>,IEquatable<K>
    {

    }
}
