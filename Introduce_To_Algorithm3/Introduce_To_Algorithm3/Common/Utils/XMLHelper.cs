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
    /// ����xml������ͨ����
    /// </summary>
    /// <remarks>
    /// ���ڼ��ء�����xml�ĵ�����ѯ���޸ġ����Ӻ�ɾ��xml�ڵ㡣
    /// </remarks>
    public class XMLHelper
    {
        #region �ֶ�

        /// <summary>
        /// ��ʾxml�ĵ�ʵ��
        /// </summary>
        private XmlDocument m_xmlDocument;

        /// <summary>
        /// Ϊ��λ�ͱ༭ XML �����ṩ�α�ģ��
        /// </summary>
        private XPathNavigator m_navigator;

        /// <summary>
        /// �������һ�δ�����Ϣ
        /// </summary>
        private string m_sLastErrorMessage;

        #endregion

        #region ����

        /// <summary>
        /// ����xml�ĵ��ĸ�Ԫ��
        /// </summary>
        public XmlElement RootNode
        {
            get
            {
                return m_xmlDocument.DocumentElement;
            }
        }

        /// <summary>
        /// ����xml�ĵ�ʵ��
        /// </summary>
        public XmlDocument Document
        {
            get
            {
                return m_xmlDocument;
            }
        }

        /// <summary>
        /// Ϊ��λ�ͱ༭xml�ĵ��ṩ���α�ʵ��
        /// </summary>
        public XPathNavigator Navigator
        {
            get
            {
                return m_navigator;
            }
        }

        /// <summary>
        /// ���һ�δ�����Ϣ
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

        #region �����ڲ���:��ʾ�ĵ����ص�Դ

        /// <summary>
        /// ��ʾ�ĵ����ص�Դ
        /// </summary>
        public enum LoadType
        {
            /// <summary>
            /// ���ַ����м���xml�ĵ�
            /// </summary>
            FromString,
            /// <summary>
            /// �ӱ��ش����ļ��м���xml�ĵ�
            /// </summary>
            FromLocalFile,
            /// <summary>
            /// ������url������xml�ĵ�
            /// </summary>
            FromURL
        }

        #endregion

        #region ���캯��

        /// <summary>
        /// ��������Ϣ��Ϊ�գ�������XmlDocument����
        /// </summary>
        public XMLHelper()
        {
            m_sLastErrorMessage = string.Empty;
            m_xmlDocument = new XmlDocument();
        }

        /// <summary>
        /// ��������Ϣ��Ϊ�գ���ʹ�ò�������XmlDocument����
        /// </summary>
        /// <param name="xmlDocument">XmlDocument����</param>
        public XMLHelper(XmlDocument xmlDocument)
        {
            m_sLastErrorMessage = string.Empty;
            
            //����XmlDocument����
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

        #region ����

        /// <summary>
        /// ����xml�ĵ��Ĵ�������Ĺ����о���ĺ�������
        /// </summary>
        /// <param name="sTargetXML">Ҫ�����ļ���Ŀ�ĵ�</param>
        /// <returns>true,����ɹ���false,����ʧ�ܡ�</returns>
        public delegate bool Save(string sTargetXML);

        /// <summary>
        /// ����xml��Ŀ���ļ���
        /// </summary>
        /// <param name="sTargetFileName">Ҫ�����xmlĿ�ĵ�</param>
        /// <returns>true,����ɹ���false,����ʧ�ܡ�</returns>
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
                //�쳣����
                this.HandleException(e);
            }
            return result;
        }

        /// <summary>
        /// �� XPathNavigator �ƶ������ڵ�
        /// </summary>
        /// <returns>true,��ʾ�ƶ��ɹ���false,��ʾ�ƶ�ʧ�ܡ�</returns>
        public bool MoveToRoot()
        {
            bool result = false;
            try
            {
                // �� XPathNavigator �ƶ�����ǰ�ڵ������ĸ��ڵ㡣
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
        /// ����xml�ַ��������ļ����ڴ���
        /// </summary>
        /// <param name="sourceXMLOrFile">Դxml</param>
        /// <param name="loadType">���صķ�ʽ����FromString,FromLocalFile,FromURL</param>
        /// <returns>true,���سɹ���false������ʧ�ܡ�</returns>
        public bool LoadXML(string sourceXMLOrFile,LoadType loadType)
        {
            bool result = false;

            try
            {
                switch (loadType)
                {
                    case LoadType.FromString:
                        //���ַ����м���xml�ļ�
                        this.m_xmlDocument.LoadXml(sourceXMLOrFile);
                        break;

                    case LoadType.FromLocalFile:
                        //�ӱ����ļ�����xml�ļ�
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
                //����XPathNavigator�α꣬���ƶ������ڵ�
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
        /// ʹ��XPath���ʽ��xml�ĵ�����ȡXmlNode
        /// </summary>
        /// <param name="xPathExpression">Ҫ��ѯ��XPath���ʽ</param>
        /// <returns>һ��װ��XmlNode������</returns>
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
        /// ��xml�ĵ��з��ص�һ������������XmlNode
        /// </summary>
        /// <param name="xPathExpression">Ҫ������XPath����</param>
        /// <returns>��һ������XPath������XmlNode�ڵ㣬����Ҳ���������null</returns>
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
        /// ��ָ���ڵ��ȡָ��������
        /// </summary>
        /// <param name="node">ָ���Ľڵ�</param>
        /// <param name="sAttributeName">ָ����������</param>
        /// <returns>����ֵ��������Ҳ���,����string.Empty��</returns>
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
        /// �Ӹ����ڵ��ȡ������������ֵ��Dictionary
        /// </summary>
        /// <param name="node">����XmlNode�ڵ�</param>
        /// <returns>������������ֵ��Dictionary</returns>
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
        /// �Ӹ����ڵ��ȡ������������int���͵�����ֵ
        /// </summary>
        /// <param name="node">����XmlNode�ڵ�</param>
        /// <param name="sAttributeName">������</param>
        /// <returns>int���͵�����ֵ</returns>
        public int GetAttributeInt32Value(XmlNode node, string sAttributeName)
        {
            string sVal = this.GetAttributeValue(node, sAttributeName);
            return sVal != "" ? Convert.ToInt32(sVal) : 0;
        }

        /// <summary>
        /// �Ӹ����ڵ��ȡ������������float���͵�����ֵ
        /// </summary>
        /// <param name="node">����XmlNode�ڵ�</param>
        /// <param name="sAttributeName">������</param>
        /// <returns>float���͵�����ֵ</returns>
        public float GetAttributeFloatValue(XmlNode node, string sAttributeName)
        {
            string sVal = this.GetAttributeValue(node, sAttributeName);
            return sVal != "" ? Convert.ToSingle(sVal) : 0f;
        }

        /// <summary>
        /// �Ӹ����ڵ��ȡ������������double���͵�����ֵ
        /// </summary>
        /// <param name="node">����XmlNode�ڵ�</param>
        /// <param name="sAttributeName">������</param>
        /// <returns>double���͵�����ֵ</returns>
        public double GetAttributeDoubleValue(XmlNode node, string sAttributeName)
        {
            string sVal = this.GetAttributeValue(node, sAttributeName);
            return sVal != "" ? Convert.ToDouble(sVal) : 0.0;
        }

        /// <summary>
        /// �Ӹ����ڵ��ȡ������������boolean���͵�����ֵ
        /// </summary>
        /// <param name="node">����XmlNode�ڵ�</param>
        /// <param name="sAttributeName">������</param>
        /// <returns>boolean���͵�����ֵ</returns>
        public bool GetAttributeBooleanValue(XmlNode node, string sAttributeName)
        {
            string sVal = this.GetAttributeValue(node, sAttributeName);
            return sVal != "" ? Convert.ToBoolean(sVal) : false;
        }

        /// <summary>
        /// ��ȡXmlElement�ڵ�InnerXml
        /// </summary>
        /// <param name="xmlNode">Ҫ��ȡ��XmlNode</param>
        /// <returns>InnerXml��ֵ</returns>
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
        /// ��ȡXmlElement�ڵ�int���͵�InnerXml
        /// </summary>
        /// <param name="xmlNode">Ҫ��ȡ��XmlNode</param>
        /// <returns>int���͵�InnerXml��ֵ</returns>
        public int GetElementInt32Value(XmlNode xmlNode)
        {
            string sVal = this.GetElementValue(xmlNode);
            return sVal != "" ? Convert.ToInt32(sVal) : 0;
        }

        /// <summary>
        /// ��ȡXmlElement�ڵ�float���͵�InnerXml
        /// </summary>
        /// <param name="xmlNode">Ҫ��ȡ��XmlNode</param>
        /// <returns>float���͵�InnerXml��ֵ</returns>
        public float GetElementFloatValue(XmlNode xmlNode)
        {
            string sVal = this.GetElementValue(xmlNode);
            return sVal != "" ? Convert.ToSingle(sVal) : 0f;
        }

        /// <summary>
        /// ��ȡXmlElement�ڵ�double���͵�InnerXml
        /// </summary>
        /// <param name="xmlNode">Ҫ��ȡ��XmlNode</param>
        /// <returns>double���͵�InnerXml��ֵ</returns>
        public double GetElementDoubleValue(XmlNode xmlNode)
        {
            string sVal = this.GetElementValue(xmlNode);
            return sVal != "" ? Convert.ToDouble(sVal) : 0.0;
        }

        /// <summary>
        /// ��ȡXmlElement��boolean���͵ĵ�InnerXml
        /// </summary>
        /// <param name="xmlNode">Ҫ��ȡ��XmlNode</param>
        /// <returns>boolean���͵�InnerXml��ֵ</returns>
        public bool GetElementBooleanValue(XmlNode xmlNode)
        {
            string sVal = this.GetElementValue(xmlNode);
            return sVal != "" ? Convert.ToBoolean(sVal) : false;
        }

        /// <summary>
        /// ��ȡ��ǰ���ڵ�ĵ�һ������ΪsElementName���ӽڵ��InnerXml
        /// </summary>
        /// <param name="parentNode">XmlNode���ڵ�</param>
        /// <param name="sElementName">Ҫ���ҵ�Ԫ����</param>
        /// <returns>���ڵ�ĵ�һ������ΪsElementName���ӽڵ��InnerXml</returns>
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
        /// ��ȡ��ǰ���ڵ�ĵ�һ������ΪsElementName���ӽڵ��int���͵�InnerXml
        /// </summary>
        /// <param name="parentNode">XmlNode���ڵ�</param>
        /// <param name="sElementName">Ҫ���ҵ�Ԫ����</param>
        /// <returns>���ڵ�ĵ�һ������ΪsElementName���ӽڵ��int���͵�InnerXml</returns>
        public int GetChildElementInt32Value(XmlNode parentNode, string sElementName)
        {
            string sVal = this.GetChildElementValue(parentNode, sElementName);
            return sVal != "" ? Convert.ToInt32(sVal) : 0;
        }

        /// <summary>
        /// ��ȡ��ǰ���ڵ�ĵ�һ������ΪsElementName���ӽڵ�float���͵�InnerXml
        /// </summary>
        /// <param name="parentNode">XmlNode���ڵ�</param>
        /// <param name="sElementName">Ҫ���ҵ�Ԫ����</param>
        /// <returns>���ڵ�ĵ�һ������ΪsElementName���ӽڵ�float���͵�InnerXml</returns>
        public float GetChildElementFloatValue(XmlNode parentNode, string sElementName)
        {
            string sVal = this.GetChildElementValue(parentNode, sElementName);
            return sVal != "" ? Convert.ToSingle(sVal) : 0f;
        }

        /// <summary>
        /// ��ȡ��ǰ���ڵ�ĵ�һ������ΪsElementName���ӽڵ��double���͵�InnerXml
        /// </summary>
        /// <param name="parentNode">XmlNode���ڵ�</param>
        /// <param name="sElementName">Ҫ���ҵ�Ԫ����</param>
        /// <returns>���ڵ�ĵ�һ������ΪsElementName���ӽڵ��double���͵�InnerXml</returns>
        public double GetChildElementDoubleValue(XmlNode parentNode, string sElementName)
        {
            string sVal = this.GetChildElementValue(parentNode, sElementName);
            return sVal != "" ? Convert.ToDouble(sVal) : 0.0;
        }

        /// <summary>
        /// ��ȡ��ǰ���ڵ�ĵ�һ������ΪsElementName���ӽڵ��bool���͵�InnerXml
        /// </summary>
        /// <param name="parentNode">XmlNode���ڵ�</param>
        /// <param name="sElementName">Ҫ���ҵ�Ԫ����</param>
        /// <returns>���ڵ�ĵ�һ������ΪsElementName���ӽڵ��bool���͵�InnerXml</returns>
        public bool GetChildElementBooleanValue(XmlNode parentNode, string sElementName)
        {
            string sVal = this.GetChildElementValue(parentNode, sElementName);
            return sVal != "" ? Convert.ToBoolean(sVal) : false;
        }

        /// <summary>
        /// �Ӹ��ڵ㷵�ص�һ��ƥ��Ԫ������XmlNode�ڵ�
        /// </summary>
        /// <param name="sElementName">��ƥ���Ԫ����</param>
        /// <returns>��һ��ƥ��Ԫ������XmlNode�ڵ�</returns>
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
        /// �Ӹ��ڵ㿪ʼ����ƥ��ָ�����ƵĽڵ㼯
        /// </summary>
        /// <param name="sElementName">��ƥ�������</param>
        /// <returns>ƥ��ָ�����ƵĽڵ㼯</returns>
        private XmlNodeList GetChildNodesFromRoot(string sElementName)
        {
            return this.m_xmlDocument.GetElementsByTagName(sElementName);
        }

        /// <summary>
        /// �������ڵ㣬�������ӽڵ��е�һ������ƥ��Ľڵ�
        /// </summary>
        /// <param name="parentNode">�������ĸ��ڵ�</param>
        /// <param name="sElementName">ƥ���Ԫ����</param>
        /// <returns>�ӽڵ㼯�е�һ������ƥ��Ľڵ�</returns>
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
        /// �������ڵ㣬�������ӽڵ�����������ƥ��Ľڵ㡣���������ݹ�������е��ӽڵ㡣
        /// </summary>
        /// <param name="parentNode">�������ĸ��ڵ�</param>
        /// <param name="sElementName">��ƥ���Ԫ����</param>
        /// <returns>�ӽڵ�����������ƥ��Ľڵ�</returns>
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
                        //�����ǰ�ڵ����ӽڵ㣬�ݹ�����ӽڵ�
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
        /// ʹ�ø���Ԫ������Ԫ��ֵ����һ��XmlElement������������ָ���Ľڵ���
        /// </summary>
        /// <param name="parentNode">�������½ڵ��ָ�����ڵ�</param>
        /// <param name="sElementName">��Ԫ�ص�Ԫ����</param>
        /// <param name="sElementValue">��Ԫ�ص�Ԫ��ֵ</param>
        /// <returns>�´�����XmlElement�ڵ�</returns>
        public XmlElement CreateNodeElement(XmlNode parentNode,string sElementName,string sElementValue)
        {
            XmlElement newElem = null;

            try
            {
                //������Ԫ��
                newElem = this.m_xmlDocument.CreateElement(sElementName);
                if (sElementValue != null)
                {
                    newElem.InnerXml = this.Encode(sElementValue);
                }
                else
                {
                    newElem.InnerXml = string.Empty;
                }

                //��ȡ���ڵ����ڵ��ĵ�
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
        /// �ڸ����ڵ�������һ��XmlComment
        /// </summary>
        /// <param name="parentNode">Ҫ����XmlComment�ĸ��ڵ�</param>
        /// <param name="sVal">XmlComment��ֵ</param>
        /// <returns>������Xml�ڵ�</returns>
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
        /// ����Xml����������ӵ��ĵ��Ŀ�ͷ
        /// </summary>
        /// <param name="version">�汾����Ϊ��1.0��</param>
        /// <param name="encoding">������� �����û� String.Empty���� Save �������� XML ������д�����뷽ʽ���ԣ���˽�ʹ��Ĭ�ϵı��뷽ʽ UTF-8��</param>
        /// <param name="standalone">��ֵ�����ǡ�yes����no��.������� �����û� String.Empty��Save �������� XML ������д����������.</param>
        /// <returns>������XmlNode</returns>
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
        /// ��Xml�ĵ���ɾ��ָ���Ľڵ�
        /// </summary>
        /// <param name="targetNode">��ɾ���Ľڵ�</param>
        /// <returns>�Ƿ�ɾ���ɹ�</returns>
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
        /// �޸�ָ��Ԫ�ص�ֵ
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
        /// ������XmlElement����һ������
        /// </summary>
        /// <param name="targetElement">Ԫ�ؽڵ�</param>
        /// <param name="sAttributeName">Ԫ����</param>
        /// <param name="sAttributeValue">Ԫ��ֵ</param>
        /// <returns>�½��Ľڵ�</returns>
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
        /// �Ӹ����ڵ���ɾ��һ������
        /// </summary>
        /// <param name="targetNode">�����ڵ�</param>
        /// <param name="sAttributeName">��ɾ��������</param>
        /// <returns>�ɹ�����true,ʧ�ܷ���false</returns>
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
        /// ��xml�ļܹ�д��ָ�����ļ���
        /// </summary>
        /// <param name="sTargetFile">����xml�ܹ����ļ�</param>
        /// <returns>�ɹ�����true;����false</returns>
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
        /// ����Xml�ܹ����ַ���
        /// </summary>
        /// <returns>xml�ܹ����ַ���</returns>
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
        /// �޸�һ������Ϊ��ֵ
        /// </summary>
        /// <param name="targetNode">���޸ĵĽڵ�</param>
        /// <param name="sAttributeName">���޸Ľڵ��������</param>
        /// <param name="sNewAttributeValue">���޸ĵ�����ֵ</param>
        /// <returns>�ɹ�����true;����false</returns>
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

        #region ��������

        /// <summary>
        /// �����������������������쳣�����������������ø÷����������쳣��
        /// </summary>
        /// <param name="e">��������쳣</param>
        private void HandleException(Exception e)
        {
            m_sLastErrorMessage = e.Message;
            Debug.WriteLine("Generate Error from XMLHelper Class: "+m_sLastErrorMessage+" Stack Trace: "+e.StackTrace+" Source: "+e.Source);
        }

        /// <summary>
        /// ����XPathNavigator�α꣬���ƶ����ĵ��ĸ��ڵ�
        /// </summary>
        private void DoPostLoadCreateInit()
        {
            m_navigator = m_xmlDocument.CreateNavigator();
            this.MoveToRoot();
        }

        /// <summary>
        /// ʹ��ASCII��url��ַ�м����ַ���
        /// </summary>
        /// <param name="sURL">Ҫ���ص�Դ</param>
        /// <returns>���ص����ַ���</returns>
        public string GetURLContent(string sURL)
        {
            string result = string.Empty;

            try
            {
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(sURL);
                HttpWebResponse webResp = (HttpWebResponse)webReq.GetResponse();
                //������
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
        /// ��html��ʽ�ַ�����������
        /// </summary>
        /// <param name="input">������ַ���</param>
        /// <returns>�������ַ���</returns>
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
        /// ���ַ������б���
        /// </summary>
        /// <param name="input">�����ַ���</param>
        /// <returns>Encode����ַ���</returns>
        private string Encode(string input)
        {
            if (input == null)
            {
                return string.Empty;
            }

            string output = input;
            //��input�еĿ��Ƹ��滻Ϊ�ո�
            char ch;
            for (int i = 1; i < 32; i++)
            {
                if (10 != i && 13 != i)
                {
                    ch = (char)i;
                    output = output.Replace(ch, ' ');
                }
            }
            //��&�滻Ϊ&amp;
            output = Regex.Replace(output, "&", "&amp;");
            output = Regex.Replace(output, "<", "&lt;");
            output = Regex.Replace(output, ">", "&gt;");
            output = Regex.Replace(output, "\"", "&quot;");

            return output;
        }

        #endregion

        #region ����Object���෽��

        /// <summary>
        /// ���ǻ���ToString����
        /// </summary>
        /// <returns>����xml������</returns>
        public override string ToString()
        {
            return m_xmlDocument.OuterXml;
        }

        #endregion
    }
}
