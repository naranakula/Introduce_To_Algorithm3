using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.sqls.EF2
{
    /// <summary>
    /// SQLite主键使用long
    /// 
    /// 目前SQLite EF不支持Code First,目前使用策略是使用SqliteHelper，先手动创建数据库。再使用EF来兼容数据库。
    /// 当SQLiteConnection调用Open的时候如果数据库不存在（通过File.Exists检查），则自动使用连接字符串创建数据库
    /// 文档：
    /// http://system.data.sqlite.org/index.html/doc/trunk/www/index.wiki
    /// 
    /// 现在应该支持了
    /// </summary>
    public class SqliteDbContext:EfDbContext
    {
    }
}
