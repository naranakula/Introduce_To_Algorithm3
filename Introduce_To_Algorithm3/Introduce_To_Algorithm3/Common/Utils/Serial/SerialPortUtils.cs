using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;
using System.Threading;

namespace Introduce_To_Algorithm3.Common.Utils.Serial
{
    /// <summary>
    /// 串口通信工具类
    /// </summary>
    public class SerialPortUtils
    {
        /// <summary>
        /// 获取当前机器的串口
        /// </summary>
        /// <returns></returns>
        public static string[] GetPorts()
        {
            return SerialPort.GetPortNames();
        }

        #region 单例方法

        /// <summary>
        /// 私有的串口
        /// </summary>
        private volatile SerialPort _serialPort;

        /// <summary>
        /// 是否串口已经连接
        /// </summary>
        private volatile bool isAlive = false;

        /// <summary>
        /// 底层的单例
        /// </summary>
        private static volatile SerialPortUtils _instance;

        /// <summary>
        /// 锁
        /// </summary>
        private object locker = new object();

        /// <summary>
        /// 底层的读取buffer  4k的buffer
        /// </summary>
        private byte[] readBuffer = new byte[4096];

        /// <summary>
        /// 8M最大，超多8M，丢弃消息
        /// </summary>
        private const int MaxBufferSizeBeforeAbandon = 1024 * 1024*8;

        
        /// <summary>
        /// byteList列表
        /// </summary>
        private List<byte> readList = new List<byte>();

        /// <summary>
        /// 上一次串口通信时间
        /// </summary>
        private DateTime lastCommunicateTime = DateTime.Now.AddDays(-1);

        #region 串口参数

        /// <summary>
        /// 串口名称
        /// </summary>
        private string portName;

        /// <summary>
        /// 波特率
        /// </summary>
        private int baudRate;

        /// <summary>
        /// 数据位  多少位一个字节 默认是8
        /// </summary>
        private int dataBits;

        /// <summary>
        /// a value that enables the Data Terminal Ready (DTR) signal during serial communication
        /// new SerialPort()时默认是false 
        /// </summary>
        private bool dtrEnable;

        /// <summary>
        /// 传输文本时的编码，new SerialPort()时默认是 ASCIIEncoding 
        /// </summary>
        private Encoding encoding;

        /// <summary>
        /// 奇偶校验位 new SerialPort()时默认是None
        /// </summary>
        private Parity parity;

        /// <summary>
        /// 停止位
        /// </summary>
        private StopBits stopBits;

        /// <summary>
        /// the handshaking protocol for serial port transmission of data new SerialPort()时默认是None
        /// </summary>
        private Handshake handshake;

        /// <summary>
        /// 读写超时时间毫秒
        /// </summary>
        private int readWriteTimeout = 5000;

        /// <summary>
        /// enable Request to Transmit (RTS)   new SerialPort()时默认是false
        /// </summary>
        private bool rtsEnable;

        /// <summary>
        /// The number of bytes in the internal input buffer before a DataReceived event is fired. The default is 1.
        /// </summary>
        private int receivedBytesThreshold;
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        private SerialPortUtils()
        {
        }

        /// <summary>
        /// 初始化串口参数
        /// </summary>
        /// <param name="portName">串口名称</param>
        /// <param name="baudRate">波特率</param>
        /// <param name="dataBits">数据位  多少位一个字节 默认是8</param>
        /// <param name="dtrEnable">a value that enables the Data Terminal Ready (DTR) signal during serial communication</param>
        /// <param name="rtsEnable">enable Request to Transmit (RTS) </param>
        /// <param name="handshake">the handshaking protocol for serial port transmission of data new SerialPort()时默认是None</param>
        /// <param name="encoding">传输文本时的编码，new SerialPort()时默认是 ASCIIEncoding </param>
        /// <param name="parity">奇偶校验位 new SerialPort()时默认是None</param>
        /// <param name="stopBits">停止位</param>
        /// <param name="readWriteTimeout">读写超时时间毫秒</param>
        /// <param name="receivedBytesThreshold">The number of bytes in the internal input buffer before a DataReceived event is fired. The default is 1.</param>
        public void InitSerialPort(string portName, int baudRate, int dataBits = 8, bool dtrEnable = true,bool rtsEnable = true, Handshake handshake = Handshake.RequestToSend, String encoding="utf-8", Parity parity = Parity.None,StopBits stopBits = StopBits.One,int readWriteTimeout = 5000,int receivedBytesThreshold =1)
        {
            this.portName = portName;
            this.baudRate = baudRate;
            this.dataBits = dataBits;
            this.dtrEnable = dtrEnable;
            this.rtsEnable = rtsEnable;
            this.handshake = handshake;
            this.encoding = String.Equals("utf-8",encoding,StringComparison.InvariantCultureIgnoreCase)?Encoding.UTF8: Encoding.GetEncoding(encoding);
            this.parity = parity;
            this.stopBits = stopBits;
            this.readWriteTimeout = readWriteTimeout;
            this.receivedBytesThreshold = receivedBytesThreshold;
        }


        /// <summary>
        /// 不是在UI线程上执行的
        /// 串口出错的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="serialErrorReceivedEventArgs"></param>
        private void SerialPortOnErrorReceived(object sender, SerialErrorReceivedEventArgs serialErrorReceivedEventArgs)
        {
            isAlive = false;
            NLogHelper.Warn($"串口出错事件ErrorReceived,事件类型:{serialErrorReceivedEventArgs?.EventType}");
        }

        /// <summary>
        /// 不是在UI线程上执行的
        /// 接收到数据的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="serialDataReceivedEventArgs"></param>
        private void SerialPortOnDataReceived(object sender, SerialDataReceivedEventArgs serialDataReceivedEventArgs)
        {
            try
            {
                lastCommunicateTime = DateTime.Now;

                SerialPort port = sender as SerialPort;

                if (port == null)
                {
                    NLogHelper.Warn("参数sender转换为SerialPort为null，可能出错了");
                    return;
                }

                int readCount = 0;
                lock (locker)
                {
                    //No bytes were available to read. 将会抛出TimeoutException,真的会等待readTimeout时间
                    //Thread.Sleep(50);//等待一会read可以读取所有的数据
                    readCount = port.Read(readBuffer, 0, readBuffer.Length);
                    if (readCount > 0)
                    {
                        for (int i = 0; i < readCount; i++)
                        {
                            readList.Add(readBuffer[i]);
                        }
                    }

                    //解析字符列表
                    ParseBytes(readList);
                }
                
            }
            catch(Exception ex)
            {
                NLogHelper.Error($"接收数据DataReceived出错:{ex}");
            }
        }

        /// <summary>
        /// 解析字符列表
        /// </summary>
        /// <param name="readList"></param>
        private void ParseBytes(List<byte> readList)
        {
            if(readList == null || readList.Count == 0)
            {
                return;
            }

            #region 实际的解析消息

            String s = Encoding.UTF8.GetString(readList.ToArray());
            NLogHelper.Info(s);
            readList.Clear();

            #endregion


            if (readList.Count >= MaxBufferSizeBeforeAbandon)
            {
                //超出数据，清空
                readList.Clear();
            }

        }

        /// <summary>
        /// 获取底层实例
        /// </summary>
        /// <returns></returns>
        public static SerialPortUtils GetInstance()
        {
            if (_instance != null)
            {
                return _instance;
            }

            if (_instance == null)
            {
                _instance = new SerialPortUtils();
            }

            return _instance;
        }

        #endregion

        #region 方法
        
        /// <summary>
        /// 启动
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            if (_serialPort != null)
            {
                Stop();
            }

            try
            {
                _serialPort = new SerialPort(portName);//, 9600, Parity.None, 8, StopBits.One);
                _serialPort.BaudRate = baudRate;
                _serialPort.Parity = parity;
                _serialPort.StopBits = stopBits;
                _serialPort.DataBits = dataBits;
                _serialPort.Handshake = handshake;
                _serialPort.ReceivedBytesThreshold = receivedBytesThreshold;//
                _serialPort.DtrEnable = dtrEnable;
                _serialPort.RtsEnable = rtsEnable;
                _serialPort.ReadTimeout = readWriteTimeout;
                _serialPort.WriteTimeout = readWriteTimeout;
                _serialPort.Encoding = encoding;
                
                //数据接收事件
                _serialPort.DataReceived += SerialPortOnDataReceived;
                //端口出错事件
                _serialPort.ErrorReceived += SerialPortOnErrorReceived;
                //非数据信号事件
                _serialPort.PinChanged += SerialPort_PinChanged;
                //串口dispose事件
                _serialPort.Disposed += SerialPort_Disposed;
                
                _serialPort.Open();

                lastCommunicateTime = DateTime.Now;
                isAlive = true;
                NLogHelper.Info($"打开串口{portName}成功");
                return true;
            }
            catch (Exception ex)
            {
                NLogHelper.Error("打开串口失败：" + ex);

                isAlive = false;
                return false;
            }
        }

        /// <summary>
        /// 串口Dispose事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SerialPort_Disposed(object sender, EventArgs e)
        {
            isAlive = false;
            NLogHelper.Warn("串口Disposed事件");
        }

        /// <summary>
        /// 非数据信号事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SerialPort_PinChanged(object sender, SerialPinChangedEventArgs e)
        {
            NLogHelper.Warn($"串口发生非数据信号事件PinChanged,EventType={e?.EventType}");
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            Stop();
        }

        /// <summary>
        /// 是否存活
        /// </summary>
        /// <returns></returns>
        public bool IsAlive()
        {
            return isAlive;
        }

        /// <summary>
        /// 写字符数组
        /// </summary>
        /// <param name="buffer"></param>
        public bool Send(byte[] buffer,Action<Exception> exceptionHandler = null)
        {
            try
            {
                _serialPort.Write(buffer, 0, buffer.Length);
                lastCommunicateTime = DateTime.Now;
                return true;
            }
            catch (InvalidOperationException invalidEx)
            {
                isAlive = false;
                if (exceptionHandler != null)
                {
                    exceptionHandler(invalidEx);
                }

                return false;
            }
            catch(Exception ex)
            {
                if(exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }

                return false;
            }
        }

        /// <summary>
        /// 关闭串口
        /// </summary>
        public void Stop()
        {
            try
            {
                isAlive = false;
                if (_serialPort != null)
                {
                    _serialPort.Close();
                    _serialPort = null;
                    NLogHelper.Info("串口关闭成功");
                }
            }
            catch (Exception ex)
            {
                NLogHelper.Error($"关闭串口失败:{ex}");
            }
        }
        
        #endregion
    }
}
