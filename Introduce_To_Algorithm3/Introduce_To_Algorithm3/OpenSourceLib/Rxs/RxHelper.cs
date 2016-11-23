using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Introduce_To_Algorithm3.OpenSourceLib.Rxs
{
    /// <summary>
    /// RX 用于UI的响应式编程
    /// 1、不需要自己实现IObservable<T>和IObserver<T>接口
    /// 2、RX中订阅subscriptions设计为了Fire and forget,当OnCompleted或者OnError时，Observable自动取消了它的所有订阅
    /// 3、Cold observable:订阅时才产生数据，每次订阅接收到相同的从头开始的数据，Hot observable一直产生数据，订阅时推送，接收到最新的数据
    /// 
    /// https://msdn.microsoft.com/en-us/library/hh242981(v=vs.103).aspx
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
        /// 强IEnumerable<T>转换为IObservable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static IObservable<T> ToObservable<T>(IEnumerable<T> enumerable)
        {
            return enumerable.ToObservable();
        }

        /// <summary>
        /// 生成数据
        /// </summary>
        /// <returns></returns>
        public static IObservable<long> Generate()
        {
            return Observable.Generate<int,long>(0/*初始的状态*/, i => true/*根据状态判断生成结束，返回FALSE，表示生成结束*/, 
                i => 0/*根据当前状态生成下一个状态*/, i => 0/*根据状态生成结果*/, i => TimeSpan.FromSeconds(1)/*生成时间周期*/);
        } 


        #region 样例

        /// <summary>
        /// 订阅样例
        /// </summary>
        public static void SubscribeEx()
        {
            IObservable<int> source = Observable.Range(1, 5);
            IDisposable subsciption = source.Subscribe(x => Console.WriteLine("OnNext:{0}", x),
                ex => Console.WriteLine("OnError:{0}", ex.Message),
                () => Console.WriteLine("OnCompleted"));
            IDisposable subsciption2 = source.Subscribe(x => Console.WriteLine("OnNext:{0}", x),
                ex => Console.WriteLine("OnError:{0}", ex.Message),
                () => Console.WriteLine("OnCompleted"));
        }

        /// <summary>
        /// 订阅2
        /// </summary>
        public static void SubscribeEx2()
        {
            IObservable<int> source = Observable.Range(1, 10);
            IObserver<int> obsvr = Observer.Create<int>(
                x => Console.WriteLine("OnNext:{0}", x), ex=>Console.WriteLine("OnError:{0}",ex),
                ()=>Console.WriteLine("OnCompleted"));
            IDisposable subscription = source.Subscribe(obsvr);
            Console.ReadLine();
        }

        /// <summary>
        /// 订阅3
        /// </summary>
        public static void SubscribeEx3()
        {
            IObservable<long> source = Observable.Interval(TimeSpan.FromSeconds(1));

            IDisposable subscription1 = source.Subscribe(x => Console.WriteLine("Observer1:OnNext:{0}", x), 
               ex=>Console.WriteLine("Observer1:OnError:{0}",ex.Message),
               ()=>Console.WriteLine("Observer1:OnCompleted"));

            IDisposable subscription2 = source.Subscribe(x => Console.WriteLine("Observer2:OnNext:{0}", x),
               ex => Console.WriteLine("Observer2:OnError:{0}", ex.Message),
               () => Console.WriteLine("Observer2:OnCompleted"));
            Console.ReadLine();
        }


        #endregion

        /// <summary>
        /// 将其转换为hot Observable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="observable"></param>
        /// <returns></returns>
        public static IConnectableObservable<T> ToHot<T>(IObservable<T> observable)
        {
            var hot = observable.Publish();
            //只有调用Connect之后，hot才开始链接observable，发送数据
            hot.Connect();
            return hot;
        }


        /// <summary>
        /// 用于UI事件订阅
        /// </summary>
        public static void FromEvent()
        {
            var lbl = new Label();
            var frm = new Form();
            frm.Controls.Add(lbl);

            IObservable<EventPattern<MouseEventArgs>> move = Observable.FromEventPattern<MouseEventArgs>(frm,
                "MouseMove");
            move.Subscribe(evt =>
            {
                lbl.Text = evt.EventArgs.Location.ToString();
            });

            Application.Run(frm);
        }

        #region Linq

        /// <summary>
        /// 连接，将second连接到first之后，second在first之后开始推送数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static IObservable<T> Concat<T>(IObservable<T> first, IObservable<T> second)
        {
            var result = first.Concat(second);

            return result;
        }

        /// <summary>
        /// wo sequences are active at the same time and values are pushed out as they occur in the sources. The resultant sequence only completes when the last source sequence has finished pushing values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static IObservable<T> Merge<T>(IObservable<T> first, IObservable<T> second)
        {
            var result = first.Merge(second);

            return result;
        }


        /// <summary>
        /// 对Rx使用linq
        /// </summary>
        public static void LinqEx1()
        {
            var seqNum = Observable.Range(1, 50);
            var seqStr = from n in seqNum where n < 12 select new string('*', n);
            seqStr.Subscribe(str => Console.WriteLine(str));

        }

        /// <summary>
        /// 对rx使用Linq2
        /// </summary>
        public static void LinqEx2()
        {
            var frm = new Form();

            IObservable<EventPattern<MouseEventArgs>> move = Observable.FromEventPattern<MouseEventArgs>(frm,
                "MouseMove");
            IObservable<System.Drawing.Point> points = from evt in move where evt.EventArgs.Location.X>10 select evt.EventArgs.Location;
            points.Subscribe(pos => Console.WriteLine("mouse at " + pos));
            Application.Run(frm);

        }

        /// <summary>
        /// 对Rx使用linq
        /// </summary>
        public static void LinqEx3()
        {
            var seq = Observable.Interval(TimeSpan.FromSeconds(1));
            var bufSeq = seq.Buffer(TimeSpan.FromSeconds(3));
            bufSeq.Subscribe(val => Console.WriteLine(val.Last()));
            Console.ReadLine();
        }

        #endregion

        #region Subject

        /*
         * 订阅
         * 
         */

        #endregion




    }
}
