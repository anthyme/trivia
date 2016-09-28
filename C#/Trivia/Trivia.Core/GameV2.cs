using System;
using System.Collections.Generic;
using System.Linq;

namespace Trivia.Core
{
    public class GameV2 : IGame
    {
        private readonly List<Player> players = new List<Player>();
        private readonly Deck popQuestions = new Deck();
        private readonly Deck scienceQuestions = new Deck();
        private readonly Deck sportsQuestions = new Deck();
        private readonly Deck rockQuestions = new Deck();
        private readonly Board board;

        private int currentPlayerIndex = 0;

        private bool isGettingOutOfPenaltyBox;

        public GameV2()
        {
            CreateQuestions();
            board = new Board(CreateSquares());
        }

        private IEnumerable<Square> CreateSquares()
        {
            foreach (var i in Enumerable.Range(0, 3))
            {
                yield return new Square("Pop", popQuestions);
                yield return new Square("Science", scienceQuestions);
                yield return new Square("Sports", sportsQuestions);
                yield return new Square("Rock", rockQuestions);
            }
        }

        private void CreateQuestions()
        {
            for (int i = 0; i < 50; i++)
            {
                popQuestions.Enqueue("Pop Question " + i);
                scienceQuestions.Enqueue("Science Question " + i);
                sportsQuestions.Enqueue("Sports Question " + i);
                rockQuestions.Enqueue("Rock Question " + i);
            }
        }

        private int PlayerCount => players.Count;
        private Player CurrentPlayer => players[currentPlayerIndex];

        public void AddPlayer(string playerName)
        {
            players.Add(new Player(playerName));
            Log(playerName + " was added");
            Log("They are player number " + players.Count);
        }

        public void Roll(int roll)
        {
            Log(CurrentPlayer.Name + " is the current player");
            Log("They have rolled a " + roll);

            if (CurrentPlayer.InPenaltyBox)
            {
                if (roll % 2 != 0)
                {
                    isGettingOutOfPenaltyBox = true;

                    Log(CurrentPlayer.Name + " is getting out of the penalty box");
                    CurrentPlayer.Position = CurrentPlayer.Position + roll;
                    if (CurrentPlayer.Position > 11) CurrentPlayer.Position = CurrentPlayer.Position - 12;

                    Log(CurrentPlayer.Name
                            + "'s new location is "
                            + CurrentPlayer.Position);
                    Log("The category is " + board[CurrentPlayer.Position].Category);
                    AskQuestion();
                }
                else
                {
                    Log(CurrentPlayer.Name + " is not getting out of the penalty box");
                    isGettingOutOfPenaltyBox = false;
                }
            }
            else
            {
                CurrentPlayer.Position = CurrentPlayer.Position + roll;
                if (CurrentPlayer.Position > 11) CurrentPlayer.Position = CurrentPlayer.Position - 12;

                Log(CurrentPlayer.Name
                        + "'s new location is "
                        + CurrentPlayer.Position);
                Log("The category is " + board[CurrentPlayer.Position].Category);
                AskQuestion();
            }
        }

        private void AskQuestion()
        {
            var card = board[CurrentPlayer.Position].AssociatedDeck.Dequeue();
            Log(card);
        }

        public bool CorrectAnswer()
        {
            if (CurrentPlayer.InPenaltyBox)
            {
                if (isGettingOutOfPenaltyBox)
                {
                    Log("Answer was correct!!!!");
                    CurrentPlayer.Purse++;
                    Log(CurrentPlayer.Name
                            + " now has "
                            + CurrentPlayer.Purse
                            + " Gold Coins.");

                    bool winner = DidPlayerWin();
                    currentPlayerIndex++;
                    if (currentPlayerIndex == players.Count) currentPlayerIndex = 0;

                    return winner;
                }
                else
                {
                    currentPlayerIndex++;
                    if (currentPlayerIndex == players.Count) currentPlayerIndex = 0;
                    return false;
                }
            }
            else
            {
                Log("Answer was corrent!!!!");
                CurrentPlayer.Purse++;
                Log($"{CurrentPlayer.Name} now has {CurrentPlayer.Purse} Gold Coins.");

                bool winner = DidPlayerWin();
                currentPlayerIndex++;
                if (currentPlayerIndex == players.Count) currentPlayerIndex = 0;

                return winner;
            }
        }

        public void WrongAnswer()
        {
            Log("Question was incorrectly answered");
            Log(CurrentPlayer.Name + " was sent to the penalty box");
            CurrentPlayer.InPenaltyBox = true;

            currentPlayerIndex++;
            if (currentPlayerIndex == players.Count) currentPlayerIndex = 0;
        }

        private bool DidPlayerWin()
        {
            return CurrentPlayer.Purse == 6;
        }

        private void Log(string message)
        {
            Console.WriteLine(message);
        }
    }

    public class Player
    {
        public string Name { get; }
        public int Purse { get; set;  }
        public int Position { get; set; }
        public bool InPenaltyBox { get; set; }

        public Player(string name)
        {
            Name = name;
        }
    }

    class Board : List<Square>
    {
        public Board(IEnumerable<Square> squares) : base(squares) { }
    }

    class Square
    {
        public string Category { get; }
        public Deck AssociatedDeck { get; }

        public Square(string category, Deck associatedDeck)
        {
            Category = category;
            AssociatedDeck = associatedDeck;
        }
    }

    class Deck : Queue<string> { }
}
