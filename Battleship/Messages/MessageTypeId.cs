namespace Battleship.Messages
{
    public enum MessageTypeId : ushort
    {
        LogOn = 0,
        RejectLogOn = 1,
        AcceptLogOn = 2,
        GameType = 3,
        SubmitBoard = 4,
        RejectBoard = 5,
        AcceptBoard = 6,
        RecallBoard = 7,
        FoundGame = 8,
        RejectGame = 9,
        AcceptGame = 10,
        GameExpired = 11,
        AssignRed = 12,
        AssignBlue = 13,
        MyGuess = 14,
        TheirGuess = 15,
        Hit = 16,
        Miss = 17,
        Sunk = 18,
        YouWin = 19,
        YouLose = 20
    }
}