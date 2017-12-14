using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Introduce_To_Algorithm3.Common.Utils.strings;

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
         * 2 Setting项有两个作用域：用户作用域和应用程序作用域。应用程序域只能读，用户作用域可以读可以写。用户作用域的修改不是直接修改app.config文件，而是保存在application data文件下
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
            string str = Properties.Settings.Default[name] as string;
            return str == null ? "" : str.Trim();
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

        #region appSettings

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
                

                string settingValue = ConfigurationManager.AppSettings[key];

                if (string.IsNullOrWhiteSpace(settingValue))
                {
                    return defaultValue;
                }

                
                int value;
                if (int.TryParse(settingValue.Trim(), out value))
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

        #endregion


        #region 自定义的配置信息

        #region 数据结构定义

        /// <summary>
        /// 一个配置项
        /// </summary>
        public class  ConfigItem
        {
            /// <summary>
            /// 配置的键
            /// </summary>
            public string ConfigKey { get; set; }

            /// <summary>
            /// 配置的值
            /// </summary>
            public string ConfigValue { get; set; }

            /// <summary>
            /// 关于配置信息的描述，作为辅助信息，不直接使用
            /// </summary>
            public string ConfigDesc { get; set; }
        }

        /// <summary>
        /// 配置集合
        /// </summary>
        public class ConfigCollection
        {
            public ConfigCollection()
            {
                ConfigItems = new List<ConfigItem>();
            }

            /// <summary>
            /// 具体的配置项
            /// </summary>
            public List<ConfigItem> ConfigItems { get; set; }
        }

        #endregion

        #region 加载&保存本地配置  键是唯一的
        /// <summary>
        /// 建议保存个性化配置，全局配置放到app.config中
        /// 保存本地配置的xml文件
        /// </summary>
        private static readonly string ConfigXmlFileName = "localconfig.xml";

        /// <summary>
        /// 从localconfig.xml中加载或者重新加载本地配置文件
        /// 返回是否加载成功
        /// </summary>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public static bool LoadLocalConfig(Action<Exception> exceptionHandler = null)
        {
            try
            {
                if (!File.Exists(ConfigXmlFileName))
                {
                    //没有配置
                    _localConfigDicts.Clear();
                    //配置文件不存在
                    return true;
                }

                ConfigUtils.ConfigCollection configs = LoadFileToObject<ConfigCollection>(ConfigXmlFileName, ex =>
                {
                    exceptionHandler?.Invoke(ex);
                });

                if (configs == null)
                {
                    //加载失败
                    return false;
                }

                //清空，并重新加载
                _localConfigDicts.Clear();
                foreach (ConfigItem item in configs.ConfigItems)
                {
                    _localConfigDicts.AddOrUpdate(item.ConfigKey, item, (k, v) => item);
                }

                return true;
            }
            catch (Exception e)
            {
                exceptionHandler?.Invoke(e);
                return false;
            }
        }

        /// <summary>
        /// 持久化本地配置信息到localconfig中,如果文件存在，则覆盖
        /// </summary>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public static bool SaveLocalConfig(Action<Exception> exceptionHandler = null)
        {
            try
            {
                var configList = _localConfigDicts.Values.ToList().OrderBy(r=>r.ConfigKey).ToList();
                ConfigCollection collection = new ConfigCollection();
                collection.ConfigItems.AddRange(configList);
                
                return ToFile(collection,ConfigXmlFileName, ex =>
                {
                    exceptionHandler?.Invoke(ex);
                });
            }
            catch (Exception e)
            {
                exceptionHandler?.Invoke(e);
                return false;
            }
        }

        /// <summary>
        /// 添加或者更新新的本地配置
        ///  未持久化到文件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddOrUpdateLocalConfig(string key, string value)
        {
            ConfigItem oldItem = null;
            _localConfigDicts.TryGetValue(key, out oldItem);
            ConfigItem newItem = new ConfigItem();
            if (oldItem != null)
            {
                newItem.ConfigValue = value;
                newItem.ConfigDesc = oldItem.ConfigDesc;
            }
            else
            {
                newItem.ConfigKey = key;
                newItem.ConfigValue = value;
                newItem.ConfigDesc = string.Empty;
            }
            _localConfigDicts.AddOrUpdate(key, newItem, (k, v) => newItem);
        }

        /// <summary>
        /// 删除本地配置
        /// 未持久化到文件
        /// </summary>
        /// <param name="key"></param>
        public static void DeleteLocalConfig(string key)
        {
            ConfigItem oldItem = null;
            _localConfigDicts.TryRemove(key, out oldItem);
        }


        /// <summary>
        /// 底层存储
        /// </summary>
        private static readonly ConcurrentDictionary<string,ConfigItem> _localConfigDicts = new ConcurrentDictionary<string, ConfigItem>();


        #region 获取本地配置

        /// <summary>
        /// 从本地配置获取值
        /// </summary>
        /// <param name="key">键，不区分大小写</param>
        /// <param name="defaultValue">获取不到返回的默认值</param>
        /// <param name="exceptionHandler">异常处理</param>
        /// <returns></returns>
        public static string GetLocalConfigString(string key, string defaultValue = "",Action<Exception> exceptionHandler = null)
        {
            try
            {
                var keyList = _localConfigDicts.Keys.ToList();
                string actualKey = null;//实际的键

                foreach (var item in keyList)
                {
                    if (StringUtils.EqualsEx(key, item))
                    {
                        actualKey = item;
                        break;
                    }
                }

                if (string.IsNullOrEmpty(actualKey))
                {
                    return defaultValue;
                }

                ConfigItem configItem = null;
                if (_localConfigDicts.TryGetValue(actualKey, out configItem) && configItem != null)
                {
                    return StringUtils.TrimEx(configItem.ConfigValue);
                }
                else
                {
                    return defaultValue;
                }

            }
            catch (Exception e)
            {
                exceptionHandler?.Invoke(e);
                return defaultValue;
            }
        }

        /// <summary>
        /// 从本地配置获取值
        /// </summary>
        /// <param name="key">键，不区分大小写</param>
        /// <param name="defaultValue">获取不到返回的默认值</param>
        /// <param name="exceptionHandler">异常处理</param>
        /// <returns></returns>
        public static int GetLocalConfigInteger(string key, int defaultValue = -1, Action<Exception> exceptionHandler = null)
        {
            try
            {
                var keyList = _localConfigDicts.Keys.ToList();
                string actualKey = null;//实际的键

                foreach (var item in keyList)
                {
                    if (StringUtils.EqualsEx(key, item))
                    {
                        actualKey = item;
                        break;
                    }
                }

                if (string.IsNullOrEmpty(actualKey))
                {
                    return defaultValue;
                }

                ConfigItem configItem = null;
                if (_localConfigDicts.TryGetValue(actualKey, out configItem) && configItem != null)
                {
                    string theValue = StringUtils.TrimEx(configItem.ConfigValue);
                    int returnInt = -1;
                    if (int.TryParse(theValue, out returnInt))
                    {
                        return returnInt;
                    }
                    else
                    {
                        return defaultValue;
                    }
                }
                else
                {
                    return defaultValue;
                }

            }
            catch (Exception e)
            {
                exceptionHandler?.Invoke(e);
                return defaultValue;
            }
        }
        
        #endregion

        #endregion

        #region 序列化反序列化辅助方法

        /// <summary>
        /// 将对象序列化到文件destFile中
        /// 如果成功返回true,失败返回false
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="destFile">如果文件不存在，创建；否则覆盖</param>
        /// <param name="exceptionHandler">异常处理</param>
        public static bool ToFile<T>(T obj, string destFile, Action<Exception> exceptionHandler = null) where T : class
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add(string.Empty, string.Empty);
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = Encoding.UTF8;
                settings.Indent = true;

                using (StreamWriter streamWriter = new StreamWriter(destFile, false, Encoding.UTF8))
                {
                    using (XmlWriter writer = XmlWriter.Create(streamWriter, settings))
                    {
                        serializer.Serialize(writer, obj, ns);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }

                return false;
            }
        }

        /// <summary>
        /// 加载xml文件将其转换为对象
        /// 加载失败返回null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName">文件名，本方法不检测文件是否存在</param>
        /// <param name="exceptionHandler">异常处理</param>
        /// <returns></returns>
        public static T LoadFileToObject<T>(string fileName, Action<Exception> exceptionHandler = null) where T : class
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (StreamReader reader = new StreamReader(fileName, Encoding.UTF8))
                {
                    return (T)serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }

                return null;
            }
        }

        #endregion
        #endregion

    }
}