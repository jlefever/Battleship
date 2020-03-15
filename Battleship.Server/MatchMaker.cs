using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Battleship.DataTypes;

namespace Battleship.Server
{
    public class MatchMaker
    {
        private readonly IDictionary<string, UserBoard> _players;
        private readonly IDictionary<string, WriteOnceBlock<Match>> _blocks;
        private readonly Random _random;

        public MatchMaker()
        {
            _players = new Dictionary<string, UserBoard>();
            _blocks = new Dictionary<string, WriteOnceBlock<Match>>();
            _random = new Random();
        }

        public async Task<Match> FindMatchAsync(UserBoard player)
        {
            if (!_players.TryAdd(player.Username, player))
            {
                return null;
            }

            var block = new WriteOnceBlock<Match>(null);

            if (!_blocks.TryAdd(player.Username, block))
            {
                _players.Remove(player.Username);
                return null;
            }

            LookForMatches(player, block);
            return await block.ReceiveAsync();
        }

        public void Remove(string username)
        {
            _players.Remove(username);
            _blocks.Remove(username);
        }

        private void LookForMatches(UserBoard player, ITargetBlock<Match> playerBlock)
        {
            foreach (var opponent in _players.Values)
            {
                // Do not match with yourself!
                if (opponent.Username == player.Username)
                {
                    continue;
                }

                // Match with the first person who has the same GameType
                if (opponent.Board.GameTypeId != player.Board.GameTypeId)
                {
                    continue;
                }

                var opponentBlock = _blocks[opponent.Username];

                // Remove opponent from match making.
                Remove(opponent.Username);

                // Remove player from match making.
                Remove(player.Username);

                // Notify both async waiting contexts that a match has been found.
                var playerGoesFirst = FlipCoin();
                playerBlock.Post(new Match(player, opponent, playerGoesFirst));
                opponentBlock.Post(new Match(opponent, player, !playerGoesFirst));

                // Stop searching through the players.
                break;
            }
        }

        private bool FlipCoin()
        {
            return _random.NextDouble() >= 0.5;
        }
    }
}
