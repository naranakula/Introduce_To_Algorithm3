using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.sqls.EF2.Sqlite.DbModels
{


    /// <summary>
    /// 键值对表
    /// KvPair和DictItem一起添加
    /// </summary>
    public class KvPair
    {
        /// <summary>
        /// 键 方法保证不为空或null
        /// SQLite变长记录，字段不需要指定长度。
        /// 长度不要超过128
        /// </summary>
        //当发生UNIQUE约束冲突，先存在的，导致冲突的行在更改或插入发生冲突的行之前被删除。这样，更改和插入总是被执行。命令照常执行且不返回错误信息。
        //[SQLite.CodeFirst.Unique(OnConflictAction.Replace)]//唯一键
        public string Key { get; set; }

        /// <summary>
        /// 值 方法保证不为空或null
        /// SQLite变长记录，字段不需要指定长度
        /// </summary>
        public string Value { get; set; }


        /// <summary>
        /// 更新时间  不会产生UTC问题,读取时全部转换为了本地时间
        /// 直接使用本地时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 创建时间  不会产生UTC问题,读取时全部转换为了本地时间
        /// 直接使用本地时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }


}
