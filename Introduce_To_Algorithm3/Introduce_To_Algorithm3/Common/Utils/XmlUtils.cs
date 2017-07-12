using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;

namespace Introduce_To_Algorithm3.Common.Utils
{


    /// <summary>
    /// 解析xml的辅助类
    /// </summary>
    public static class XmlUtils
    {
        /// <summary>
        /// 解析xml字符串，返回根元素
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static XElement ParseString(String xmlString)
        {
            if (string.IsNullOrWhiteSpace(xmlString))
            {
                return null;
            }

            XDocument document = XDocument.Parse(xmlString.Trim());

            if(document != null)
            {
                return document.Root;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        ///  解析xml字符串，返回根元素
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static Tuple<bool,XElement,Exception> TryParseString(string xmlString)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(xmlString))
                {
                    return new Tuple<bool, XElement, Exception>(false, null, new Exception("字符串不能为空白"));
                }

                XDocument document = XDocument.Parse(xmlString.Trim());
                XElement xelement = document.Root;
                if(xelement!= null)
                {
                    return new Tuple<bool, XElement, Exception>(true, xelement, null);
                }
                else
                {
                    return new Tuple<bool, XElement, Exception>(false, null, new Exception("未获取到xml根元素"));
                }
            }
            catch(Exception ex)
            {
                return new Tuple<bool, XElement, Exception>(false, null, ex);
            }
        }



        /// <summary>
        /// 根据节点名获取第一个匹配的一级子元素，不考虑命名空间
        /// XElement.Element("name")会进行命名空间的匹配
        /// 
        /// 注：有可以忽略的效率问题
        /// </summary>
        /// <param name="container">XDocment和XElement的基类</param>
        /// <param name="name"></param>
        /// <param name="ignoreCaseAndWhiteSpace">是否忽略大小写和空白</param>
        /// <returns></returns>
        public static XElement GetFirstMatchElement(this XContainer container, string name, bool ignoreCaseAndWhiteSpace = false)
        {
            if (ignoreCaseAndWhiteSpace)
            {
                return container.Elements().FirstOrDefault(r => StringUtils.EqualsEx(r.Name.LocalName, name));
            }
            else
            {
                return container.Elements().FirstOrDefault(r => r.Name.LocalName == name);
            }
        }

        /// <summary>
        /// 根据节点名获取匹配的一级子元素集合，不考虑命名空间
        /// XElement.Element("name")会进行命名空间的匹配
        /// 
        /// 注：有可以忽略的效率问题
        /// </summary>
        /// <param name="container">XDocment和XElement的基类</param>
        /// <param name="name"></param>
        /// <param name="ignoreCaseAndWhiteSpace">是否忽略大小写和空白</param>
        /// <returns></returns>
        public static List<XElement> GetElements(this XContainer container, string name, bool ignoreCaseAndWhiteSpace = false)
        {
            if (ignoreCaseAndWhiteSpace)
            {
                return container.Elements().Where(r => StringUtils.EqualsEx(r.Name.LocalName, name)).ToList();
            }
            else
            {
                return container.Elements().Where(r => r.Name.LocalName == name).ToList();
            }
        }

        /// <summary>
        /// 获取xe下name子元素的值
        /// </summary>
        /// <param name="xe"></param>
        /// <param name="name"></param>
        /// <param name="defaultValue">如果找不到name子元素的返回值</param>
        /// <param name="isTrim">是否忽略结果前后空白</param>
        /// <returns>第一项获取的字符串；第二项是否能够找到指定name的子元素</returns>
        public static Tuple<string, bool> GetString(this XElement xe, string name, string defaultValue = "",
            bool isTrim = true)
        {
            try
            {
                //查找xe的一级子目录
                var temp = xe.GetFirstMatchElement(name);

                if (temp == null)
                {
                    return new Tuple<string, bool>(defaultValue, false);
                }


                string theValue = temp.Value;
                if (isTrim)
                {
                    return new Tuple<string, bool>(theValue==null?string.Empty:theValue.Trim(), true);
                }
                else
                {
                    return new Tuple<string, bool>(theValue, true);
                }
            }
            catch (Exception ex)
            {
                NLogHelper.Error("解析节点{0}失败：{1}".FormatWith(name, ex));
                return new Tuple<string, bool>(defaultValue, false);
            }
        } 

    }


}
