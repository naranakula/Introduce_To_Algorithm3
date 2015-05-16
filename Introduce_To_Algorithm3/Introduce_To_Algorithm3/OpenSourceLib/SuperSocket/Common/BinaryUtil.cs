using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.OpenSourceLib.SuperSocket.Common
{
    /// <summary>
    /// Binary util class
    /// </summary>
    public static class BinaryUtil
    {
        /// <summary>
        /// Search target from source
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">the source</param>
        /// <param name="target">the target</param>
        /// <param name="pos">从pos开始查找</param>
        /// <param name="length">查找的长度</param>
        /// <returns>找到返回[pos,pos+length-1]的值，找不到返回-1</returns>
        public static int IndexOf<T>(this IList<T> source, T target, int pos, int length)
        {
            for (int i = pos; i < pos+length && i<source.Count; i++)
            {
                if (source[i].Equals(target))
                {
                    return i;
                }
            }

            return -1;
        }


        /// <summary>
        /// Search target from source
        /// </summary>
        /// <param name="source">the source</param>
        /// <param name="target">the target</param>
        /// <param name="pos">从pos开始查找</param>
        /// <param name="length">查找的长度</param>
        /// <returns>找到返回[pos,pos+length-1]的值，找不到返回-1</returns>
        public static int IndexOf(this IList<byte> source, byte target, int pos, int length)
        {
            for (int i = pos; i < pos + length && i < source.Count; i++)
            {
                if (source[i] == target)
                {
                    return i;
                }
            }

            return -1;
        }



    }
}
