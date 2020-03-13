﻿using Battleship.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            if (message.GameType.Ships.Length > BspConstants.MaxShips)
            {
                throw new MessageUnparserException($"Must not have more than {BspConstants.MaxShips} ships.");
            }

            var diff = BspConstants.MaxShips - message.GameType.Ships.Length;
            var ships = message.GameType.Ships.Concat(Enumerable.Repeat((byte)0, diff));

            return GetHeaderBytes(message.TypeId)
                .Concat(GetBytes(message.GameType.GameTypeId))
                .Concat(GetBytes(message.GameType.BoardWidth))
                .Concat(GetBytes(message.GameType.BoardHeight))
                .Concat(ships);
        }

        public IEnumerable<byte> VisitSubmitBoardMessage(SubmitBoardMessage message)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<byte> VisitRejectBoardMessage(RejectBoardMessage message)
        {
            return GetHeaderBytes(message.TypeId)
                .Concat(GetBytes((byte)message.ErrorId));
        }

        public IEnumerable<byte> VisitMyGuessMessage(MyGuessMessage message)
        {
            return GetHeaderBytes(message.TypeId)
                .Concat(GetBytes(message.Position.Row))
                .Concat(GetBytes(message.Position.Col));
        }

        public IEnumerable<byte> VisitTheirGuessMessage(TheirGuessMessage message)
        {
            return GetHeaderBytes(message.TypeId)
                .Concat(GetBytes(message.Position.Row))
                .Concat(GetBytes(message.Position.Col));
        }

        public IEnumerable<byte> VisitYouLoseMessage(YouLoseMessage message)
        {
            return GetHeaderBytes(message.TypeId)
                .Concat(GetBytes(message.Position.Row))
                .Concat(GetBytes(message.Position.Col));
        }

        private static IEnumerable<byte> GetHeaderBytes(MessageTypeId typeId)
        {
            return GetBytes((ushort)typeId).Concat(GetBytes((byte)0));
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
