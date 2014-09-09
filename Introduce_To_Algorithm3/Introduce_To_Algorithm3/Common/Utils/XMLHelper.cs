using System;
using System.Collections.Generic;
using System.Text;
//using System.Collections;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;

/**************************************************
 * author:           lcm
 * starttime:        2011-9-2 15:43:42
 * finishtime:       
 * modifytime0:
 **************************************************/

namespace iFlyTek.SMSS10.ProcEngine.SMSHall.BaseRoute
{
    /// <summary>
    /// 处理xml操作的通用类
    /// </summary>
    /// <remarks>
    /// 用于加载、保存xml文档，查询、修改、增加和删除xml节点。
    /// </remarks>
    public class XMLHelper
    {
        #region 字段

        /// <summary>
        /// 表示xml文档实例
        /// </summary>
        private XmlDocument m_xmlDocument;

        /// <summary>
        /// 为定位和编辑 XML 数据提供游标模型
        /// </summary>
        private XPathNavigator m_navigator;

        /// <summary>
        /// 保存最近一次错误信息
        /// </summary>
        private string m_sLastErrorMessage;

        #endregion

        #region 属性

        /// <summary>
        /// 返回xml文档的根元素
        /// </summary>
        public XmlElement RootNode
        {
            get
            {
                return m_xmlDocument.DocumentElement;
            }
        }

        /// <summary>
        /// 返回xml文档实例
        /// </summary>
        public XmlDocument Document
        {
            get
            {
                return m_xmlDocument;
            }
        }

        /// <summary>
        /// 为定位和编辑xml文档提供的游标实例
        /// </summary>
        public XPathNavigator Navigator
        {
            get
            {
                return m_navigator;
            }
        }

        /// <summary>
        /// 最后一次错误信息
        /// </summary>
        public string LastErrorMessage
        {
            get
            {
                return m_sLastErrorMessage;
            }
            set
            {
                m_sLastErrorMessage = value;
            }
        }

        #endregion

        #region 辅助内部类:表示文档加载的源

        /// <summary>
        /// 表示文档加载的源
        /// </summary>
        public enum LoadType
        {
            /// <summary>
            /// 从字符串中加载xml文档
            /// </summary>
            FromString,
            /// <summary>
            /// 从本地磁盘文件中加载xml文档
            /// </summary>
            FromLocalFile,
            /// <summary>
            /// 从网络url来加载xml文档
            /// </summary>
            FromURL
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 将错误信息置为空，并构建XmlDocument对象。
        /// </summary>
        public XMLHelper()
        {
            m_sLastErrorMessage = string.Empty;
            m_xmlDocument = new XmlDocument();
        }

        /// <summary>
        /// 将错误信息置为空，并使用参数设置XmlDocument对象。
        /// </summary>
        /// <param name="xmlDocument">XmlDocument参数</param>
        public XMLHelper(XmlDocument xmlDocument)
        {
            m_sLastErrorMessage = string.Empty;
            
            //设置XmlDocument对象
            if (xmlDocument == null)
            {
                m_xmlDocument = new XmlDocument();
            }
            else
            {
                m_xmlDocument = xmlDocument;
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 保存xml文档的代理，具体的工作有具体的函数处理
        /// </summary>
        /// <param name="sTargetXML">要保存文件的目的地</param>
        /// <returns>true,保存成功；false,保存失败。</returns>
        public delegate bool Save(string sTargetXML);

        /// <summary>
        /// 保存xml到目标文件中
        /// </summary>
        /// <param name="sTargetFileName">要保存的xml目的地</param>
        /// <returns>true,保存成功；false,保存失败。</returns>
        public bool SaveToFile(string sTargetFileName)
        {
            bool result = false;

            try
            {
                m_xmlDocument.Save(sTargetFileName);
                result = true;
            }
            catch(XmlException e)
            {
                //异常处理
                this.HandleException(e);
            }
            return result;
        }

        /// <summary>
        /// 将 XPathNavigator 移动到根节点
        /// </summary>
        /// <returns>true,表示移动成功；false,表示移动失败。</returns>
        public bool MoveToRoot()
        {
            bool result = false;
            try
            {
                // 将 XPathNavigator 移动到当前节点所属的根节点。
                this.m_navigator.MoveToRoot(); 
                result = true;
            }
            catch (Exception e)
            {
                this.HandleException(e);
            }

            return result;
        }

        /// <summary>
        /// 加载xml字符串或者文件到内存中
        /// </summary>
        /// <param name="sourceXMLOrFile">源xml</param>
        /// <param name="loadType">加载的方式，如FromString,FromLocalFile,FromURL</param>
        /// <returns>true,加载成功；false，加载失败。</returns>
        public bool LoadXML(string sourceXMLOrFile,LoadType loadType)
        {
            bool result = false;

            try
            {
                switch (loadType)
                {
                    case LoadType.FromString:
                        //从字符串中加载xml文件
                        this.m_xmlDocument.LoadXml(sourceXMLOrFile);
                        break;

                    case LoadType.FromLocalFile:
                        //从本地文件加载xml文件
                        this.m_xmlDocument.Load(sourceXMLOrFile);
                        break;

                    case LoadType.FromURL:
                        {
                            string sURLContent = this.GetURLContent(sourceXMLOrFile);
                            this.m_xmlDocument.LoadXml(sURLContent);
                            break;
                        }

                    default:
                        string sErr = "Developer note: No LoadType case supported for "+loadType.ToString();
                        throw (new Exception(sErr));
                }
                //创建XPathNavigator游标，并移动到根节点
                this.DoPostLoadCreateInit();
                result = true;
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }

            return result;
        }

        /// <summary>
        /// 使用XPath表达式从xml文档中提取XmlNode
        /// </summary>
        /// <param name="xPathExpression">要查询的XPath表达式</param>
        /// <returns>一个装有XmlNode的容器</returns>
        public List<XmlNode> GetChildNodesFromDocument(string xPathExpression)
        {
            List<XmlNode> alResult = new List<XmlNode>();

            try
            {
                XmlNodeList nl = this.m_xmlDocument.SelectNodes(xPathExpression);

                if (nl != null)
                {
                    for (int i = 0; i < nl.Count; i++)
                    {
                        alResult.Add(nl.Item(i));
                    }
                }
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }

            return alResult;
        }

        /// <summary>
        /// 从xml文档中返回第一个符合条件的XmlNode
        /// </summary>
        /// <param name="xPathExpression">要搜索的XPath条件</param>
        /// <returns>第一个符合XPath条件的XmlNode节点，如果找不到，返回null</returns>
        public XmlNode GetFirstChildNodeFromDocument(string xPathExpression)
        {
            XmlNode node = null;

            try
            {
                node = m_xmlDocument.SelectSingleNode(xPathExpression);
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }

            return node;
        }

        /// <summary>
        /// 从指定节点获取指定属性名
        /// </summary>
        /// <param name="node">指定的节点</param>
        /// <param name="sAttributeName">指定的属性名</param>
        /// <returns>属性值。如果查找不到,返回string.Empty。</returns>
        public string GetAttributeValue(XmlNode node, string sAttributeName)
        {
            string sVal = string.Empty;

            try
            {
                XmlAttributeCollection attribColl = node.Attributes;
                XmlAttribute attrib = attribColl[sAttributeName];
                sVal = this.Decode(attrib.Value);
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }

            return sVal;
        }

        /// <summary>
        /// 从给定节点获取属性名和属性值的Dictionary
        /// </summary>
        /// <param name="node">给定XmlNode节点</param>
        /// <returns>属性名和属性值的Dictionary</returns>
        public Dictionary<string,string> GetAttributeValue(XmlNode node)
        {
            Dictionary<string,string> htAttributes = new Dictionary<string,string>();
            try
            {
                XmlAttributeCollection attribColl = node.Attributes;
                foreach (XmlAttribute attrib in attribColl)
                {
                    htAttributes.Add(attrib.Name, this.Decode(attrib.Value));
                }
                return htAttributes;
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }
            return null;
        }

        /// <summary>
        /// 从给定节点获取给定属性名的int类型的属性值
        /// </summary>
        /// <param name="node">给定XmlNode节点</param>
        /// <param name="sAttributeName">属性名</param>
        /// <returns>int类型的属性值</returns>
        public int GetAttributeInt32Value(XmlNode node, string sAttributeName)
        {
            string sVal = this.GetAttributeValue(node, sAttributeName);
            return sVal != "" ? Convert.ToInt32(sVal) : 0;
        }

        /// <summary>
        /// 从给定节点获取给定属性名的float类型的属性值
        /// </summary>
        /// <param name="node">给定XmlNode节点</param>
        /// <param name="sAttributeName">属性名</param>
        /// <returns>float类型的属性值</returns>
        public float GetAttributeFloatValue(XmlNode node, string sAttributeName)
        {
            string sVal = this.GetAttributeValue(node, sAttributeName);
            return sVal != "" ? Convert.ToSingle(sVal) : 0f;
        }

        /// <summary>
        /// 从给定节点获取给定属性名的double类型的属性值
        /// </summary>
        /// <param name="node">给定XmlNode节点</param>
        /// <param name="sAttributeName">属性名</param>
        /// <returns>double类型的属性值</returns>
        public double GetAttributeDoubleValue(XmlNode node, string sAttributeName)
        {
            string sVal = this.GetAttributeValue(node, sAttributeName);
            return sVal != "" ? Convert.ToDouble(sVal) : 0.0;
        }

        /// <summary>
        /// 从给定节点获取给定属性名的boolean类型的属性值
        /// </summary>
        /// <param name="node">给定XmlNode节点</param>
        /// <param name="sAttributeName">属性名</param>
        /// <returns>boolean类型的属性值</returns>
        public bool GetAttributeBooleanValue(XmlNode node, string sAttributeName)
        {
            string sVal = this.GetAttributeValue(node, sAttributeName);
            return sVal != "" ? Convert.ToBoolean(sVal) : false;
        }

        /// <summary>
        /// 获取XmlElement内的InnerXml
        /// </summary>
        /// <param name="xmlNode">要获取的XmlNode</param>
        /// <returns>InnerXml的值</returns>
        public string GetElementValue(XmlNode xmlNode)
        {
            string sVal = string.Empty;

            try
            {
                sVal = this.Decode(xmlNode.InnerXml);
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }

            return sVal;
        }

        /// <summary>
        /// 获取XmlElement内的int类型的InnerXml
        /// </summary>
        /// <param name="xmlNode">要获取的XmlNode</param>
        /// <returns>int类型的InnerXml的值</returns>
        public int GetElementInt32Value(XmlNode xmlNode)
        {
            string sVal = this.GetElementValue(xmlNode);
            return sVal != "" ? Convert.ToInt32(sVal) : 0;
        }

        /// <summary>
        /// 获取XmlElement内的float类型的InnerXml
        /// </summary>
        /// <param name="xmlNode">要获取的XmlNode</param>
        /// <returns>float类型的InnerXml的值</returns>
        public float GetElementFloatValue(XmlNode xmlNode)
        {
            string sVal = this.GetElementValue(xmlNode);
            return sVal != "" ? Convert.ToSingle(sVal) : 0f;
        }

        /// <summary>
        /// 获取XmlElement内的double类型的InnerXml
        /// </summary>
        /// <param name="xmlNode">要获取的XmlNode</param>
        /// <returns>double类型的InnerXml的值</returns>
        public double GetElementDoubleValue(XmlNode xmlNode)
        {
            string sVal = this.GetElementValue(xmlNode);
            return sVal != "" ? Convert.ToDouble(sVal) : 0.0;
        }

        /// <summary>
        /// 获取XmlElement内boolean类型的的InnerXml
        /// </summary>
        /// <param name="xmlNode">要获取的XmlNode</param>
        /// <returns>boolean类型的InnerXml的值</returns>
        public bool GetElementBooleanValue(XmlNode xmlNode)
        {
            string sVal = this.GetElementValue(xmlNode);
            return sVal != "" ? Convert.ToBoolean(sVal) : false;
        }

        /// <summary>
        /// 获取当前父节点的第一个名字为sElementName的子节点的InnerXml
        /// </summary>
        /// <param name="parentNode">XmlNode父节点</param>
        /// <param name="sElementName">要查找的元素名</param>
        /// <returns>父节点的第一个名字为sElementName的子节点的InnerXml</returns>
        public string GetChildElementValue(XmlNode parentNode, string sElementName)
        {
            string sVal = string.Empty;
            try
            {
                XmlNodeList childNodes = parentNode.ChildNodes;
                foreach (XmlNode childNode in childNodes)
                {
                    if (childNode.Name == sElementName)
                    {
                        sVal = this.GetElementValue(childNode);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }
            return sVal;
        }

        /// <summary>
        /// 获取当前父节点的第一个名字为sElementName的子节点的int类型的InnerXml
        /// </summary>
        /// <param name="parentNode">XmlNode父节点</param>
        /// <param name="sElementName">要查找的元素名</param>
        /// <returns>父节点的第一个名字为sElementName的子节点的int类型的InnerXml</returns>
        public int GetChildElementInt32Value(XmlNode parentNode, string sElementName)
        {
            string sVal = this.GetChildElementValue(parentNode, sElementName);
            return sVal != "" ? Convert.ToInt32(sVal) : 0;
        }

        /// <summary>
        /// 获取当前父节点的第一个名字为sElementName的子节点float类型的InnerXml
        /// </summary>
        /// <param name="parentNode">XmlNode父节点</param>
        /// <param name="sElementName">要查找的元素名</param>
        /// <returns>父节点的第一个名字为sElementName的子节点float类型的InnerXml</returns>
        public float GetChildElementFloatValue(XmlNode parentNode, string sElementName)
        {
            string sVal = this.GetChildElementValue(parentNode, sElementName);
            return sVal != "" ? Convert.ToSingle(sVal) : 0f;
        }

        /// <summary>
        /// 获取当前父节点的第一个名字为sElementName的子节点的double类型的InnerXml
        /// </summary>
        /// <param name="parentNode">XmlNode父节点</param>
        /// <param name="sElementName">要查找的元素名</param>
        /// <returns>父节点的第一个名字为sElementName的子节点的double类型的InnerXml</returns>
        public double GetChildElementDoubleValue(XmlNode parentNode, string sElementName)
        {
            string sVal = this.GetChildElementValue(parentNode, sElementName);
            return sVal != "" ? Convert.ToDouble(sVal) : 0.0;
        }

        /// <summary>
        /// 获取当前父节点的第一个名字为sElementName的子节点的bool类型的InnerXml
        /// </summary>
        /// <param name="parentNode">XmlNode父节点</param>
        /// <param name="sElementName">要查找的元素名</param>
        /// <returns>父节点的第一个名字为sElementName的子节点的bool类型的InnerXml</returns>
        public bool GetChildElementBooleanValue(XmlNode parentNode, string sElementName)
        {
            string sVal = this.GetChildElementValue(parentNode, sElementName);
            return sVal != "" ? Convert.ToBoolean(sVal) : false;
        }

        /// <summary>
        /// 从根节点返回第一个匹配元素名的XmlNode节点
        /// </summary>
        /// <param name="sElementName">待匹配的元素名</param>
        /// <returns>第一个匹配元素名的XmlNode节点</returns>
        public XmlNode GetFirstChildXmlNodeFromRoot(string sElementName)
        {
            XmlNodeList nodeList = this.GetChildNodesFromRoot(sElementName);
            if(nodeList.Count>0)
            {
                return nodeList[0];
            }
            return null;
        }

        /// <summary>
        /// 从根节点开始查找匹配指定名称的节点集
        /// </summary>
        /// <param name="sElementName">待匹配的名称</param>
        /// <returns>匹配指定名称的节点集</returns>
        private XmlNodeList GetChildNodesFromRoot(string sElementName)
        {
            return this.m_xmlDocument.GetElementsByTagName(sElementName);
        }

        /// <summary>
        /// 给定父节点，返回其子节点中第一个名称匹配的节点
        /// </summary>
        /// <param name="parentNode">待搜索的父节点</param>
        /// <param name="sElementName">匹配的元素名</param>
        /// <returns>子节点集中第一个名称匹配的节点</returns>
        public XmlNode GetFirstChildXmlNode(XmlNode parentNode, string sElementName)
        {
            XmlNode foundChildNode = null;
            try
            {
                XmlNodeList childNodes = parentNode.ChildNodes;
                foreach (XmlNode childNode in childNodes)
                {
                    if (childNode.Name == sElementName)
                    {
                        foundChildNode = childNode;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }

            return foundChildNode;
        }

        /// <summary>
        /// 给定父节点，返回其子节点中所有名称匹配的节点。这个方法会递归查找所有的子节点。
        /// </summary>
        /// <param name="parentNode">待搜索的父节点</param>
        /// <param name="sElementName">待匹配的元素名</param>
        /// <returns>子节点中所有名称匹配的节点</returns>
        public List<XmlNode> GetRecursiveChildNodesFromParent(XmlNode parentNode,string sElementName)
        {
            List<XmlNode> elementList = new List<XmlNode>();

            try
            {
                XmlNodeList children = parentNode.ChildNodes;
                foreach (XmlNode child in children)
                {
                    if (child.Name == sElementName)
                    {
                        elementList.Add(child);
                    }

                    if (child.HasChildNodes == true)
                    {
                        //如果当前节点有子节点，递归查找子节点
                        List<XmlNode> childrenList = this.GetRecursiveChildNodesFromParent(child, sElementName);
                        if (childrenList.Count > 0)
                        {
                            foreach (XmlNode subChild in childrenList)
                            {
                                elementList.Add(subChild);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                this.HandleException(ex);
            }

            return elementList;
        }

        /// <summary>
        /// 使用给定元素名和元素值创建一个XmlElement，并将其置于指定的节点下
        /// </summary>
        /// <param name="parentNode">待放置新节点的指定父节点</param>
        /// <param name="sElementName">新元素的元素名</param>
        /// <param name="sElementValue">新元素的元素值</param>
        /// <returns>新创建的XmlElement节点</returns>
        public XmlElement CreateNodeElement(XmlNode parentNode,string sElementName,string sElementValue)
        {
            XmlElement newElem = null;

            try
            {
                //创建新元素
                newElem = this.m_xmlDocument.CreateElement(sElementName);
                if (sElementValue != null)
                {
                    newElem.InnerXml = this.Encode(sElementValue);
                }
                else
                {
                    newElem.InnerXml = string.Empty;
                }

                //获取父节点所在的文档
                XmlDocument ownerDoc = parentNode.OwnerDocument;
                if (ownerDoc != null)
                {
                    parentNode.AppendChild(newElem);
                }
                else
                {
                    XmlElement root = this.m_xmlDocument.DocumentElement;
                    root.AppendChild(newElem);
                }
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }

            return newElem;
        }

        /// <summary>
        /// 在给定节点下增加一个XmlComment
        /// </summary>
        /// <param name="parentNode">要增加XmlComment的父节点</param>
        /// <param name="sVal">XmlComment的值</param>
        /// <returns>新增的Xml节点</returns>
        public XmlNode CreateComment(XmlNode parentNode,string sVal)
        {
            if (parentNode == null)
            {
                return null;
            }

            XmlNode createdNode = null;
            try
            {
                XmlComment commentNode = this.m_xmlDocument.CreateComment(this.Encode(sVal));
                createdNode = parentNode.AppendChild(commentNode);
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }

            return createdNode;
        }

        /// <summary>
        /// 创建Xml声明，并添加到文档的开头
        /// </summary>
        /// <param name="version">版本必须为“1.0”</param>
        /// <param name="encoding">如果这是 空引用或 String.Empty，则 Save 方法不在 XML 声明上写出编码方式属性，因此将使用默认的编码方式 UTF-8。</param>
        /// <param name="standalone">该值必须是“yes”或“no”.如果这是 空引用或 String.Empty，Save 方法不在 XML 声明上写出独立属性.</param>
        /// <returns>新增的XmlNode</returns>
        public XmlNode CreateXmlDeclaration(string version, string encoding, string standalone)
        {
            XmlNode createdNode = null;
            try
            {
                XmlDeclaration dec = this.m_xmlDocument.CreateXmlDeclaration(version, encoding, standalone);
                createdNode = this.m_xmlDocument.PrependChild(dec);
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }

            return createdNode;
        }

        /// <summary>
        /// 从Xml文档中删除指定的节点
        /// </summary>
        /// <param name="targetNode">待删除的节点</param>
        /// <returns>是否删除成功</returns>
        public bool DeleteNodeElement(XmlNode targetNode)
        {
            bool result = false;

            try
            {
                XmlNode xmlNode = this.RootNode.RemoveChild(targetNode);
                if (xmlNode != null)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }

            return result;
        }

        /// <summary>
        /// 修改指定元素的值
        /// </summary>
        /// <param name="targetNode"></param>
        /// <param name="sNewElementValue"></param>
        /// <returns></returns>
        public bool ModifyNodeElementValue(XmlNode targetNode,string sNewElementValue)
        {
            bool result = false;

            try
            {
                targetNode.InnerXml = this.Encode(sNewElementValue);
                result = true;
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }

            return result;
        }

        /// <summary>
        /// 给给定XmlElement增加一个属性
        /// </summary>
        /// <param name="targetElement">元素节点</param>
        /// <param name="sAttributeName">元素名</param>
        /// <param name="sAttributeValue">元素值</param>
        /// <returns>新建的节点</returns>
        public XmlAttribute CreateNodeAttribute(XmlElement targetElement, string sAttributeName, string sAttributeValue)
        {
            XmlAttribute newAttr = null;

            try
            {
                newAttr = this.m_xmlDocument.CreateAttribute(sAttributeName);
                targetElement.SetAttributeNode(newAttr);
                targetElement.SetAttribute(sAttributeName, this.Encode(sAttributeValue));
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }

            return newAttr;
        }

        /// <summary>
        /// 从给定节点上删除一个属性
        /// </summary>
        /// <param name="targetNode">给定节点</param>
        /// <param name="sAttributeName">待删除的属性</param>
        /// <returns>成功返回true,失败返回false</returns>
        public bool DeleteNodeAttribute(XmlNode targetNode,string sAttributeName)
        {
            bool result = false;

            try
            {
                XmlAttributeCollection attrColl = targetNode.Attributes;
                XmlAttribute xmlAttribute = attrColl.Remove((XmlAttribute)attrColl[sAttributeName, ""]);
                if (xmlAttribute != null)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }

            return result;
        }

        /// <summary>
        /// 将xml的架构写到指定的文件中
        /// </summary>
        /// <param name="sTargetFile">保存xml架构的文件</param>
        /// <returns>成功返回true;否则false</returns>
        public bool GenerateSchema(string sTargetFile)
        {
            bool result = false;

            try
            {
                DataSet data = new DataSet();
                data.ReadXml(new XmlNodeReader(this.RootNode), XmlReadMode.Auto);
                data.WriteXmlSchema(sTargetFile);
                result = true;
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }

            return result;
        }

        /// <summary>
        /// 创建Xml架构的字符串
        /// </summary>
        /// <returns>xml架构的字符串</returns>
        public string GenerateSchemaAsString()
        {
            string sSchemaXmlString = string.Empty;
            try
            {
                DataSet data = new DataSet();
                data.ReadXml(new XmlNodeReader(this.RootNode),XmlReadMode.Auto);
                string sTempFile = Path.GetTempFileName();
                data.WriteXmlSchema(sTempFile);
                using (StreamReader sr = new StreamReader(sTempFile))
                {
                    sSchemaXmlString = sr.ReadToEnd();
                }
                if (File.Exists(sTempFile) == true)
                {
                    File.Delete(sTempFile);
                }
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
                sSchemaXmlString = "<root><error>" + this.LastErrorMessage + "</error></root>";
            }

            return sSchemaXmlString;
        }

        /// <summary>
        /// 修改一个属性为新值
        /// </summary>
        /// <param name="targetNode">待修改的节点</param>
        /// <param name="sAttributeName">待修改节点的属性名</param>
        /// <param name="sNewAttributeValue">待修改的属性值</param>
        /// <returns>成功返回true;否则false</returns>
        public bool ModifyNodeAttributeValue(XmlNode targetNode, string sAttributeName, string sNewAttributeValue)
        {
            bool result = false;

            try
            {
                XmlAttributeCollection attrColl = targetNode.Attributes;
                XmlAttribute xmlAttribute = (XmlAttribute)attrColl[sAttributeName,string.Empty];
                xmlAttribute.Value = this.Encode(sNewAttributeValue);
                result = true;
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }

            return result;
        }



        #endregion

        #region 辅助方法

        /// <summary>
        /// 辅助方法，用来处理错误和异常。所有其它方法调用该方法来处理异常。
        /// </summary>
        /// <param name="e">待处理的异常</param>
        private void HandleException(Exception e)
        {
            m_sLastErrorMessage = e.Message;
            Debug.WriteLine("Generate Error from XMLHelper Class: "+m_sLastErrorMessage+" Stack Trace: "+e.StackTrace+" Source: "+e.Source);
        }

        /// <summary>
        /// 创建XPathNavigator游标，并移动到文档的根节点
        /// </summary>
        private void DoPostLoadCreateInit()
        {
            m_navigator = m_xmlDocument.CreateNavigator();
            this.MoveToRoot();
        }

        /// <summary>
        /// 使用ASCII从url地址中加载字符串
        /// </summary>
        /// <param name="sURL">要加载的源</param>
        /// <returns>加载到的字符串</returns>
        public string GetURLContent(string sURL)
        {
            string result = string.Empty;

            try
            {
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(sURL);
                HttpWebResponse webResp = (HttpWebResponse)webReq.GetResponse();
                //创建流
                using (StreamReader stream = new StreamReader(webResp.GetResponseStream(), Encoding.ASCII))
                {
                    result = stream.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                this.HandleException(e);
            }

            return result;
        }

        /// <summary>
        /// 将html格式字符串进行译码
        /// </summary>
        /// <param name="input">输入的字符串</param>
        /// <returns>译码后的字符串</returns>
        private string Decode(string input)
        {
            string output = input;
            output = Regex.Replace(output, "&amp;", "&");
            output = Regex.Replace(output, "&lt;", "<");
            output = Regex.Replace(output, "&gt;", ">");
            output = Regex.Replace(output, "&quot;", "\"");
            return output;
        }

        /// <summary>
        /// 对字符串进行编码
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns>Encode后的字符串</returns>
        private string Encode(string input)
        {
            if (input == null)
            {
                return string.Empty;
            }

            string output = input;
            //将input中的控制副替换为空格
            char ch;
            for (int i = 1; i < 32; i++)
            {
                if (10 != i && 13 != i)
                {
                    ch = (char)i;
                    output = output.Replace(ch, ' ');
                }
            }
            //将&替换为&amp;
            output = Regex.Replace(output, "&", "&amp;");
            output = Regex.Replace(output, "<", "&lt;");
            output = Regex.Replace(output, ">", "&gt;");
            output = Regex.Replace(output, "\"", "&quot;");

            return output;
        }

        #endregion

        #region 覆盖Object基类方法

        /// <summary>
        /// 覆盖基类ToString方法
        /// </summary>
        /// <returns>整个xml的内容</returns>
        public override string ToString()
        {
            return m_xmlDocument.OuterXml;
        }

        #endregion
    }
}
