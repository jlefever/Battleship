﻿namespace Battleship.Messages
{
    public enum RejectBoardErrorId : byte
    {
        ShipOverlap = 0,
        OutOfBounds = 1,
        UnsupportedGameType = 2,
        WrongShips = 3,
        BadSubmission = 4
    }
}