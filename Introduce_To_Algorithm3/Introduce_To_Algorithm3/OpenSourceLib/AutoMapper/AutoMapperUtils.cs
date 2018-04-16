using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apache.NMS.ActiveMQ;
using AutoMapper;

namespace Introduce_To_Algorithm3.OpenSourceLib.AutoMapper
{
    /// <summary>
    /// Map工具
    /// 将同名属性映射，创建目标对象，如果Src中存在同名的，将Src的同名属性值赋给dest,不存在Src同名属性，则不作处理
    /// https://github.com/AutoMapper/AutoMapper
    /// http://automapper.readthedocs.io/en/latest/Getting-started.html
    /// 
    /// AutoMapper的CreateMap不是多线程安全的，但Map是多线程安全的
    /// </summary>
    public class AutoMapperUtils
    {
        /// <summary>
        /// 初始化
        /// 在程序启动时初始化一次
        /// </summary>
        public static bool InitAutoMapperUtils(Action<Exception> exceptionHandler = null)
        {
            //createmap的顺序是不重要的
            try
            {
                Mapper.Initialize(cfg =>
                {
                    #region 初始化代码
                    //经测试，所有的类型转换必须先CreateMap，即使是从A到A的转换  从A到B和从B到A是两个转换
                    cfg.CreateMap<A, B>();
                    //默认是MemberList.Destination，表示Check that all destination members are mapped 所有目标成员都必须映射
                    //MemberList.None表示不检查  MemberList.Source表示所有的源被影射，默认时MemberList.Destination
                    //A是源类型,B是目标类型
                    //PreserveReferences()是默认的配置
                    //MapFrom和ResolveUsing的区别是MapFrom内部有Nullcheck（可忽略的性能区别）,除此之外没有任何区别 ResolveUsing可以定制的代码更多
                    var imap = cfg.CreateMap<A, B>(MemberList.None).PreserveReferences().ForMember(b=>b.a,opt=>opt.MapFrom(a=>a.a/*对某些字段进行定制的转换*/)).ForMember(a=>a.a,opt=>opt.ResolveUsing(a=>a.a/*对某些字段进行定制转换*/)).ForMember(b=>b.a,opt=>opt.Ignore()/*对某些字段不转换*/);

                    
                    #region 循环引用
                    // Self-referential mapping指定循环引用的递归深度
                    //cfg.CreateMap<Category, CategoryDto>().MaxDepth(3);
                    //PreserveReferences()是默认的配置
                    // Circular references between users and groups循环引用保持引用，而不是map
                    //cfg.CreateMap<User, UserDto>().PreserveReferences();
                    #endregion
                    var imap2 = cfg.CreateMap<AA, BB>(MemberList.None);

                    #endregion
                });

                return true;
            }
            catch (Exception e)
            {
                exceptionHandler?.Invoke(e);
                return false;
            }
        }

        /// <summary>
        /// 将Source对象映射到target上
        /// 多线程安全
        /// </summary>
        /// <typeparam name="Source"></typeparam>
        /// <typeparam name="Target"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        public static Target Map<Source, Target>(Source src) where Source:class 
        {
            if (src == null)
            {
                return default(Target);
            }
            return Mapper.Map<Source, Target>(src);
        }

        /// <summary>
        /// 将Source对象映射到target上
        /// 多线程安全
        /// </summary>
        /// <typeparam name="Target"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        public static Target Map2<Target>(object src)
        {
            if (src == null)
            {
                return default(Target);
            }

            return Mapper.Map<Target>(src);
        }

        public static void Test(string[] args)
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<A, B>();
                cfg.CreateMap<AA, BB>();
            });
            A ats = new A() { a = "Hello world", list = new List<AA>() };
            ats.list.Add(new AA() { aa = "agfaa" });
            B bts = Mapper.Map<A, B>(ats);
            Console.WriteLine(bts);
        }
      
    }

    class A
    {
        public string a { get; set; }
        public List<AA> list { get; set; }
    }

    class AA
    {
        public string aa { get; set; }
    }

    class B
    {
        public string a { get; set; }
        public List<BB> list { get; set; }
    }

    class BB
    {
        public string aa { get; set; }
    }

}
