using Battleship.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battleship.DataTypes;

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
                .Concat(FromByte(message.Version))
                .Concat(FromString(message.Username))
                .Concat(FromString(message.Password));
        }

        public IEnumerable<byte> VisitRejectLogOnMessage(RejectLogOnMessage message)
        {
            return GetHeaderBytes(message.TypeId).Concat(FromByte(message.Version));
        }

        public IEnumerable<byte> VisitGameTypeMessage(GameTypeMessage message)
        {
            if (message.GameType.Ships.Length > BspConstants.MaxShips)
            {
                throw new MessageUnparserException($"Must not have more than {BspConstants.MaxShips} ships.");
            }

            var diff = BspConstants.MaxShips - message.GameType.Ships.Length;

            // Pad ship list to the required number of bytes.
            var ships = message.GameType.Ships.Concat(Enumerable.Repeat((byte)0, diff));

            return GetHeaderBytes(message.TypeId)
                .Concat(FromByte(message.GameType.GameTypeId))
                .Concat(FromByte(message.GameType.BoardWidth))
                .Concat(FromByte(message.GameType.BoardHeight))
                .Concat(ships);
        }

        public IEnumerable<byte> VisitSubmitBoardMessage(SubmitBoardMessage message)
        {
            var bytes = GetHeaderBytes(message.TypeId);

            foreach (var placement in message.ShipPlacements)
            {
                bytes = bytes.Concat(FromPlacement(placement));
            }

            return bytes;
        }

        public IEnumerable<byte> VisitRejectBoardMessage(RejectBoardMessage message)
        {
            return GetHeaderBytes(message.TypeId)
                .Concat(FromByte((byte)message.ErrorId));
        }

        public IEnumerable<byte> VisitMyGuessMessage(MyGuessMessage message)
        {
            return GetHeaderBytes(message.TypeId)
                .Concat(FromPosition(message.Position));
        }

        public IEnumerable<byte> VisitTheirGuessMessage(TheirGuessMessage message)
        {
            return GetHeaderBytes(message.TypeId)
                .Concat(FromPosition(message.Position));
        }

        public IEnumerable<byte> VisitYouLoseMessage(YouLoseMessage message)
        {
            return GetHeaderBytes(message.TypeId)
                .Concat(FromPosition(message.Position));
        }

        private static IEnumerable<byte> GetHeaderBytes(MessageTypeId typeId)
        {
            return FromUInt16((ushort)typeId).Concat(FromByte((byte)0));
        }

        private static IEnumerable<byte> FromPlacement(Placement placement)
        {
            return FromPosition(placement.Position).Concat(FromBool(placement.Vertical));
        }

        private static IEnumerable<byte> FromPosition(Position position)
        {
            return FromByte(position.Row).Concat(FromByte(position.Col));
        }

        private static IEnumerable<byte> FromUInt16(ushort value)
        {
            return BitConverter.GetBytes(value);
        }

        private static IEnumerable<byte> FromByte(byte value)
        {
            return new[] { value };
        }

        private static IEnumerable<byte> FromBool(bool value)
        {
            yield return value ? (byte)1 : (byte)0;
        }

        private static IEnumerable<byte> FromString(string value)
        {
            if (value.Length > BspConstants.MaxStringLength)
            {
                var message = $"Strings must not be longer than {BspConstants.MaxStringLength} characters.";
                throw new MessageUnparserException(message);
            }

            value = value.PadRight(BspConstants.MaxStringLength, '\0');
            return Encoding.ASCII.GetBytes(value);
        }
    }
}
