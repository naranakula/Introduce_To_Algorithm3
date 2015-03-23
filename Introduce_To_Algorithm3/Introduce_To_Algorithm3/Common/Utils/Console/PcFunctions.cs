using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils
{
    public class PcFunctions
    {
        /// <summary>
        /// 锁定工作台的显示，效果等同于开始按钮的锁定和CTRL+ALT+DEL
        /// 该方法异步执行
        /// </summary>
        [DllImport("user32")]
        public static extern void LockWorkStation();


    }
}
