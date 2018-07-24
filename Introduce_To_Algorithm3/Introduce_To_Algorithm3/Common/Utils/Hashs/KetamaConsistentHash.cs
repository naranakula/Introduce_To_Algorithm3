using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.Hashs
{
    /// <summary>
    /// 一致性hash
    /// 当新增节点时，会造成扰动，但只会影响少量数据
    /// 
    /// https://www.cnblogs.com/daizhj/archive/2010/08/24/1807324.html
    /// </summary>
    public class KetamaConsistentHash
    {
        /// <summary>
        /// 红黑树
        /// </summary>
        private readonly SortedDictionary<long,string> _ketamaNodes = new SortedDictionary<long, string>();

        /// <summary>
        /// 节点的倍数，
        /// 该值变动会造成映射变动，因此声明为常数
        /// </summary>
        private const int NumOfNodeCopy = 128;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nodeList"></param>
        public KetamaConsistentHash(List<string> nodeList)
        {

            if (nodeList == null || nodeList.Count == 0)
            {
                throw new Exception("node list can not be empty");
            }

            int loopCount = NumOfNodeCopy / 8 ;

            if (NumOfNodeCopy % 8 != 0 || loopCount <= 0)
            {
                loopCount++;
            }


            foreach (var node in nodeList)
            {
                for (int i = 0; i < loopCount; i++)
                {
                    //计算 节点名 + 虚节点编号 的散列  sha256 共256位 32字节 
                    byte[] hashBytes = CryptoHelper.Sha256Hash(Encoding.UTF8.GetBytes(node + i));

                    //32个字节，每4个字节一个整数
                    for (int j = 0; j < 8; j++)
                    {
                        //实际上 nodePosition是一个整数
                        long nodePosition = (hashBytes[j * 4] & 0xff) | ((hashBytes[j * 4 + 1] & 0xff) << 8) |
                                           ((hashBytes[j * 4 + 2] & 0xff) << 16) | ((hashBytes[j * 4 + 3] & 0xff) << 24);

                        if (_ketamaNodes.ContainsKey(nodePosition))
                        {
                            //更新
                            _ketamaNodes[nodePosition] = node;
                        }
                        else
                        {
                            _ketamaNodes.Add(nodePosition, node);
                        }
                    }
                }
            }

        }


        /// <summary>
        /// 获取hash映射值
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public string GetNodeFromHash(int hash)
        {
            if (_ketamaNodes.ContainsKey(hash))
            {
                return _ketamaNodes[hash];
            }
            else
            {
                long key = _ketamaNodes.Keys.FirstOrDefault(r => r > hash);
                if (key == 0)
                {
                    //返回0有可能找到，也有可能找不到，找不到返回第一个
                    if (_ketamaNodes.ContainsKey(key))
                    {
                        return _ketamaNodes[key];
                    }
                    else
                    {
                        return _ketamaNodes[_ketamaNodes.Keys.First()];
                    }
                }
                else
                {
                    return _ketamaNodes[key];
                }
            }
        }

        /// <summary>
        /// 获取hash映射值
        /// </summary>
        /// <param name="strHash"></param>
        /// <returns></returns>
        public string GetNodeFromHash(string strHash)
        {
            return GetNodeFromHash(strHash.GetHashCode());
        }

    }
}
