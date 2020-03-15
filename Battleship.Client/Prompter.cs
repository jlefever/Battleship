using Battleship.DataTypes;
using Battleship.Messages;
using System;
using System.Collections.Generic;

namespace Battleship.Client
{
    /// <summary>
    /// All user interaction is provided as prompts. Some prompts will poll
    /// for input and send messages to the server. These are mostly used as
    /// callbacks for inside the state machine.
    /// </summary>
    public class Prompter
    {
        private readonly BspSender _sender;

        public Prompter(BspSender sender)
        {
            _sender = sender;
        }

        public void PromptLogOn()
        {
            Write("Username: ");
            var username = Console.ReadLine();

            Write("Password: ");
            var password = Console.ReadLine();

            _sender.Send(new LogOnMessage(BspConstants.Version, username, password));
        }

        public void PromptSuccessfulLogOn()
        {
            WriteLine("Welcome!");
        }

        public void PromptFailedLogOn(byte version)
        {
            WriteLine("Could not log on! Is that the right username and password?");
            WriteLine($"This server only supports clients running BSP version {version}.");
        }

        public void PromptWaitingForBoard()
        {
            Write("Press ENTER to get the default board.");
            Console.ReadLine();

            var placements = new List<Placement>
            {
                new Placement(new Position(0, 0), false),
                new Placement(new Position(1, 0), false),
                new Placement(new Position(2, 0), false),
                new Placement(new Position(3, 0), false),
                new Placement(new Position(4, 0), false)
            };

            _sender.Send(new SubmitBoardMessage(0, placements));
        }

        public void PromptInvalidBoard()
        {
            WriteLine("Something was wrong with your board submission.");
        }

        public void PromptValidBoard()
        {
            WriteLine("Looking for an opponent...");
        }

        public void PromptFoundGame()
        {
            Write("Found an opponent! Accept match (Y/N)? ");
            var accept = ReadY();
            var id = accept ? MessageTypeId.AcceptGame : MessageTypeId.RejectGame;
            _sender.Send(new BasicMessage(id));
        }

        public void PromptMatchExpired()
        {
            WriteLine("Too slow! The match has expired.");
        }

        public void PromptOpponentAccepted()
        {
            WriteLine("Opponent has also accepted the match!");
        }

        public void PromptOpponentRejected()
        {
            WriteLine("Opponent has declined the match.");
        }

        public void PromptAssignRed()
        {
            WriteLine("You go first!");
        }

        public void PromptAssignBlue()
        {
            WriteLine("You go second!");
        }

        public void PromptMyTurn()
        {
            Write("Enter your guess (ex: \"4 3\"): ");
            var position = ReadPosition();
            _sender.Send(new MyGuessMessage(position));
        }

        public void PromptTheirTurn()
        {
            WriteLine("Waiting for your opponents move...");
        }

        public void PromptYouHit()
        {
            WriteLine("You hit one of your opponents ships!");
        }

        public void PromptYouSunk()
        {
            WriteLine("You hit and sunk one of your opponents ships!");
        }

        public void PromptYouMissed()
        {
            WriteLine("You missed!");
        }

        public void PromptTheirGuess(Position p)
        {
            WriteLine($"Your opponent guessed [{p.Row} {p.Col}].");
        }

        public void PromptYouWin()
        {
            WriteLine("You won! You sunk all your opponents ships.");
        }

        public void PromptYouLose()
        {
            WriteLine("You lost! All your ships have been sunk.");
        }

        private static Position ReadPosition()
        {
            var line = Console.ReadLine().Split(' ');
            var row = Convert.ToByte(line[0]);
            var col = Convert.ToByte(line[1]);
            return new Position(row, col);
        }

        private static bool ReadY()
        {
            var text = Console.ReadLine();
            return text.Equals("Y", StringComparison.OrdinalIgnoreCase);
        }

        private static void Write(string text)
        {
            SetColor();
            Console.Write(text);
            ResetColor();
        }

        private static void WriteLine(string text)
        {
            SetColor();
            Console.WriteLine(text);
            ResetColor();
        }

        private static void SetColor()
        {
            Console.ForegroundColor = ConsoleColor.Green;
        }

        private static void ResetColor()
        {
            Console.ResetColor();
        }
    }
}
