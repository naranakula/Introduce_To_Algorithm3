using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;
using MathNet.Numerics;

namespace Introduce_To_Algorithm3.Common.Utils.sockets.SocketClients
{
    /// <summary>
    /// socket客户端代理
    /// </summary>
    public static class SocketClientProxy
    {
        #region 属性

        ///// <summary>
        ///// 是否断线重连
        ///// </summary>
        //private static volatile bool isFailOver = true;

        ///// <summary>
        ///// 是否断线重连
        ///// </summary>
        //public static bool IsFailOver
        //{
        //    get
        //    {
        //        lock (locker)
        //        {
        //            return isFailOver;
        //        }
        //    }
        //    set
        //    {
        //        lock (locker)
        //        {
        //            isFailOver = value;
        //        }
        //    }
        //}

        /// <summary>
        /// 是否已经连接
        /// </summary>
        public static bool IsConnected
        {
            get
            {
                lock (locker)
                {
                    if (socket == null)
                    {
                        return false;
                    }

                    if (_isDisconnect)
                    {
                        //确信断开了连接
                        return false;
                    }

                    try
                    {
                        //根据上一次Send或者Receive操作判断是否是否连接
                        // Connected 属性反映截止到最近的操作的连接的状态
                        //如果服务器意外断电，网线被拔掉，服务器意外杀死，是无法检测到问题的(连接断开) 远程主机 shuts down or close可以检测到
                        bool isConnected = socket.Connected;
                        if (!isConnected)
                        {
                            return false;
                        }
                    }
                    catch(Exception ex)
                    {
                        NLogHelper.Error("根据Socket.Connected判断是否连接异常："+ex);
                        return false;
                    }

                    //注：这种方式是相对精确的，需要根据lastUpdateTime进行二次确认
                    //因为 Poll 如果服务器意外断电，网线被拔掉，服务器意外杀死，Poll Available是无法检测到问题的(连接断开), 远程主机 shuts down or close可以检测到
                    return true;
                }
            }
           
        }

        /// <summary>
        /// 是否已经断开了连接，用于确信断开了连接
        /// </summary>
        private static volatile bool _isDisconnect = true;

        /// <summary>
        /// 服务器ip
        /// </summary>
        private static readonly string serverIp = ConfigUtils.GetString("ServerIp");

        /// <summary>
        /// 服务器端口
        /// </summary>
        private static readonly int serverPort = ConfigUtils.GetInteger("ServerPort");

        /// <summary>
        /// 锁
        /// </summary>
        private static object locker = new object();

        /// <summary>
        /// 底层socket
        /// </summary>
        private static volatile Socket socket;

        /// <summary>
        /// 最近一次通信时间
        /// </summary>
        private static DateTime? lastSendRecvTime = null;

        /// <summary>
        /// 发送queue
        /// </summary>
        private static readonly BlockingCollection<MessageItem> SendBlockingQueue = new BlockingCollection<MessageItem>();

        /// <summary>
        /// 接受的byte list
        /// RecvByteList只在一个线程中使用
        /// </summary>
        private static readonly List<byte> RecvByteList = new List<byte>();

        /// <summary>
        /// 接收解析后的消息
        /// </summary>
        private static readonly BlockingCollection<MessageItem> RecvBlockingQueue = new BlockingCollection<MessageItem>();

        /// <summary>
        /// 发送的Thread
        /// </summary>
        private static volatile Thread sendThread = null;

        /// <summary>
        /// 接收的thread
        /// </summary>
        private static volatile Thread recvThread = null;

        /// <summary>
        /// 消息处理的thread
        /// </summary>
        private static volatile Thread msgHandlerThread = null;

        /// <summary>
        /// 通信使用的编码
        /// </summary>
        private const string EncodingStr = "UTF-8";

        /// <summary>
        /// 发送接收超时时间 ms
        /// </summary>
        private const int SendRecvTimeout = 5000;

        /// <summary>
        /// 是否正在运行，标记是否多线程应该退出
        /// </summary>
        private static volatile bool isRunning = false;

        /// <summary>
        /// 是否正在运行，标记是否多线程应该退出
        /// </summary>
        public static bool IsRunning
        {
            get
            {
                lock (locker)
                {
                    return isRunning;
                }
            }
            set
            {
                lock (locker)
                {
                    isRunning = false;
                }
            }
        }

        #endregion

        #region 接收 发送 处理线程

        /// <summary>
        /// 初始化接收发送 处理线程
        /// </summary>
        private static void InitThread()
        {
            if (sendThread == null)
            {
                IsRunning = true;
                sendThread = new Thread(SendCallBack);
                sendThread.Start();
            }

            if (recvThread == null)
            {
                recvThread = new Thread(RecvCallBack);
                recvThread.Start();
            }

            if (msgHandlerThread == null)
            {
                msgHandlerThread = new Thread(MsgHandleCallback);
                msgHandlerThread.Start();
            }
        }

        /// <summary>
        /// 消息处理的线程
        /// </summary>
        /// <param name="obj"></param>
        private static void MsgHandleCallback(object obj)
        {
            MessageItem item = null;
            while (isRunning)
            {
                if (RecvBlockingQueue.TryTake(out item, 1000))
                {
                    if (item == null || string.IsNullOrWhiteSpace(item.Message))
                    {
                        continue;
                    }

                    try
                    {
                        string message = item.Message.Trim();
                        NLogHelper.Info("开始处理消息：" + message);
                        //TODO: 处理消息

                        #region 处理消息


                        #endregion

                    }
                    catch (Exception ex)
                    {
                        NLogHelper.Info("处理消息失败：" + ex);
                    }
                }
            }
        }

        /// <summary>
        /// 接收消息的线程
        /// </summary>
        /// <param name="obj"></param>
        private static void RecvCallBack(object obj)
        {
            byte[] buffer = new byte[1024*512];
            //单条消息的最大长度
            int maxByteInList = 8 * 1024 * 1024;//8M 声明为全局常量
            while (isRunning)
            {
                try
                {
                    if (!IsConnected)
                    {

                        if (RecvByteList.Count > 0)
                        {
                            //避免受到上一次连接的不完整消息
                            RecvByteList.Clear();
                        }
                        Thread.Sleep(1200);
                        NLogHelper.Info("检测到socket没有连接");
                        //不能使用break
                        continue;
                    }

                    /*
true 如果 Listen 已调用和一个连接处于挂起状态。
- 或 -
true 如果数据是可供读取;
- 或 -
true 如果连接已关闭、 重置，或者终止，则返回，
否则，返回 false。
                     */
                    //500毫秒
                    ////Poll 如果服务器意外断电，网线被拔掉，服务器意外杀死，是无法检测到问题的(连接断开)，指定时间过后返回false
                    if (socket.Poll(500000, SelectMode.SelectRead))
                    {
                        int count = socket.Receive(buffer, 0, buffer.Length, SocketFlags.None);

                        if (count <= 0)
                        {
                            //连接断开
                            _isDisconnect = true;
                            Thread.Sleep(500);
                            continue;
                        }
                        lastSendRecvTime = DateTime.Now;
                        _isDisconnect = false;
                        for (int i = 0; i < count; i++)
                        {
                            RecvByteList.Add(buffer[i]);
                        }

                        //TODO:开始解析数据

                        #region 解析数据

                        //消息头部开始位置
                        int headPos = -1;
                        //消息尾部结束位置
                        int tailPos = -1;
                        //[headPos,tailPos)之间,是消息内容
                        string header = "head"; //根据实际情况设置消息头尾，消息头尾包含在消息内
                        string tail = "tail";
                        byte[] headerBytes = Encoding.GetEncoding(EncodingStr).GetBytes(header);
                        byte[] tailBytes = Encoding.GetEncoding(EncodingStr).GetBytes(tail);

                        while (true)
                        {
                            headPos = Position(RecvByteList, headerBytes);
                            if (headPos >= 0)
                            {
                                //找到头部,开始查找尾部位置
                                tailPos = Position(RecvByteList, tailBytes, headPos + headerBytes.Length);
                                if (tailPos >= 0)
                                {

                                    //找到尾部位置
                                    tailPos = tailPos + tailBytes.Length; //不含  尾部位置+1
                                    byte[] itemBuffer = new byte[tailPos - headPos];
                                    RecvByteList.CopyTo(headPos, itemBuffer, 0, itemBuffer.Length);
                                    //移除list中的数据
                                    RecvByteList.RemoveRange(0, tailPos);
                                    string recvStr = Encoding.GetEncoding(EncodingStr).GetString(itemBuffer);
                                    RecvBlockingQueue.Add(new MessageItem() {Message = recvStr});
                                }
                                else
                                {
                                    //没有找到结束位置
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }


                        if (RecvByteList.Count > maxByteInList)
                        {
                            RecvByteList.Clear();
                        }

                        #endregion
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                    
                }
                catch (Exception ex)
                {
                    NLogHelper.Error("接收线程错误："+ex);
                    Thread.Sleep(2000);
                }
            }
        }

        /// <summary>
        /// 发送消息的线程
        /// </summary>
        /// <param name="obj"></param>
        private static void SendCallBack(object obj)
        {
            MessageItem item = null;
            while (isRunning)
            {
                //阻塞1s
                if (SendBlockingQueue.TryTake(out item, 1000))
                {
                    if (item == null || string.IsNullOrWhiteSpace(item.Message))
                    {
                        //空消息
                        continue;
                    }

                    try
                    {
                        NLogHelper.Info("开始发送消息"+item.Message);
                        byte[] buffer = Encoding.GetEncoding(EncodingStr).GetBytes(item.Message);
                        socket.Send(buffer, 0, buffer.Length, SocketFlags.None);
                        lastSendRecvTime = DateTime.Now;
                        _isDisconnect = false;
                    }
                    catch (Exception ex)
                    {
                        NLogHelper.Error("发送消息失败："+ex);
                    }
                }
            }
        }

        /// <summary>
        /// 查找buffer第一次在list中出现的位置
        /// 如果没有返回-1
        /// </summary>
        /// <param name="list"></param>
        /// <param name="buffer">不能为null或者空</param>
        /// <param name="startPos">在list中开始查找的位置</param>
        /// <returns></returns>
        private static int Position(List<byte> list, byte[] buffer, int startPos = 0)
        {
            for (int i = startPos; i < list.Count; i++)
            {
                bool finded = true;
                int maxIndex = i + buffer.Length - 1;//buffer最后一个位置
                for (int j = 0; j < buffer.Length; j++)
                {
                    if (maxIndex >= list.Count)
                    {
                        //超出索引位置
                        finded = false;
                        break;
                    }

                    int curIndex = i + j;//buffer当前位置
                    if (list[curIndex] != buffer[j])
                    {
                        finded = false;
                    }
                }

                if (finded)
                {
                    return i;
                }
            }

            return -1;
        }


        #endregion

        #region Start & Stop

        /// <summary>
        /// 开始连接
        /// </summary>
        public static void Start()
        {
            try
            {
                Stop();

                NLogHelper.Info("开始建立到{0}:{1}的连接".FormatWith(serverIp,serverPort));

                socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);

                socket.SendTimeout = socket.ReceiveTimeout = SendRecvTimeout;

                socket.Connect(IPAddress.Parse(serverIp),serverPort);
                lastSendRecvTime = DateTime.Now;
                _isDisconnect = false;
                //初始化接收\发送多线程
                InitThread();
            }
            catch (Exception ex)
            {
                NLogHelper.Error("连接失败："+ex);
                Stop();
            }
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public static void Stop()
        {
            if (socket != null)
            {
                try
                {
                    socket.Shutdown(SocketShutdown.Both);
                }
                catch (Exception ex)
                {
                    NLogHelper.Error("Socket的ShutDown失败："+ex);
                }

                try
                {
                    socket.Close();
                }
                catch (Exception ex)
                {
                    NLogHelper.Error("Socket的Close失败："+ex);
                }
                //连接断开
                _isDisconnect = true;
                socket = null;
            }
        }

        #endregion
    }
}
