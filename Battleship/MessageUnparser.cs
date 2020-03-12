using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battleship.Messages;

namespace Battleship
{
    public class MessageUnparser : IMessageVisitor<IEnumerable<byte>>
    {
        public IEnumerable<byte> VisitBasicMessage(BasicMessage message)
        {
            return GetHeaderBytes(message.TypeId);
        }

        public IEnumerable<byte> VisitLogOnMessage(LogOnMessage message)
        {
            return GetHeaderBytes(message.TypeId)
                .Concat(GetBytes(message.Version))
                .Concat(GetBytes(message.Username))
                .Concat(GetBytes(message.Password));
        }

        public IEnumerable<byte> VisitRejectLogOnMessage(RejectLogOnMessage message)
        {
            return GetHeaderBytes(message.TypeId).Concat(GetBytes(message.Version));
        }

        public IEnumerable<byte> VisitGameTypeMessage(GameTypeMessage message)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<byte> VisitSubmitBoardMessage(SubmitBoardMessage message)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<byte> VisitRejectBoardMessage(RejectBoardMessage message)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<byte> VisitMyGuessMessage(MyGuessMessage message)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<byte> VisitTheirGuessMessage(TheirGuessMessage message)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<byte> VisitYouLoseMessage(YouLoseMessage message)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<byte> GetHeaderBytes(MessageTypeId typeId)
        {
            return GetBytes((ushort)typeId).Concat(GetBytes(0));
        }

        private static IEnumerable<byte> GetBytes(byte value)
        {
            return new[] { value };
        }

        private static IEnumerable<byte> GetBytes(ushort value)
        {
            return BitConverter.GetBytes(value);
        }

        private static IEnumerable<byte> GetBytes(string value)
        {
            value = value.PadRight(16, '\0').Substring(0, 16);
            return Encoding.ASCII.GetBytes(value);
        }
    }
}
