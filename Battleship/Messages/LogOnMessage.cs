namespace Battleship.Messages
{
    public class LogOnMessage : IMessage
    {
        public byte Version { get; }

        public string Username { get; }

        public string Password { get; }

        public LogOnMessage(byte version, string username, string password)
        {
            Version = version;
            Username = username;
            Password = password;
        }

        public TResult Accept<TResult>(IMessageVisitor<TResult> visitor)
        {
            return visitor.VisitLogOn(this);
        }

        public override string ToString()
        {
            return $"{nameof(Version)}: {Version}, {nameof(Username)}: {Username}, {nameof(Password)}: {Password}";
        }
    }
}
