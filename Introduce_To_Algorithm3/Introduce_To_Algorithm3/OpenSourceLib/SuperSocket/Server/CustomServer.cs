﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Introduce_To_Algorithm3.OpenSourceLib.SuperSocket.Server;
using SuperSocket.Facility.Protocol;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace Introduce_To_Algorithm3.OpenSourceLib
{
    /// <summary>
    /// 带定制Session的定制的服务器
    /// </summary>
    public class CustomServer:AppServer<CustomSession>
    {

        #region 构造函数

        public CustomServer()
            : base(new CommandLineReceiveFilterFactory())
        {
            //
            //默认的命令行协议要求每个请求必须以”\r\n”结束，空格分隔命令、各个参数。在SuperSocket中命令行协议会翻译成StringRequestInfo实例。
        }


        /// <summary>
        /// 构造函数
        /// 使用定制的命令行协议
        /// </summary>
        public CustomServer(CommandLineReceiveFilterFactory filterFactory)
            : base(new CommandLineReceiveFilterFactory(Encoding.UTF8, new BasicRequestInfoParser(":", ",")))
        {
            //定制的协议 请求的 key 和 body 通过字符 ':' 分隔, 而且多个参数被字符 ',' 分隔。
        }

        /// <summary>
        /// 终止符协议
        /// </summary>
        public CustomServer(TerminatorReceiveFilterFactory filterFactory=null)
            : base(new TerminatorReceiveFilterFactory("###"))
        {
            //结束符协议
            //一个协议使用三个字符 "###" 作为结束符
        }

        /// <summary>
        /// 固定数量分隔符协议
        /// </summary>
        public CustomServer(CountSpliterReceiveFilterFactory filterFactory)
            : base(new CountSpliterReceiveFilterFactory((byte)'#',8))
        {
            //协议定义了像这样格式的请求 "#part1#part2#part3#part4#part5#part6#part7#". 每个请求有7个由 '#' 分隔的部分. 
        }

        /// <summary>
        /// 固定长度请求的协议
        /// </summary>
        /// <param name="filter"></param>
        public CustomServer(FixedSizeReceiveFilter<StringRequestInfo> filter) : base(new DefaultReceiveFilterFactory<CmluFixedSizeReceiveFilter,StringRequestInfo>())
        {
            //在这种协议之中, 所有请求的大小都是相同的。
        }

        /// <summary>
        /// 带起止符的协议
        /// 即协议的开始部分和结束部分是一致的
        /// </summary>
        /// <param name="receiveFilter"></param>
        public CustomServer(BeginEndMarkReceiveFilter<StringRequestInfo> receiveFilter)
            : base(new DefaultReceiveFilterFactory<CmluBeginEndMarkReceiveFilter, StringRequestInfo>())
        {
            
        }

        #endregion



        /// <summary>
        /// Called when [started].
        /// </summary>
        protected override void OnStarted()
        {
            base.OnStarted();
        }

        /// <summary>
        ///  Called when [stopped].
        /// </summary>
        protected override void OnStopped()
        {
            base.OnStopped();
        }


    }
}