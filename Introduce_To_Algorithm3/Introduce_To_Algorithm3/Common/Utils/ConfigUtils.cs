using System;
using System.Collections.Generic;
using System.Configuration;

namespace Com.Utility.Commons
{
    /// <summary>
    /// 获取应用程序配置的工具类
    /// </summary>
    public static class ConfigUtils
    {
        /// <summary>
        /// 获取配置字符串
        /// </summary>
        /// <param name="key">配置文件的键</param>
        /// <param name="defaultValue">获取不到对应的键值时的默认返回值</param>
        /// <returns></returns>
        public static string GetString(string key,string defaultValue="")
        {
            if(string.IsNullOrEmpty(key))
            {
                return defaultValue;
            }
            try
            {
                string value = ConfigurationManager.AppSettings[key];
                return value ?? defaultValue ;
            }
            catch(Exception)
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
        public static bool GetString(string key,out string resultValue)
        {
            resultValue = string.Empty;
            if(string.IsNullOrEmpty(key))
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
        public static int GetInteger(string key,int defaultValue)
        {
            if(string.IsNullOrEmpty(key))
            {
                return defaultValue;
            }
            try
            {
                int value;
                if(int.TryParse(ConfigurationManager.AppSettings[key],out value))
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
        public static bool GetInteger(string key,out int resultValue)
        {
            resultValue = -1;
            if(string.IsNullOrEmpty(key))
            {
                return false;
            }

            try
            {
                return int.TryParse(ConfigurationManager.AppSettings[key],out resultValue);
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
        public static double GetDouble(string key,double defaultValue)
        {
            if(string.IsNullOrEmpty(key))
            {
                return defaultValue;
            }
            try
            {
                double value;
                if(double.TryParse(ConfigurationManager.AppSettings[key],out value))
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
        public static bool GetDouble(string key,out double resultValue)
        {
            resultValue = -1.0;
            if(string.IsNullOrEmpty(key))
            {
                return false;
            }

            try
            {
                return double.TryParse(ConfigurationManager.AppSettings[key], out resultValue) ;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}