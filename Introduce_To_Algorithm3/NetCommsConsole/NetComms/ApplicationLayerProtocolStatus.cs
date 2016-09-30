using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCommsConsole.NetComms
{
    /// <summary>
    /// The connections application layer protocol status
    /// </summary>
    public enum ApplicationLayerProtocolStatus
    {
        /// <summary>
        /// Useful for selecting or searching connections when the ApplicationLayerProtocolStatus
        /// is unimportant.
        /// </summary>
        Undefined,

        /// <summary>
        /// Default value. NetworkComms.Net will use a custom application layer protocol to provide 
        /// useful features such as inline serialisation, transparent packet send and receive, 
        /// connection handshakes and remote information etc. We strongly recommend you enable the 
        /// NetworkComms.Net application layer protocol.
        /// </summary>
        Enabled,

        /// <summary>
        /// No application layer protocol will be used. TCP packets may fragment or be concatenated （粘包 拆包）
        /// with other packets. A large number of library features will be unavailable.
        /// </summary>
        Disabled
    }
}
