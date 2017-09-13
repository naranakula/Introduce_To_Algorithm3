using System;
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

    }


}
