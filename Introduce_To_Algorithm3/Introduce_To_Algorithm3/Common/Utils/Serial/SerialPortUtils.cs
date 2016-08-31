using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;

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
        private SerialPort _serialPort;

        /// <summary>
        /// 底层的单例
        /// </summary>
        private static SerialPortUtils _instance;


        /// <summary>
        /// 构造函数
        /// </summary>
        private SerialPortUtils(string comName)
        {
        }

        private void SerialPortOnErrorReceived(object sender, SerialErrorReceivedEventArgs serialErrorReceivedEventArgs)
        {
            Console.WriteLine("Error");
        }

        private void SerialPortOnDataReceived(object sender, SerialDataReceivedEventArgs serialDataReceivedEventArgs)
        {
            Console.WriteLine("data received");
            SerialPort port = (SerialPort)sender;
            string s = port.ReadExisting();
            Console.WriteLine(s);
        }

        /// <summary>
        /// 获取底层实例
        /// </summary>
        /// <returns></returns>
        public static SerialPortUtils GetInstance(string comName)
        {
            if (_instance != null)
            {
                return _instance;
            }

            if (_instance == null)
            {
                _instance = new SerialPortUtils(comName);
            }

            return _instance;
        }
        #endregion

        #region 方法

        /// <summary>
        /// 打开
        /// </summary>
        public void Open()
        {
            _serialPort.Open();
        }

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
                _serialPort = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
                _serialPort.Encoding = Encoding.UTF8;

                _serialPort.DataReceived += SerialPortOnDataReceived;
                _serialPort.ErrorReceived += SerialPortOnErrorReceived;
                _serialPort.Open();

                return true;
            }
            catch (Exception ex)
            {
                NLogHelper.Error("打开串口失败：" + ex);
                return false;
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            _serialPort.Close();
        }

        public void Send(string s)
        {
            _serialPort.WriteLine(s);
        }

        /// <summary>
        /// 关闭串口
        /// </summary>
        public void Stop()
        {
            if (_serialPort != null)
            {
                try
                {
                    _serialPort.Close();
                }
                catch
                {

                }
            }

            _serialPort = null;
        }



        #endregion
    }
}
