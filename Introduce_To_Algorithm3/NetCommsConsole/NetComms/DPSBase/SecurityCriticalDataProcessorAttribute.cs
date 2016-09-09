using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCommsConsole.NetComms.DPSBase
{
    /// <summary>
    /// Custom attribute used to label data processors as security critical or not
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Class)]
    public class SecurityCriticalDataProcessorAttribute : System.Attribute
    {
        /// <summary>
        /// A booling defining if this data processor is security critical
        /// </summary>
        public bool IsSecurityCritical { get; private set; }

        /// <summary>
        /// Create a new instance of this attribute
        /// </summary>
        /// <param name="isSecurityCritical">是否安全性重要</param>
        public SecurityCriticalDataProcessorAttribute(bool isSecurityCritical)
        {
            this.IsSecurityCritical = isSecurityCritical;
        }
    }
}
