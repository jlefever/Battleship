﻿using System.Collections.Generic;

namespace Battleship.Messages
{
    public class SubmitBoardMessage : IMessage
    {
        public SubmitBoardMessage(byte gameTypeId, IEnumerable<ShipLocation> locations)
        {
            GameTypeId = gameTypeId;
            Locations = locations;
        }

        public byte GameTypeId { get; }
        public IEnumerable<ShipLocation> Locations { get; }

        public MessageTypeId TypeId => MessageTypeId.SubmitBoard;

        public TResult Accept<TResult>(IMessageVisitor<TResult> visitor)
        {
            return visitor.VisitSubmitBoardMessage(this);
        }

        public override string ToString()
        {
            return $"{nameof(TypeId)}: {TypeId}, {nameof(GameTypeId)}: " +
                   $"{GameTypeId}, {nameof(Locations)}: {Locations}";
        }
    }
}