using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCommsConsole.NetComms.Models
{
    /// <summary>
    /// Any <see cref="PacketHeader"/> options which are stored as a long.
    /// </summary>
    public enum PacketHeaderLongItems
    {
        /// <summary>
        /// The total size of the packet data payload in bytes. This is a compulsory option.
        /// </summary>
        TotalPayloadSize,

        /// <summary>
        /// The data serialiser and data processor used to unwrap the payload. Used as flags.
        /// </summary>
        SerializerProcessors,

        /// <summary>
        /// The sequence number for this packet. Each connection maintains a unique counter which is increments on each sent packet. This is a compulsory option.
        /// </summary>
        PacketSequenceNumber,

        /// <summary>
        /// The creation time of the packet header.
        /// </summary>
        PacketCreationTime,
    }
}
