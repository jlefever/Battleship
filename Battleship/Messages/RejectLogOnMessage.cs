namespace Battleship.Messages
{
    public class RejectLogOnMessage : IMessage
    {
        public RejectLogOnMessage(byte version)
        {
            Version = version;
        }

        public byte Version { get; }

        public MessageTypeId TypeId => MessageTypeId.RejectLogOn;

        public TResult Accept<TResult>(IMessageVisitor<TResult> visitor)
        {
            return visitor.VisitRejectLogOnMessage(this);
        }

        public override string ToString()
        {
            return $"{nameof(TypeId)}: {TypeId}, {nameof(Version)}: {Version}";
        }
    }
}