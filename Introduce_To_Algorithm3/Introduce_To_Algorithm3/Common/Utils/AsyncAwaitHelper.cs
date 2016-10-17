using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils
{
    /// <summary>
    /// 1)方法使用async修饰
    /// 2)方法约定以Async结尾，但不是必须的
    /// 3)返回类型可以是三种
    ///         a)Task<TResult> 方法 return TResult
    ///         b)Task 方法 没有返回值
    ///         c)void 方法是一个 async event handler 表示fire and forget
    /// 4)方法至少一个await语句，标志method can't continue until the awaited asynchronous operation is complete. 此时方法挂起，控制返回方法的调用方。 await并没有阻塞当前线程，它标记后面的代码为Continuation,并返回控制到async的调用方。The suspension of an async method at an await expression doesn't constitute an exit from the method, and finally blocks don’t run.
    /// 5）async和await底层使用的是ThreadPool
    /// 6）当到达await时，.net将方法所有的信息保存到栈，调用方继续拥有当前线程
    /// 7)Task有IsFaulted属性标记任务是否正常完成，当发生异常时，IsFaulted=true,await捕获异常，并重新在当前线程抛出，当返回void时，如果有SyncronizationContext(如winform和wpf)，异常传递给该context,否则传递给thread pool
    /// wpf或者winform使用SynchronizationContext,await之后的代码在UI线程上执行，
    /// An async method that has a void return type can’t be awaited, and the caller of a void-returning method can't catch any exceptions that the method throws.（async方法抛出的异常）
    /// An async method can't declare ref or out parameters, but the method can call methods that have such parameters.
    /// you should return a Task wherever possible, because a void-returning async method can't be awaited. Any caller of such a method must be able to continue to completion without waiting for the called async method to finish, and the caller must be independent of any values or exceptions that the async method generates.
    ///  If an exception occurs in an async method that returns a Task or Task<TResult>, the exception is stored in the returned task, and rethrown when the task is awaited. Therefore, make sure that any async method that can produce an exception has a return type of Task or Task<TResult> and that calls to the method are awaited.
    /// </summary>
    public static class AsyncAwaitHelper
    {
    }
}
