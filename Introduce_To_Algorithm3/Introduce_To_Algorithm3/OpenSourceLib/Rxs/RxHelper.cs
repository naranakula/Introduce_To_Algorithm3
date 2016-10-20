using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.OpenSourceLib.Rxs
{
    /// <summary>
    /// RX 用于UI的响应式编程
    /// 1、不需要自己实现IObservable<T>和IObserver<T>接口
    /// 2、RX中订阅subscriptions设计为了Fire and forget,当OnCompleted或者OnError时，Observable自动取消了它的所有订阅
    /// 3、Cold observable:订阅时才产生数据，Hot observable一直产生数据，订阅时推送
    /// </summary>
    public static class RxHelper
    {
        /// <summary>
        /// 将list转换为IObservable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IObservable<T> ToObservable<T>(List<T> list)
        {
            return list.ToObservable();
        }

        /// <summary>
        /// 用于UI事件订阅
        /// </summary>
        public static void FromEvent()
        {
            
        }

    }
}
