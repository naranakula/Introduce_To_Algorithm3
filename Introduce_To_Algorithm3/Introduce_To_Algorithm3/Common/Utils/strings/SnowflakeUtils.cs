using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.strings
{
    /// <summary>
    /// Guid有两个缺点:1)较长 2)无序
    /// Twitter的分布式自增id算法snowflake:
    /// 共64位，第一位未使用，接下来41位为毫秒级时间(41位可以使用69年)，然后是5位datacenterid和5位workerid(10位最多1024个节点)，最后12位毫秒内计数(每毫秒可以生成4096个id序号)
    /// </summary>
    public static class SnowflakeUtils
    {
    }
}
