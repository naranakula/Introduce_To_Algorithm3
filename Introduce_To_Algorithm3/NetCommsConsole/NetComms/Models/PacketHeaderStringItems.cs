using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCommsConsole.NetComms.Models
{
    /// <summary>
    /// Any <see cref="PacketHeader"/> options which are stored as a string.
    /// </summary>
    public enum PacketHeaderStringItems
    {
        /// <summary>
        /// The type of the packet. This is a compulsory option which determines how the incoming packet is handled.
        /// </summary>
        PacketType,

        /// <summary>
        /// Specifies if a receive confirmation is required for this packet. String option as takes up less space for a boolean option.
        /// </summary>
        ReceiveConfirmationRequired,

        /// <summary>
        /// The packet type which should be used for any return packet type.
        /// </summary>
        RequestedReturnPacketType,

        /// <summary>
        /// A checksum corresponding to the payload data.
        /// </summary>
        CheckSumHash,

        /// <summary>
        /// The network identifier of the packet source
        /// </summary>
        SourceNetworkIdentifier,

        /// <summary>
        /// Optional packet identifier.
        /// </summary>
        PacketIdentifier,

        /// <summary>
        /// The data section should be interpreted as a null 
        /// </summary>
        NullDataSection,
    }
}
