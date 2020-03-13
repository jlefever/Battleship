using System.Collections.Generic;
using Battleship.DataTypes;

namespace Battleship.Messages
{
    public class SubmitBoardMessage : IMessage
    {
        public SubmitBoardMessage(byte gameTypeId, IEnumerable<Placement> shipPlacements)
        {
            GameTypeId = gameTypeId;
            ShipPlacements = shipPlacements;
        }

        public byte GameTypeId { get; }
        public IEnumerable<Placement> ShipPlacements { get; }

        public MessageTypeId TypeId => MessageTypeId.SubmitBoard;

        public TResult Accept<TResult>(IMessageVisitor<TResult> visitor)
        {
            return visitor.VisitSubmitBoardMessage(this);
        }

        public override string ToString()
        {
            return $"{nameof(TypeId)}: {TypeId}, {nameof(GameTypeId)}: " +
                   $"{GameTypeId}, {nameof(ShipPlacements)}: {ShipPlacements}";
        }
    }
}