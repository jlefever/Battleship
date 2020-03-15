using System.Collections.Generic;
using Battleship.DataTypes;

namespace Battleship
{
    public class GameTypeRepository
    {
        private readonly IDictionary<byte, GameType> _dictionary;

        public GameTypeRepository()
        {
            _dictionary = new Dictionary<byte, GameType>
            {
                // The classic GameType that is always supported.
                { 0, new GameType(0, 10, 10, new byte[] {5, 4, 3, 3, 2}) }
            };
        }

        public bool TryAdd(GameType gameType)
        {
            return _dictionary.TryAdd(gameType.GameTypeId, gameType);
        }

        public bool TryGet(byte id, out GameType gameType)
        {
            return _dictionary.TryGetValue(id, out gameType);
        }

        public IEnumerable<GameType> GetAll()
        {
            return _dictionary.Values;
        }
    }
}
