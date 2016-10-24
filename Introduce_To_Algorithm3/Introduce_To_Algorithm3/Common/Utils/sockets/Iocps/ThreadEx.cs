using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common.Logging.Configuration;

namespace Introduce_To_Algorithm3.Common.Utils.sockets.Iocps
{
    /// <summary>
    /// 类实现基本的线程操作
    /// </summary>
    public class ThreadEx
    {
        /// <summary>
        /// thread handle
        /// </summary>
        private Thread m_threadHandle;

        /// <summary>
        /// Thread Priority
        /// 线程优先级
        /// </summary>
        private ThreadPriority m_threadPriority;

        /// <summary>
        /// Parent thread handle
        /// </summary>
        private Thread m_parentThreadHandle;

        /// <summary>
        /// Thread status
        /// </summary>
        private ThreadStatus m_status;

        /// <summary>
        /// thread func
        /// </summary>
        private Action m_threadFunc;

        /// <summary>
        /// 参数化的thread func
        /// </summary>
        private Action<object> m_threadParameterizedFunc;

        /// <summary>
        /// Parameter object for parameterized function
        /// </summary>
        private object m_parameter;

        /// <summary>
        /// lock
        /// </summary>
        private object m_threadLock = new object();

        /// <summary>
        /// exit code
        /// </summary>
        private long m_exitCode;
    }

    /// <summary>
    /// 线程状态
    /// </summary>
    public enum ThreadStatus
    {
        /// <summary>
        /// The thread is started and running
        /// </summary>
        STARTED = 0,
        /// <summary>
        /// The thread is suspended
        /// </summary>
        SUSPENDED,
        /// <summary>
        /// The thread is terminated
        /// </summary>
        TERMINATED,
    }

}
