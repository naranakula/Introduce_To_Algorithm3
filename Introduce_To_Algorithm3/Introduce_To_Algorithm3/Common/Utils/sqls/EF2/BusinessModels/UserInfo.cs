using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.sqls.EF2.BusinessModels
{

    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 帐号
        /// </summary>
        public string AccountNo { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 密码的hash，建议使用sha256,然后转base64字符串
        /// </summary>
        public string PasswordHash { get; set; }
        
        /// <summary>
        /// 最近一次登录时间
        /// </summary>
        public DateTime LastestLoginTime { get; set; }

        /// <summary>
        /// 最近一次登录的Token标志
        /// </summary>
        public string CurrentLoginToken { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

    }
}
