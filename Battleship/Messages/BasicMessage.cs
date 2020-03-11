namespace Battleship.Messages
{
    public class BasicMessage : IMessage
    {
        public BasicMessage(MessageTypeId typeId)
        {
            TypeId = typeId;
        }

        public MessageTypeId TypeId { get; }

        public TResult Accept<TResult>(IMessageVisitor<TResult> visitor)
        {
            return visitor.VisitBasicMessage(this);
        }

        public override string ToString()
        {
            return $"{nameof(TypeId)}: {TypeId}";
        }
    }
}
