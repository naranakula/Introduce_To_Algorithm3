using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.ConcurrentCollections
{
    /// <summary>
    /// 异步action
    /// </summary>
    public static class AsyncActionUtils
    {
        /// <summary>
        /// 底层的action
        /// </summary>
        private static readonly BlockingQueueEx<Tuple<Action<object>, object>> SBlockingQueueEx = new BlockingQueueEx<Tuple<Action<object>, object>>(singleDataHandler: SingleDataHandler, maxNumberDataInQueue: 4096);


        /// <summary>
        /// 添加action
        /// </summary>
        /// <param name="tupleAction"></param>
        public static void AddAction(Tuple<Action<object>, object> tupleAction)
        {
            SBlockingQueueEx.Add(tupleAction);
        }

        /// <summary>
        /// 添加action
        /// </summary>
        /// <param name="action"></param>
        public static void AddAction(Action action)
        {
            SBlockingQueueEx.Add(new Tuple<Action<object>, object>(obj=> { action?.Invoke(); },null));
        }

        /// <summary>
        /// 停止
        /// </summary>
        public static void Stop()
        {
            SBlockingQueueEx.Stop();
        }

        /// <summary>
        /// 启动实际上什么也没有做，如果不调用该方法，将延迟初始化静态变量
        /// 初始化静态变量
        /// </summary>
        public static void Start()
        {
            
        }



        /// <summary>
        /// 单个消息处理
        /// </summary>
        /// <param name="tupleAction"></param>
        private static void SingleDataHandler(Tuple<Action<object>, object> tupleAction)
        {
            tupleAction?.Item1?.Invoke(tupleAction?.Item2);
        }
    }
}
