using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Introduce_To_Algorithm3.OpenSourceLib.MongoDbs.Models
{
    public class BaseBsonModel
    {

        /*
         * ObjectID长度为12byte,包含 4字节时间(自epoch以来的秒数),3字节机器id,2字节进程Id,3字节计数
         * 每秒中产生256*256*256=16777216个不重复数据
         * 
         * 
         * 
         * 
         */

        /// <summary>
        /// 主键 名字必须为_id
        /// 
        /// field name _id 保留用来做主键，必须唯一，不可修改，may be of any type other than an array.
        /// 
        /// </summary>
        public ObjectId _id { get; set; }




        #region 其它属性


        #endregion


    }
}
