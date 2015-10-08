using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Introduce_To_Algorithm3.OpenSourceLib.Autofac
{
    /// <summary>
    /// Autofac帮助类
    /// </summary>
    public static class AutofacHelper
    {
        /// <summary>
        /// Builder容器for组件注册
        /// </summary>
        private static ContainerBuilder builder = new ContainerBuilder();

        /// <summary>
        /// 注册类型容器
        /// </summary>
        private static IContainer _container;

        #region 静态构造函数，用作初始化

        static AutofacHelper()
        {
            //注册类型
            //通过接口暴露类型,并制定使用的构造函数
            builder.RegisterType<ConsoleOutput>().UsingConstructor().As<IOutput>();
            //通过接口或者本身暴露类型
            builder.RegisterType<TodayWriter>().AsSelf().As<IDateWriter>();
            // Register instances of objects you create...
            var output = new StringWriter();
            builder.RegisterInstance(output).As<TextWriter>();

            // Register expressions that execute to create objects...
            builder.Register(c => new StringReader("")).As<TextReader>();
            //组件注册类型
            _container = builder.Build();
        }

        #endregion


        public static void Do(Action<ILifetimeScope> action)
        {
            using (ILifetimeScope scope = _container.BeginLifetimeScope())
            {
                action(scope);
                //var writer = scope.Resolve<IDateWriter>();
                //writer.WriteDate();
            }
        }


        #region 测试程序 可以忽略


        public static void Test()
        {
            Do(new Action<ILifetimeScope>(scope =>
            {
                var writer = scope.Resolve<IDateWriter>();
                writer.WriteDate();
            }));
        }


        public interface IOutput
        {
            void Write(string content);
        }

        public class ConsoleOutput : IOutput
        {
            public void Write(string content)
            {
                Console.WriteLine(content);
            }
        }

        public interface IDateWriter
        {
            void WriteDate();
        }

        public class TodayWriter : IDateWriter
        {
            private IOutput _output;

            public TodayWriter(IOutput output)
            {
                _output = output;
            }

            public void WriteDate()
            {
                _output.Write(DateTime.Today.ToString());
            }
        }

        #endregion
    }
}
