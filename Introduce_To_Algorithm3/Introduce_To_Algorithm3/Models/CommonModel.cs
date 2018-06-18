using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Models
{
    /// <summary>
    /// 通用模型
    /// </summary>
    public class CommonModel
    {
        private string _commonModelTypeStr;
        private object _commonModelObject;

        /// <summary>
        /// locker
        /// </summary>
        private readonly object _locker = new object();

        /// <summary>
        /// 模型类型
        /// </summary>
        public string CommonModelTypeStr
        {
            get
            {
                lock (_locker)
                {
                    return _commonModelTypeStr;
                }
            }
            set
            {
                lock (_locker)
                {
                    _commonModelTypeStr = value;
                }
            }
        }


        /// <summary>
        /// 模型对象
        /// </summary>
        public object CommonModelObject
        {
            get
            {
                lock (_locker)
                {
                    return _commonModelObject;
                }
            }
            set
            {
                lock (_locker)
                {
                    _commonModelObject = value;
                }
            }
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
