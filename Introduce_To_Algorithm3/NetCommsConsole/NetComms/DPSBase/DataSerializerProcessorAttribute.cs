using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCommsConsole.NetComms.DPSBase
{
    /// <summary>
    /// Custom attribute used to keep track of serializers and processors
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DataSerializerProcessorAttribute:Attribute
    {
        /// <summary>
        /// A byte identifier, unique amongst all serialisers and data processors.
        /// </summary>
        public byte Identifier { get; private set; }

        /// <summary>
        /// Create a new instance of this attribute
        /// </summary>
        /// <param name="identifier"></param>
        public DataSerializerProcessorAttribute(byte identifier)
        {
            this.Identifier = identifier;
        }
    }
}
