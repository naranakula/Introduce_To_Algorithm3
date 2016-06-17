using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.Common.MachineLearning;
using Ninject;
using Ninject.Activation;
using Ninject.Modules;

namespace Introduce_To_Algorithm3.OpenSourceLib.Ninject
{
    /// <summary>
    /// Ninject帮助类，根据GitHub和Nuget上的下载量和Stars，Ninject比Autofac要好
    /// 
    /// Ninject是轻量级或者超轻量级的Dependency injector.
    /// 
    /// Ninject支持三类injection：
    /// 1、 Constructor Injection 这种方式使用如下的顺序选择实体类构造函数
    ///         a)如果构造函数有[Inject]属性，选择它。只能标记一个构造函数，否则抛出 Ninject will throw a NotSupportedException at runtime
    ///         b)均没有[Inject]属性， Ninject will select the one with the most parameters that Ninject understands how to resolve.
    ///         c)If no constructors are defined, Ninject will select the default parameterless constructor (assuming there is one).
    /// 2、Method injection
    /// 3、Property injection  没有 Feild injection
    ///         后两种注入要求标记[inject],在构造函数调用之后注入，注入的顺序是不确定的
    /// 
    /// 注：GC回收之前会自动调用Dispose
    /// Ninject有四种Object scope：
    ///     Transient:(短暂)  调用.InTransientScope(),默认，implicit ToSelf()来声明   实例每次请求创建一个实例，这是默认的scope，      Kernel不管理这种对象的生命周期，不调用Dispose函数
    ///     Singleton:   调用.InSingletonScope() or .ToConstant()来声明   只有一个实例，每次请求返回相同实例  Disposed when the Kernel is Disposed(这意味着Dispose在会多次调用（GC时也会调用）)
    ///     Thread:   .InThreadScope()  每个线程一个实例   当线程GC时Ninject调用Dispose
    ///     Request	.InRequestScope()	One instance of the type will be created for each Web Request. .	Disposable instances are Disposed at end of request processing
    /// 
    /// </summary>
    public static class NinjectHelper
    {
        /// <summary>
        /// 标准的Kernel
        /// </summary>
        private static readonly IKernel ninjectKernel = new StandardKernel();

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static NinjectHelper()
        {
            //if the type you’re resolving is a concrete type , Ninject will automatically create a default association via a mechanism called implicit self binding. 
            //绑定接口和接口实现类
            //ninjectKernel.Bind<IValueCalculater>().To<LinqValueCalculator>();
            //ninjectKernel.Bind<IValueCalculater>().To<LinqValueCalculator>().InThreadScope();
            //当代码请求一个接口时，须返回接口实现类的实例
            //ninjectKernel.Bind<接口>().To<接口实现类>();
            //Person类型自绑定到了单例模式
            //ninjectKernel.Bind<Person>().ToSelf().InSingletonScope();
            //.ToMethod(Func<IContext, T> method)
            //实质上binding是从接口类型到provider上，默认是StandardProvider，可以通过ToMethod定制对象初始化
            //ninjectKernel.Bind<IWeapon>().ToMethod(context => new Sword()).InTransientScope();
        }



        ///// <summary>
        ///// 获取类实例
        ///// </summary>
        ///// <returns></returns>
        //public static 接口 Get()
        //{
        //    return ninjectKernel.Get<接口>();
        //}

        ///// <summary>
        ///// 方法注入
        ///// </summary>
        ///// <param name="weapon"></param>
        //[Inject]
        //public void Arm(IWeapon weapon)
        //{
        //    this.weapon = weapon;
        //}

        ///// <summary>
        ///// 属性注入
        ///// </summary>
        //[Inject]
        //public IWeapon Weapon { private get; set; }

        //组织Bind到不同的Module中去
        //public class WarriorModule : NinjectModule
        //{
        //    public override void Load()
        //    {
        //        Bind<IWeapon>().To<Sword>();
        //        Bind<Samurai>().ToSelf().InSingletonScope();
        //    }
        //}


        //IKernel kernel = new StandardKernel(new Module1(), new Module2(), ...);
        //IKernel kernel = new StandardKernel(new WarriorModule());
        //Samurai warrior = kernel.Get<Samurai>();
    }
}
