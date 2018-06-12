using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils
{


    /// <summary>
    /// 集合辅助类
    /// </summary>
    public static class CollectionUtils
    {
        /// <summary>
        /// 重排
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inList">不修改inList</param>
        /// <returns></returns>
        public static List<T> Shuffle<T>(IList<T> inList) 
        {
            List<T> outList = new List<T>();

            if (inList == null || inList.Count == 0)
            {
                //没有元素
                return outList;
            }
           
            //拷贝原来的一份数据
            foreach (var item in inList)
            {
                outList.Add(item);
            }
            
            #region 重排

            Random random = new Random();
            int count = outList.Count;//集合中元素的总数量
            for (int i = 0; i < count - 1; i++)
            {
                int j = random.Next(i, count);//返回[i,count-1]的数，对i，j交换
                if (i != j)
                {
                    T elementOfI = outList[i];
                    T elementOfJ = outList[j];
                    outList[i] = elementOfJ;
                    outList[j] = elementOfI;
                }
            }

            #endregion
            

            return outList;
        }


        /// <summary>
        /// 判断集合是否为空
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static bool IsCollectionEmtpy(ICollection collection)
        {
            return collection == null || collection.Count == 0;
        }

        /// <summary>
        /// 浅复制
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inList">IList继承ICollection</param>
        /// <returns></returns>
        public static List<T> ShallowCopy<T>(ICollection<T> inList)
        {
            List<T> outList = new List<T>();

            if (inList == null || inList.Count == 0)
            {
                //没有元素
                return outList;
            }

            //拷贝原来的一份数据
            foreach (var item in inList)
            {
                outList.Add(item);
            }

            return outList;
        }

        /// <summary>
        /// 比较两个集合
        /// 对于数值类型没有问题
        /// 对于类类型，要求集合中的元素不能为null，否则会抛异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inList1"></param>
        /// <param name="inList2"></param>
        /// <returns></returns>
        public static bool EqualsEx<T>(IList<T> inList1, IList<T> inList2) where T:IEquatable<T>
        {
            bool isEmpty1 = inList1 == null || inList1.Count == 0;
            bool isEmpty2 = inList2 == null || inList2.Count == 0;

            //同时为空
            if (isEmpty1 && isEmpty2)
            {
                return true;
            }

            //只有一个为空
            if (isEmpty1 || isEmpty2)
            {
                return false;
            }

            int count1 = inList1.Count;
            int count2 = inList2.Count;
            if (count2 != count1)
            {
                //数量不一致
                return false;
            }


            for (int i = 0; i < count1; i++)
            {
                T item1 = inList1[i];
                T item2 = inList2[i];
                //这个实现是有问题的
                if (item1.Equals(item2) == false)
                {
                    return false;
                }
            }

            return true;
        }
    }


}
