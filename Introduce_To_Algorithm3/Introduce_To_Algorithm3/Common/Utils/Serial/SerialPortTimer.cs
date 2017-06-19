using Introduce_To_Algorithm3.OpenSourceLib.Utils;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.Serial
{
    class SerialPortTimer
    {
        /// <summary>
        /// 底层的timer
        /// </summary>
        private static volatile Timer portTimer = null;

        /// <summary>
        /// 定时器是否正在运行
        /// </summary>
        private static bool isRunning = false;

        /// <summary>
        /// 锁
        /// </summary>
        private static object locker = new object();

        /// <summary>
        /// 串口工具实例
        /// </summary>
        private static volatile SerialPortUtils portUtils = SerialPortUtils.GetInstance();

        /// <summary>
        /// 通过定时器开启串口
        /// </summary>
        public static void Start()
        {
            if (portTimer == null)
            {
                portTimer = new Timer(new TimerCallback(SerialPortCallBack), null, 300, 18000);
            }
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
        public void InitSerialPort(string portName, int baudRate, int dataBits = 8, bool dtrEnable = true, bool rtsEnable = true, Handshake handshake = Handshake.RequestToSend, String encoding = "utf-8", Parity parity = Parity.None, StopBits stopBits = StopBits.One, int readWriteTimeout = 5000, int receivedBytesThreshold = 1)
        {
            portUtils.InitSerialPort(portName, baudRate, dataBits, dtrEnable, rtsEnable, handshake, encoding, parity, stopBits, readWriteTimeout, readWriteTimeout);
        }

        /// <summary>
        /// 定时器回调
        /// </summary>
        /// <param name="state"></param>
        private static void SerialPortCallBack(object state)
        {
            lock (locker)
            {
                if (isRunning)
                {
                    return;
                }
                isRunning = true;
            }

            try
            {
                if (!portUtils.IsAlive())
                {
                    portUtils.Start();
                }
            }
            catch (Exception ex)
            {
                NLogHelper.Error("串口定时器异常：" + ex);
            }
            finally
            {
                lock (locker)
                {
                    isRunning = false;
                }
            }
        }


        /// <summary>
        /// 关闭定时器
        /// </summary>
        public static void Stop()
        {
            if (portTimer != null)
            {
                portTimer.Dispose();
                portTimer = null;
            }

            portUtils.Stop();
        }
    }
}
