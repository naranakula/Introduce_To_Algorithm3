using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Introduce_To_Algorithm3.Common.Utils.ConsoleUtils
{
    /// <summary>
    /// 屏幕， 开发分屏
    /// 添加 System.Windows.Forms 和 System.Drawing 引用
    /// </summary>
    public static class ScreenUtils
    {
        /// <summary>
        /// 获取所有的屏幕
        /// </summary>
        /// <returns></returns>
        public static Screen[] GetAllScreens()
        {
            return Screen.AllScreens;
        }

        /// <summary>
        /// 获取主屏幕
        /// </summary>
        /// <returns></returns>
        public static Screen GetMainScreen()
        {
            return  Screen.PrimaryScreen;
        }

    }
}
