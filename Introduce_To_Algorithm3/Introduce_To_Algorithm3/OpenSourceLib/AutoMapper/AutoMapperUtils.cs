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
    /// </summary>
    public class AutoMapperUtils
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static AutoMapperUtils()
        {
            //初始化Map
            //Mapper.CreateMap<CalendarEvent, CalendarEventForm>().ForMember(dest => dest.EventDate, opt => opt.MapFrom(src => src.Date.Date)).ForMember(dest => dest.EventHour, opt => opt.MapFrom(src => src.Date.Hour)).ForMember(dest => dest.EventMinute, opt => opt.MapFrom(src => src.Date.Minute));
            
            //忽略某些字段
            //.ForMember(r => r.Passenger, opt => opt.Ignore());
            //createmap的顺序是不重要的
        }

        /// <summary>
        /// 将Source对象映射到target上
        /// </summary>
        /// <typeparam name="Source"></typeparam>
        /// <typeparam name="Target"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        public static Target Map<Source, Target>(Source src)
        {
            return Mapper.Map<Source, Target>(src);
        }

        
        public static void Test(string[] args)
        {
            Mapper.CreateMap<A, B>();
            Mapper.CreateMap<AA, BB>();
            A ats = new A() { a = "Hello world", list = new List<AA>() };
            ats.list.Add(new AA() { aa = "agfaa" });
            B bts = Mapper.Map<A, B>(ats);
            Console.WriteLine(bts);
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
}
