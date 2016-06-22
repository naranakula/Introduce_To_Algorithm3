using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common
{
    /// <summary>
    /// Settings.settings中每一项有四个属性：
    /// Name(名称):属性名
    /// Type(类型):属性类型如System.String
    /// Scope（作用域）:作用域分为两个值：Application(应用程序)和User(用户) Application是指无论用户首选项如何，应用程序使用的值，User配置的是用户首选项   Application类型的值运行时只读， User作用域在运行时是读写权限
    /// Value(值):取值
    /// </summary>
    public static class SettingHelper
    {
        /// <summary>
        /// get
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetString(string name)
        {
            //使用如下属性名的方式更常见，使用索引只是为了演示
            //Properties.Settings.Default.myColor;
            return Properties.Settings.Default[name] as string;
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="name"></param>
        public static void SetString(string name)
        {
            //使用如下属性名的方式更常见，使用索引只是为了演示
            //只能用于User类型
            //Properties.Settings.Default.myColor = Color.AliceBlue;
            Properties.Settings.Default[name] = ConsoleColor.Black;

            //一定记住保存
            Properties.Settings.Default.Save();
        }
    }
}
