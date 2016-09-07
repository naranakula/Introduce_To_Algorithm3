using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils
{
    public static class TaskHelper
    {
        #region Task
        /**
         * 开始异步的任务
         * 
         * async await 是基于编译器的功能，编译时转换为了状态机，await将代码分为了块，之前的块执行完了，转到调用方，同时开启线程执行await部分，执行完了，转回到调用方执行
         * async修饰void Task Task<T>
         */
        public static Task Start(Action action)
        {
            Task task = Task.Factory.StartNew(action);
            return task;
        }

        /// <summary>
        /// 开始一个任务，并传入一个参数
        /// </summary>
        /// <param name="action"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static Task Start(Action<Object> action, object state)
        {
            Task task = Task.Factory.StartNew(action, state);
            return task;
        }

        /// <summary>
        /// 开始一个长期运行的任务
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static Task StartLongRun(Action action)
        {
            Task task = new Task(action, TaskCreationOptions.LongRunning);
            task.Start();
            return task;
        }

        /// <summary>
        /// 开始一个长期运行的任务
        /// </summary>
        /// <param name="action"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static Task StartLongRun(Action<Object> action, Object state)
        {
            Task task = new Task(action, state, TaskCreationOptions.LongRunning);
            task.Start();
            return task;
        }

        /**
         * 开始同步运行任务,如果目标计划程序不支持当前线程上运行此任务，则在计划程序上计划执行该任务，当前线程会阻塞，直到该任务完成执行。
         */
        public static void RunSynchronous(Action action)
        {
            Task task = new Task(action);
            task.RunSynchronously();
        }

        /// <summary>
        /// 开始一个同步任务
        /// </summary>
        /// <param name="action"></param>
        /// <param name="state"></param>
        public static void RunSynchronous(Action<object> action, Object state)
        {
            Task task = new Task(action, state);
            task.RunSynchronously();
        }


        /**
         * 开始一个异步的任务
         */
        public static Task Start<TResult>(Func<TResult> func)
        {
            return Task.Factory.StartNew(func);
        }

        /**
         * 开始一个同步任务，并获取返回值
         */
        public static TResult RunSynchronous<TResult>(Func<TResult> func)
        {
            Task<TResult> task = new Task<TResult>(func);
            task.RunSynchronously();
            return task.Result;
        }
        #endregion

        #region parallel  函数不返回，知道执行完
        /**
         * 并行运行一个函数多次，但并行的顺序不定
         * 包含第一个索引，不包含第二个索引
         */
        public static ParallelLoopResult ParallelFor(int from, int to, Action<int> action)
        {
            return Parallel.For(from, to, action);
        }

        /**
         * 对集合中的每一项，执行一个动作
         */
        public static ParallelLoopResult ParallelForEach<T>(IEnumerable<T> source, Action<T> action)
        {
            return Parallel.ForEach(source, action);
        }


        /**
         * 并行运行多个方法
         */
        public static void ParallelInvoke(params  Action[] actions)
        {
            Parallel.Invoke(actions);
        }
        #endregion

        #region 取消任务

        /// <summary>
        /// 释放资源代理
        /// </summary>
        /// <param name="state"></param>
        public static Action<Object> Release;

        /// <summary>
        /// 进行实际的工作
        /// </summary>
        /// <param name="state"></param>
        public static Action<Object> WordLoad;

        /// <summary>
        /// 取消源
        /// </summary>
        private static CancellationTokenSource tokenSource = new CancellationTokenSource();

        /// <summary>
        /// 取消符号
        /// </summary>
        private static CancellationToken token = tokenSource.Token;

        /// <summary>
        /// 传送取消请求
        /// </summary>
        public static void Cancel()
        {
            tokenSource.Cancel();
        }

        /// <summary>
        /// 开启一个异步任务,
        /// 传递信息有两种方式，一种是定义一个类，类的方法作为action，类的成员作为信息
        /// 另一种是就是本方法使用的这种。
        /// </summary>
        public static void Run(object obj)
        {
            Task task = new Task((state) =>
                                     {
                                         while (true)
                                         {
                                             if (token.IsCancellationRequested)
                                             {
                                                 if (Release != null) Release(state);
                                                 throw new OperationCanceledException(token);
                                             }
                                             else
                                             {
                                                 if (WordLoad != null) WordLoad(state);
                                             }
                                         }
                                     },obj, token, TaskCreationOptions.LongRunning);
            task.Start();
        }

        /// <summary>
        /// 当取消时回调
        /// </summary>
        /// <param name="action"></param>
        public static void Register(Action action)
        {
            token.Register(action);
        }

        /// <summary>
        /// 开始一个任务
        /// </summary>
        public static void RunWithoutRealse(object obj)
        {
            Task task = new Task((state) =>
            {
                while (true)
                {
                    token.ThrowIfCancellationRequested();
                    if (WordLoad != null) WordLoad(state);

                }
            },obj, token, TaskCreationOptions.LongRunning);
            task.Start();
        }

        #endregion

        #region 处理未捕获的异常

        /// <summary>
        /// 处理未处理的异常
        /// </summary>
        /// <param name="handler"></param>
        public static void UnobservedException(EventHandler<UnobservedTaskExceptionEventArgs> handler)
        {
            TaskScheduler.UnobservedTaskException += handler;
        }

        /// <summary>
        /// 处理未处理的异常
        /// </summary>
        /// <param name="handler"></param>
        public static void UnobservedException()
        {
            TaskScheduler.UnobservedTaskException += (sender, args) =>
                                                         {
                                                             //set exception handled
                                                             args.SetObserved();
                                                         };
        }

        #endregion
    }
}
