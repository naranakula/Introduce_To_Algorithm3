using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NetCommsConsole.NetComms.Connections;
using NetCommsConsole.NetComms.Models;

namespace NetCommsConsole.NetComms.Tools
{
    /// <summary>
    /// The following packetTypeHandlerDelegateWrappers are required so that we can do the totally general and awesome object cast on deserialise.
    /// If there is a way of achieving the same without these wrappers please let us know.
    /// </summary>
    public interface IPacketTypeHandlerDelegateWrapper : IEquatable<IPacketTypeHandlerDelegateWrapper>
    {
        object DeSerialize(MemoryStream incomingBytes, SendReceiveOptions options);

        void Process(PacketHeader packetHeader, Connection connection, object obj);
        bool EqualsDelegate(Delegate other);
    }
}
