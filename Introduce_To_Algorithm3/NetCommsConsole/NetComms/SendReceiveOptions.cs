using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCommsConsole.NetComms
{
    /// <summary>
    /// Contains options and flags for sending and receiving data such as serialisation method, data processors, encryption etc.
    /// Several static constructors are provided to help create SendReceiveOptions in the most common formats.
    /// </summary>
    public class SendReceiveOptions
    {
        #region 私有变量


        private Dictionary<string, string> options;


        #endregion

        #region 公有变量

        /// <summary>
        /// Gets the options that should be passed to the <see cref="DPSBase.DataSerializer"/> and <see cref="DPSBase.DataProcessor"/>s on object serialization and deserialization
        /// </summary>
        public Dictionary<string, string> Options
        {
            get { return options; }
        }

        #endregion
    }
}
