﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;

namespace Introduce_To_Algorithm3.OpenSourceLib.ActiveMq
{
    /// <summary>
    /// MQ监视的Consumer
    /// 外部调用
    /// MQMessageHandler.Init();//初始化消息处理
    /// MQMonitorTimer.Start();//开启接收消息
    /// 
    /// 关闭
    /// MQMonitorTimer.Stop();
    /// MQMessageHandler.Stop();
    /// </summary>
    public static class MQMonitorTimer
    {
        /// <summary>
        /// 底层的timer
        /// </summary>
        private static volatile Timer _mqTimer = null;

        /// <summary>
        /// 定时器是否正在运行
        /// </summary>
        private static bool _isRunning = false;

        /// <summary>
        /// 是否第一次启动mQ
        /// </summary>
        private static volatile bool _isFirstTimeToStartMq = true;

        /// <summary>
        /// 锁
        /// </summary>
        private static readonly object locker = new object();

        /// <summary>
        /// 通过定时器开启MQ，不要直接开启mQ
        /// </summary>
        public static void Start()
        {
            if (_mqTimer == null)
            {
                //执行周期17s
                _mqTimer = new Timer(new TimerCallback(MQTimerCallBack),null,500,19571);
            }
        }

        /// <summary>
        /// MQ回调
        /// </summary>
        /// <param name="state"></param>
        private static void MQTimerCallBack(object state)
        {
            lock (locker)
            {
                if (_isRunning)
                {
                    return;
                }
                _isRunning = true;
            }
            
            try
            {
                if (!MQConsumer.IsAlive())
                {
                    Thread.Sleep(5500);
                    //延迟5s，使其能够自动恢复
                    if (!MQConsumer.IsAlive())
                    {
                        if (_isFirstTimeToStartMq)
                        {
                            _isFirstTimeToStartMq = false;

                            NLogHelper.Debug("首次尝试启动MQConsumer");
                            MQConsumer.InitConsumer();
                        }
                        else
                        {
                            NLogHelper.Warn("MQ异常,尝试重启 MQConsumer");
                            MQConsumer.InitConsumer();
                        }
                    }
                }
                else
                {
                    NLogHelper.Trace("检测MQ连接正常");
                }
            }
            catch (Exception ex)
            {
                NLogHelper.Error("MQ定时器异常:"+ex);
            }
            finally
            {
                lock (locker)
                {
                    _isRunning = false;
                }
            }
        }

        /// <summary>
        /// 关闭定时器
        /// </summary>
        public static void Stop()
        {
            try
            {
                if (_mqTimer != null)
                {
                    _mqTimer.Dispose();
                    _mqTimer = null;
                }
            }
            catch { }

            //关闭MQ
            MQConsumer.CloseConsumer();
        }
    }

}
