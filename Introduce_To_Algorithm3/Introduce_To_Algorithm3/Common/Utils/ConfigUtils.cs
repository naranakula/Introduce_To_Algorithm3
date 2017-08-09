using System;
using System.Configuration;

namespace Introduce_To_Algorithm3.Common.Utils
{
    /// <summary>
    /// 获取应用程序配置的工具类
    /// 添加引用：System.Configuration.dll
    /// </summary>
    public static class ConfigUtils
    {
        #region Settings

        /*
         * 1 通过Settings.Default来访问Setting
         * 2 Setting项有两个作用域：用户作用域和应用程序作用域。应用程序域只能读，用户作用域可以读可以写。
         * 3 Settings.Default.Save();保存对用户设置所做的更改
         */

        /// <summary>
        /// get
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetSettingsString(string name)
        {
            //使用如下属性名的方式更常见，使用索引只是为了演示
            //Properties.Settings.Default.myColor;
            return Properties.Settings.Default[name] as string;
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="name"></param>
        public static void SetSettingsString(string name)
        {
            //使用如下属性名的方式更常见，使用索引只是为了演示
            //只能用于User类型
            //Properties.Settings.Default.myColor = Color.AliceBlue;
            Properties.Settings.Default[name] = ConsoleColor.Black;

            //一定记住保存
            Properties.Settings.Default.Save();
        }

        #endregion

        /// <summary>
        /// 获取配置字符串
        /// </summary>
        /// <param name="key">配置文件的键</param>
        /// <param name="defaultValue">获取不到对应的键值时的默认返回值</param>
        /// <returns></returns>
        public static string GetString(string key, string defaultValue = "")
        {
            if (string.IsNullOrEmpty(key))
            {
                return defaultValue;
            }
            try
            {
                string value = ConfigurationManager.AppSettings[key];
                return value == null ? defaultValue : value.Trim();
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 获取配置字符串
        /// </summary>
        /// <param name="key">配置文件中的键</param>
        /// <param name="resultValue">获取到键对应的值，仅在返回值为true时，有意义</param>
        /// <returns>是否获取成功</returns>
        public static bool GetString(string key, out string resultValue)
        {
            resultValue = string.Empty;
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            try
            {
                resultValue = ConfigurationManager.AppSettings[key];
                return !string.IsNullOrEmpty(resultValue);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取配置的整数值
        /// </summary>
        /// <param name="key">配置文件的键</param>
        /// <param name="defaultValue">获取不到对应的键值时的默认返回值</param>
        /// <returns></returns>
        public static int GetInteger(string key, int defaultValue=0)
        {
            if (string.IsNullOrEmpty(key))
            {
                return defaultValue;
            }
            try
            {
                int value;

                string settingValue = ConfigurationManager.AppSettings[key];
                settingValue = settingValue == null ? "" : settingValue.Trim();
                if (int.TryParse(settingValue, out value))
                {
                    return value;
                }
                else
                {
                    return defaultValue;
                }
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 获取配置的整数值
        /// </summary>
        /// <param name="key">配置文件中的键</param>
        /// <param name="resultValue">获取到键对应的值，仅在返回值为true时，有意义</param>
        /// <returns>是否获取成功</returns>
        public static bool GetInteger(string key, out int resultValue)
        {
            resultValue = -1;
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }

            try
            {
                return int.TryParse(ConfigurationManager.AppSettings[key], out resultValue);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取配置的double值
        /// </summary>
        /// <param name="key">配置文件的键</param>
        /// <param name="defaultValue">获取不到对应的键值时的默认返回值</param>
        /// <returns></returns>
        public static double GetDouble(string key, double defaultValue=0)
        {
            if (string.IsNullOrEmpty(key))
            {
                return defaultValue;
            }
            try
            {
                double value;
                if (double.TryParse(ConfigurationManager.AppSettings[key], out value))
                {
                    return value;
                }
                else
                {
                    return defaultValue;
                }
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 获取配置的double值
        /// </summary>
        /// <param name="key">配置文件中的键</param>
        /// <param name="resultValue">获取到的结果值，仅在返回值为true时，有效</param>
        /// <returns></returns>
        public static bool GetDouble(string key, out double resultValue)
        {
            resultValue = -1.0;
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }

            try
            {
                return double.TryParse(ConfigurationManager.AppSettings[key], out resultValue);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetConnString(string key, string defaultValue = "")
        {
            try
            {
                return ConfigurationManager.ConnectionStrings[key].ConnectionString;
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
    }
}