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
        private static readonly BlockingQueueEx<ActionTuple> SBlockingQueueEx = new BlockingQueueEx<ActionTuple>(singleDataHandler: SingleDataHandler, maxNumberDataInQueue: 4096);

        /// <summary>
        /// 添加action
        /// </summary>
        /// <param name="action"></param>
        /// <param name="obj"></param>
        public static void AddAction(Action<object> action,object obj)
        {
            if (action == null)
            {
                return;
            }

            SBlockingQueueEx.Add(new ActionTuple(action,obj));
        }

        /// <summary>
        /// 添加action
        /// </summary>
        /// <param name="action"></param>
        public static void AddAction(Action action)
        {
            if (action == null)
            {
                return;
            }

            SBlockingQueueEx.Add(new ActionTuple(obj=> { action?.Invoke(); },null));
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
        public static bool Start(Action<Exception> exceptionHandler = null)
        {
            return true;
        }



        /// <summary>
        /// 单个消息处理
        /// </summary>
        /// <param name="tupleAction"></param>
        private static void SingleDataHandler(ActionTuple tupleAction)
        {
            tupleAction?.Action?.Invoke(tupleAction?.Parameter);
        }



        /// <summary>
        /// action tuple  内部类
        /// </summary>
        private class ActionTuple
        {
            /// <summary>
            /// 锁
            /// </summary>
            private readonly object _locker = new object();

            /// <summary>
            /// action回调
            /// </summary>
            private volatile Action<object> _action = null;

            /// <summary>
            /// 参数
            /// </summary>
            private volatile object _parameter = null;

            /// <summary>
            /// action 回调
            /// </summary>
            public Action<object> Action
            {
                get
                {
                    lock (_locker)
                    {
                        return _action;
                    }
                }
            }

            /// <summary>
            /// 参数
            /// </summary>
            public Object Parameter
            {
                get
                {
                    lock (_locker)
                    {
                        return _parameter;
                    }
                }
            }



            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="action"></param>
            /// <param name="obj"></param>
            public ActionTuple(Action<object> action, object obj)
            {
                this._action = action;
                this._parameter = obj;
            }


        }


    }



}
