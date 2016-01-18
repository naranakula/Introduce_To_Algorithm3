using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.NamedPipe
{
    public static class NamedPipeClient
    {
        /// <summary>
        /// 发送消息给指定的命名管道
        /// </summary>
        /// <param name="message"></param>
        /// <param name="toNamedPipe"></param>
        /// <returns></returns>
        public static bool SendMessage(string message, string toNamedPipe)
        {
            try
            {
                using (NamedPipeClientStream client = new NamedPipeClientStream(".", toNamedPipe, PipeDirection.Out))
                {
                    //连接超时1500ms
                    client.Connect(1500);
                    if (client.IsConnected)
                    {
                        using (StreamWriter sw = new StreamWriter(client, Encoding.UTF8))
                        {
                            sw.AutoFlush = true;
                            sw.WriteLine(message);
                        }
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
