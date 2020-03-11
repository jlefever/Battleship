namespace Battleship.Messages
{
    public class LogOnMessage : IMessage
    {
        public LogOnMessage(byte version, string username, string password)
        {
            Version = version;
            Username = username;
            Password = password;
        }

        public byte Version { get; }
        public string Username { get; }
        public string Password { get; }

        public MessageTypeId TypeId => MessageTypeId.LogOn;

        public TResult Accept<TResult>(IMessageVisitor<TResult> visitor)
        {
            return visitor.VisitLogOnMessage(this);
        }

        public override string ToString()
        {
            return $"{nameof(TypeId)}: {TypeId}, {nameof(Version)}: {Version}, " +
                   $"{nameof(Username)}:{Username}, {nameof(Password)}: {Password}";
        }
    }
}