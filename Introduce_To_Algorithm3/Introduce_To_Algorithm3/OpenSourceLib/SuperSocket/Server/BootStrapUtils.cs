using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase;
using SuperSocket.SocketEngine;

namespace Introduce_To_Algorithm3.OpenSourceLib.SuperSocket.Server
{
    /// <summary>
    /// 通过BootStrap
    /// </summary>
    public class BootStrapUtils
    {
        public static void Start()
        {
            var bootstrap = BootstrapFactory.CreateBootstrap();
            if (!bootstrap.Initialize())
            {
                //初始化失败
            }

            var result = bootstrap.Start();

            if (result == StartResult.Success)
            {
                //启动成功
            }
        }
    }
}
