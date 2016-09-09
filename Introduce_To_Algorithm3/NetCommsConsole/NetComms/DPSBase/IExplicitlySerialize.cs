using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetCommsConsole.NetComms.DPSBase
{
    /// <summary>
    /// 接口序列化、反序列化
    /// </summary>
    public interface IExplicitlySerialize
    {
        /// <summary>
        /// Serializes the current <see cref="IExplicitlySerialize"/> object to the provided <see cref="System.IO.Stream"/>
        /// </summary>
        /// <param name="outputStream">The stream to serialize to</param>
        void Serialize(Stream outputStream);

        /// <summary>
        /// Deserializes from a <see cref="System.IO.Stream"/> to the current <see cref="IExplicitlySerialize"/> object
        /// </summary>
        /// <param name="inputStream">The <see cref="System.IO.Stream"/> to deserialize from</param>
        void Deserialize(Stream inputStream);
    }
}
