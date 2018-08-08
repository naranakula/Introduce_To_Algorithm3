using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.OpenSourceLib.Utils;

namespace Introduce_To_Algorithm3.OpenSourceLib.RabbitMq.ProductReady.Consumers
{

    /// <summary>
    /// MQMonitorTimer.Start();//开启接收消息
    /// 
    /// 关闭
    /// MQMonitorTimer.Stop();
    /// 
    /// 消息处理的关闭放在了Timer.Stop()
    /// </summary>
    public static class RMqMonitorTimer
    {
        /// <summary>
        /// 底层的timer
        /// </summary>
        private static volatile Timer _mqTimer = null;

        /// <summary>
        /// 定时器是否正在运行
        /// </summary>
        private static volatile bool _isRunning = false;

        /// <summary>
        /// 是否第一次启动mQ
        /// </summary>
        private static volatile bool _isFirstTimeToStartMq = true;

        /// <summary>
        /// 重启次数
        /// </summary>
        private static volatile int _restartCount = 0;

        /// <summary>
        /// 锁
        /// </summary>
        private static readonly object Slocker = new object();

        /// <summary>
        /// 通过定时器开启MQ，不要直接开启mQ
        /// </summary>
        public static void Start()
        {
            if (_mqTimer == null)
            {
                Random rand = new Random();
                //执行周期17s
                _mqTimer = new Timer(new TimerCallback(MqTimerCallBack), null, rand.Next(100,500), 29000+rand.Next(1,1071));
            }
        }

        /// <summary>
        /// MQ回调
        /// </summary>
        /// <param name="state"></param>
        private static void MqTimerCallBack(object state)
        {
            lock (Slocker)
            {
                if (_isRunning)
                {
                    return;
                }
                _isRunning = true;
            }

            try
            {
                if (!LongRunRabbitConsumer.IsAlive())
                {
                    if (_isFirstTimeToStartMq)
                    {
                        _isFirstTimeToStartMq = false;

                        NLogHelper.Debug("首次尝试启动MQConsumer");
                        LongRunRabbitConsumer.StartConsumer(ex =>
                        {
                            NLogHelper.Error($"开启MQ失败:{ex}");
                        });
                    }
                    else
                    {
                        _restartCount++;
                        if (_restartCount > 10000000)
                        {
                            _restartCount = 10000;
                        }
                        NLogHelper.Warn($"MQ异常,第{_restartCount}次尝试重启 MQConsumer");
                        LongRunRabbitConsumer.StartConsumer(ex =>
                        {
                            NLogHelper.Error($"开启MQ失败:{ex}");
                        });
                    }
                }
                else
                {
                    NLogHelper.Trace("检测MQ连接正常");
                }
            }
            catch (Exception ex)
            {
                NLogHelper.Error("MQ定时器异常:" + ex);
            }
            finally
            {
                lock (Slocker)
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
            catch
            {
                // ignored
            }

            //关闭MQ
            LongRunRabbitConsumer.StopConsumer();

            //关闭消息处理
            RMqMessageHandler.Stop();
        }
    }


}
