using System;
using System.Collections.Generic;
using Battleship.DataTypes;
using Battleship.DFA;
using Battleship.Messages;
using Battleship.Repositories;

namespace Battleship.Server.DFA
{
    public class WaitingForBoard : IWaitingForBoard
    {
        private readonly BspServerState _state;
        private readonly BspSender _sender;
        private readonly GameTypeRepository _gameTypeRepo;
        private readonly MatchMaker _matchMaker;

        public WaitingForBoard(BspServerState state, BspSender sender,
            GameTypeRepository gameTypeRepo, MatchMaker matchMaker)
        {
            _state = state;
            _sender = sender;
            _gameTypeRepo = gameTypeRepo;
            _matchMaker = matchMaker;
        }

        public IEnumerable<MessageTypeId> ValidReceives => new[]
        {
            MessageTypeId.SubmitBoard
        };

        public IEnumerable<MessageTypeId> ValidSends => new[]
        {
            MessageTypeId.GameType
        };

        public void Received(NetworkStateContext context, IMessage message)
        {
            context.SetState(NetworkStateId.PendingBoard);

            var submission = (SubmitBoardMessage)message;
            var placements = submission.ShipPlacements;

            var isValidGameType = _gameTypeRepo.TryGet(submission.GameTypeId, out var gameType);

            if (!isValidGameType)
            {
                SendRejection(RejectBoardErrorId.UnsupportedGameType);
                return;
            }

            if (placements.Count != gameType.ShipLengths.Count)
            {
                SendRejection(RejectBoardErrorId.WrongShips);
                return;
            }

            var board = new Board(gameType);

            for (var i = 0; i < placements.Count; i++)
            {
                if (board.IsOutOfBounds(placements[i], i))
                {
                    SendRejection(RejectBoardErrorId.OutOfBounds);
                    return;
                }

                if (board.IsOverlapping(placements[i], i))
                {
                    SendRejection(RejectBoardErrorId.ShipOverlap);
                    return;
                }

                if (!board.TryPlace(placements[i], i))
                {
                    // This shouldn't happen because we just checked the error
                    // conditions. If this somehow happens, we want to crash the
                    // server.
                    throw new Exception();
                }
            }

            _sender.Send(new BasicMessage(MessageTypeId.AcceptBoard));

            var userBoard = new UserBoard(_state.Username, board);
            var match = _matchMaker.FindMatchAsync(userBoard).GetAwaiter().GetResult();

            if (match == null)
            {
                // This could happen if the server somehow ends up in an
                // inconsistent state where it can't add the user to match making.
                throw new Exception();
            }

            _state.Match = match;
            _sender.Send(new BasicMessage(MessageTypeId.FoundGame));
        }

        public void Sent(NetworkStateContext context, IMessage message)
        {
            // This is left intentionally blank.
        }

        private void SendRejection(RejectBoardErrorId id)
        {
            _sender.Send(new RejectBoardMessage(id));
        }
    }
}
