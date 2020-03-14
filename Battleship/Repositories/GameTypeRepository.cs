using Battleship.DataTypes;
using System.Collections.Generic;

namespace Battleship.Repositories
{
    public class GameTypeRepository
    {
        private readonly IDictionary<byte, GameType> _dictionary;

        public GameTypeRepository()
        {
            _dictionary = new Dictionary<byte, GameType>();
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

            //return new[]
            //{
            //    new GameType(1, 15, 15, new byte[] {2}),
            //    new GameType(2, 5, 5, new byte[] {1}),
            //    new GameType(1, 10, 10, new byte[] {3, 2, 1})
            //};
        }
    }
}
