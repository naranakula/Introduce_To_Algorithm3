using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.Common.Utils.strings;

namespace Introduce_To_Algorithm3.Common.Utils
{
    public static class PropertyUtils
    {


        #region 加载properties文件

        /// <summary>
        /// 底层数据存储
        /// </summary>
        private static readonly ConcurrentDictionary<string, string> SPropertiesDict = new ConcurrentDictionary<string, string>();


        /// <summary>
        /// 获取底层的Properties数据存储
        /// </summary>
        public static ConcurrentDictionary<string, string> PropertiesConcurrentDict
        {
            get { return SPropertiesDict; }
        }

        /// <summary>
        /// 加载properties
        /// </summary>
        /// <param name="propertiesFile">文件采用utf8编码</param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public static bool LoadProperties(string propertiesFile = "config.properties", Action<Exception> exceptionHandler = null)
        {
            try
            {
                if (string.IsNullOrEmpty(propertiesFile) || !File.Exists(propertiesFile))
                {
                    throw new Exception($"未找到{propertiesFile}文件");
                }

                //This method attempts to automatically detect the encoding of a file based on the presence of byte order marks. Encoding formats UTF-8 and UTF-32 (both big-endian and little-endian) can be detected. 支持UTF8无bom
                string[] lines = File.ReadAllLines(propertiesFile);

                //清空dict  清空会在add之前的短暂时间内没有数据  不清空有可能多数据
                //暂不清空
                //_propertiesDict.Clear();

                foreach (string item in lines)
                {
                    string temp = StringUtils.TrimEx(item);
                    if (string.IsNullOrEmpty(temp))
                    {
                        continue;
                    }

                    if (temp.StartsWith("#"))
                    {
                        continue;
                    }

                    string[] array = temp.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

                    if (array.Length != 2)
                    {
                        continue;
                    }

                    //key的大小写无所谓
                    string key = StringUtils.Normalize(array[0]);
                    string val = StringUtils.TrimEx(array[1]);
                    //后面的值优先
                    SPropertiesDict.AddOrUpdate(key, val, (k, oldv) => val);
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
        /// 获取properties的值
        /// </summary>
        /// <param name="key">键不区分大小写</param>
        /// <param name="defaultValueIfNotExist">如果查找不到的默认值</param>
        /// <returns></returns>
        public static string GetPropertiesString(string key, string defaultValueIfNotExist = "")
        {
            string returnValue = "";
            string normalizedKey = StringUtils.Normalize(key);
            if (SPropertiesDict.TryGetValue(normalizedKey, out returnValue))
            {
                return returnValue;
            }
            else
            {
                return defaultValueIfNotExist;
            }
        }

        #endregion

    }
}
