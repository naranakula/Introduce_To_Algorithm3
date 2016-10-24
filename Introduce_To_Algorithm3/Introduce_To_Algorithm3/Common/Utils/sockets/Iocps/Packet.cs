using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.sockets.Iocps
{
    /// <summary>
    /// 数据包
    /// </summary>
    public sealed class Packet
    {
        /// <summary>
        /// 底层数据包
        /// </summary>
        private byte[] m_packet;

        /// <summary>
        /// 数据包大小
        /// </summary>
        private int m_packetSize;

        /// <summary>
        /// 数据包的开始索引
        /// </summary>
        private int m_packetOffset;

        /// <summary>
        /// flag whether memory is allocated in this object or not
        /// </summary>
        private bool m_isAllocated;

        /// <summary>
        /// lock锁
        /// </summary>
        private object m_packetLock = new object();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <param name="offset">数据包的起始位置</param>
        /// <param name="byteSize">数据包的大小</param>
        /// <param name="shouldAllocate">是否分配内存</param>
        public Packet(byte[] packet = null, int offset = 0, int byteSize = 0, bool shouldAllocate = true)
        {
            m_packet = null;
            m_packetSize = 0;
            m_isAllocated = shouldAllocate;

            if (shouldAllocate)
            {
                //分配内存
                if (byteSize > 0)
                {
                    m_packet = new byte[byteSize];
                    if (packet != null)
                    {
                        Array.Copy(packet,offset,m_packet,0,byteSize);
                    }

                    m_packetOffset = 0;
                    m_packetOffset = byteSize;
                }
            }
            else
            {
                m_packet = packet;
                m_packetOffset = offset;
                m_packetSize = byteSize;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="b"></param>
        public Packet(Packet b)
        {
            lock (b.m_packetLock)
            {
                m_packet = null;
                if (b.m_isAllocated)
                {
                    if (b.m_packetSize > 0)
                    {
                        m_packet = new byte[b.m_packetSize];
                        Array.Copy(b.m_packet, b.m_packetOffset, m_packet, 0, b.m_packetSize);
                    }
                }
                else
                {
                    m_packet = b.m_packet;
                }

                m_packetSize = b.m_packetSize;
                m_packetOffset = b.m_packetOffset;
                m_isAllocated = b.m_isAllocated;
            }
        }

        /// <summary>
        /// 数据包的字节数
        /// </summary>
        public int PacketByteSize
        {
            get
            {
                lock (m_packetLock)
                {
                    return m_packetSize;
                }
            }
        }

        /// <summary>
        /// 数据包的初始位置
        /// </summary>
        public int PacketOffset
        {
            get
            {
                lock (m_packetLock)
                {
                    return m_packetOffset;
                }
            }
        }

        /// <summary>
        /// 底层已经分配的数据字节数
        /// </summary>
        public int AllocatedByteSize
        {
            get
            {
                lock (m_packetLock)
                {
                    if (m_packet != null)
                    {
                        return m_packet.Length;
                    }

                    return 0;
                }
            }
        }

        /// <summary>
        /// the flag whether allocated memory or not 
        /// </summary>
        public bool IsAllocated
        {
            get { return m_isAllocated; }
        }

        /// <summary>
        /// 底层的字节数组
        /// </summary>
        public byte[] PacketRaw
        {
            get
            {
                lock (m_packetLock)
                {
                    return m_packet;
                }
            }
        }

        /// <summary>
        /// 设置数据包
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <param name="offset">数据包起始位置</param>
        /// <param name="packetByteSize">数据包字节数</param>
        public void SetPacket(byte[] packet, int offset, int packetByteSize)
        {
            lock (m_packetLock)
            {
                if (m_isAllocated)
                {
                    if (m_packet != null)
                    {
                        if (m_packet.Length >= packetByteSize)
                        {
                            Array.Copy(packet,offset,m_packet,0,packetByteSize);
                            m_packetSize = packetByteSize;
                            m_packetOffset = 0;
                            return;
                        }
                    }

                    m_packet = new byte[packetByteSize];
                    Array.Copy(packet,offset,m_packet,0,packetByteSize);
                    m_packetSize = packetByteSize;
                    m_packetOffset = 0;
                }
                else
                {
                    m_packet = packet;
                    m_packetSize = packetByteSize;
                    m_packetOffset = offset;
                }
            }
        }

    }
}
