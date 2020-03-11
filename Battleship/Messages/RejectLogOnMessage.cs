namespace Battleship.Messages
{
    public class RejectLogOnMessage : IMessage
    {
        public RejectLogOnMessage(byte version)
        {
            Version = version;
        }

        public MessageTypeId TypeId => MessageTypeId.RejectLogOn;
        public byte Version { get; }

        public TResult Accept<TResult>(IMessageVisitor<TResult> visitor)
        {
            return visitor.VisitRejectLogOnMessage(this);
        }
    }
}
