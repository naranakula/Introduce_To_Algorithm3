using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.NamedPipe
{
    /// <summary>
    /// 单例命名管道服务
    /// 命名管道是一个FIFO的管道，只能一端写，一端读。虽然理论上管道是双向的，但尽量当作单向的来使用
    /// </summary>
    public class NamedPipeServer : IDisposable
    {
        #region Singleton

        /// <summary>
        /// 命名管道服务器单例
        /// </summary>
        private static volatile NamedPipeServer _instance;

        /// <summary>
        /// 构造函数
        /// </summary>
        private NamedPipeServer()
        {

        }

        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns></returns>
        public static NamedPipeServer GetInstance()
        {
            if (_instance != null)
            {
                return _instance;
            }

            if (_instance == null)
            {
                _instance = new NamedPipeServer();
            }

            return _instance;
        }

        #endregion

        #region Member

        /// <summary>
        /// 工作线程
        /// </summary>
        private Thread _workThread;

        /// <summary>
        /// 管道服务是否正在运行
        /// </summary>
        private volatile bool _isRunning;

        /// <summary>
        /// 管道名称
        /// </summary>
        private volatile string _pipeName;

        #endregion


        #region Methods

        /// <summary>
        /// 使用指定的管道名称来进行初始化
        /// </summary>
        /// <param name="pipeName">命名管道名称，如cmlu.pipe</param>
        /// <returns></returns>
        public bool Init(string pipeName)
        {
            try
            {
                this._pipeName = pipeName;

                _isRunning = true;

                _workThread = new Thread(Work);
                _workThread.Start(null);
                return true;
            }
            catch (Exception ex)
            {
                _isRunning = false;
                return false;
            }
        }

        /// <summary>
        /// 实际的线程工作
        /// </summary>
        /// <param name="obj"></param>
        private void Work(object obj)
        {
            while (_isRunning)
            {
                try
                {
                    //最多1个共享同一名称的服务器实例
                    using (NamedPipeServerStream server = new NamedPipeServerStream(_pipeName, PipeDirection.In, 1, PipeTransmissionMode.Byte))
                    {
                        server.WaitForConnection();

                        string line;
                        using (StreamReader sr = new StreamReader(server, Encoding.UTF8))
                        {
                            line = sr.ReadLine();
                        }

                        //对line进行处理
                        Console.WriteLine(line);


                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    Thread.Sleep(100);
                }
            }
        }

        #endregion


        public void Close()
        {
            _isRunning = false;

            try
            {
                Thread.Sleep(100);
                if (_workThread != null && _workThread.IsAlive)
                {
                    _workThread.Abort();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                _workThread = null;
            }
        }

        /// <summary>
        /// Dispose  接口
        /// </summary>
        public void Dispose()
        {
            Close();
        }
    }

}
