using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Models
{
    /// <summary>
    /// 通用对象模型
    /// </summary>
    public class CommonObject
    {
        private volatile string _objectType;
        private volatile object _theObject;

        /// <summary>
        /// locker
        /// </summary>
        private readonly object _locker = new object();


        /// <summary>
        /// 默认构造函数
        /// </summary>
        public CommonObject()
        {
            
        }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="theObject">底层对象</param>
        public CommonObject(string objectType,Object theObject)
        {
            TheObjectType = objectType;
            TheObject = theObject;
        }


        /// <summary>
        /// 对象类型
        /// </summary>
        public string TheObjectType
        {
            get
            {
                lock (_locker)
                {
                    return _objectType;
                }
            }
            set
            {
                lock (_locker)
                {
                    _objectType = value;
                }
            }
        }


        /// <summary>
        /// 底层对象
        /// </summary>
        public object TheObject
        {
            get
            {
                lock (_locker)
                {
                    return _theObject;
                }
            }
            set
            {
                lock (_locker)
                {
                    _theObject = value;
                }
            }
        }


        /// <summary>
        /// 将TheObject转换成T
        /// 如果对象本身为null，返回null
        /// 转换失败返回null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetTheObject<T>() where T:class
        {
            object commonObj = TheObject;

            return commonObj as T;
        }




    }

    /// <summary>
    /// 对象类型常量
    /// </summary>
    public class CommonModelType
    {
        public const string LogType = "log";
    }
}
