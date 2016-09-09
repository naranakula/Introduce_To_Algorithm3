using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCommsConsole.NetComms.Tools
{
    /// <summary>
    /// A list of priorities used to handle incoming packets
    /// </summary>
    public enum QueueItemPriority
    {
        /// <summary>
        /// The System.Threading.Thread can be scheduled after threads with any other priority.
        /// </summary>
        Lowest = 0,

        /// <summary>
        ///  The System.Threading.Thread can be scheduled after threads with Normal priority and before those with Lowest priority.
        /// </summary>
        BelowNormal = 1,

        /// <summary>
        /// The System.Threading.Thread can be scheduled after threads with AboveNormal priority and before those with BelowNormal priority. Threads have Normal priority by default.
        /// </summary>
        Normal = 2,

        /// <summary>
        /// The System.Threading.Thread can be scheduled after threads with Highest priority and before those with Normal priority.
        /// </summary>
        AboveNormal = 3,

        /// <summary>
        /// The System.Threading.Thread can be scheduled before threads with any other priority.
        /// </summary>
        Highest = 4,
    }
}
