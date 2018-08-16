using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.sqls.EF2.Sqlite.DbModels
{


    public class ListBytesItem
    {
        /// <summary>
        /// Item
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 数据项
        /// </summary>
        public byte[] Item { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }


}
