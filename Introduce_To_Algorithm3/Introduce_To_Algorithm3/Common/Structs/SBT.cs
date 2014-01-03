using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Structs
{
    /// <summary>
    /// Size balanced Tree matain a size property. which meet
    /// s[right[t]] >= s[left[left[t]]], s[right[left[t]]]
    /// s[left[t]] >= s[left[right[t]]], s[right[right[t]]]
    /// 
    /// which runs at O(lgN)
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class SBT<K, V> where K : IComparable<K>, IEquatable<K>
    {
    }
}
