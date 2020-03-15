using System;
using System.Timers;
using Battleship.DataTypes;

namespace Battleship
{
    public sealed class BspServerState : IDisposable
    {
        private readonly Timer _matchTimer;

        public BspServerState()
        {
            _matchTimer = new Timer(BspConstants.AcceptMatchTimeout);
        }

        public string Username { get; set; }
        public Match Match { get; set; }

        public void SetMatchTimeoutCallback(ElapsedEventHandler callback)
        {
            _matchTimer.Elapsed += callback;
        }

        public void StartMatchTimer()
        {
            _matchTimer.Enabled = true;
            _matchTimer.AutoReset = false;
            _matchTimer.Start();
        }

        public void CancelMatchTimer()
        {
            _matchTimer.Stop();
        }

        public void Dispose()
        {
            _matchTimer.Close();
            _matchTimer.Dispose();
        }
    }
}
