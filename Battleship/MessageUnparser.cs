using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battleship.Messages;

namespace Battleship
{
    public class MessageUnparser : IMessageVisitor<byte[]>
    {
        public byte[] VisitLogOn(LogOnMessage message)
        {
            var idBytes = BitConverter.GetBytes((ushort)0);
            var extensionBytes = new byte[] { 0 };
            var versionBytes = new[]{ message.Version };
            var usernameBytes = Encoding.ASCII.GetBytes(message.Username.PadRight(16, '\0'));
            var passwordBytes = Encoding.ASCII.GetBytes(message.Password.PadRight(16, '\0'));

            return idBytes
                .Concat(extensionBytes)
                .Concat(versionBytes)
                .Concat(usernameBytes)
                .Concat(passwordBytes)
                .ToArray();
        }

        public byte[] VisitRejectLogOn(RejectLogOn message)
        {
            throw new NotImplementedException();
        }

        public byte[] VisitAcceptLogOn(AcceptLogOn message)
        {
            throw new NotImplementedException();
        }
    }
}
