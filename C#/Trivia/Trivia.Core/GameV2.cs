using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Trivia.Core
{
    public class GameV2 : IGame
    {
        private readonly List<string> players = new List<string>();

        private readonly int[] places = new int[6];
        private readonly int[] purses = new int[6];

        private readonly bool[] inPenaltyBox = new bool[6];

        private readonly Deck popQuestions = new Deck();
        private readonly Deck scienceQuestions = new Deck();
        private readonly Deck sportsQuestions = new Deck();
        private readonly Deck rockQuestions = new Deck();
        private readonly Board board;

        private int currentPlayer = 0;

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

        public void AddPlayer(string playerName)
        {
            players.Add(playerName);
            Log(playerName + " was added");
            Log("They are player number " + players.Count);
        }

        public void Roll(int roll)
        {
            Log(players[currentPlayer] + " is the current player");
            Log("They have rolled a " + roll);

            if (inPenaltyBox[currentPlayer])
            {
                if (roll % 2 != 0)
                {
                    isGettingOutOfPenaltyBox = true;

                    Log(players[currentPlayer] + " is getting out of the penalty box");
                    places[currentPlayer] = places[currentPlayer] + roll;
                    if (places[currentPlayer] > 11) places[currentPlayer] = places[currentPlayer] - 12;

                    Log(players[currentPlayer]
                            + "'s new location is "
                            + places[currentPlayer]);
                    Log("The category is " + board[places[currentPlayer]].Category);
                    AskQuestion();
                }
                else
                {
                    Log(players[currentPlayer] + " is not getting out of the penalty box");
                    isGettingOutOfPenaltyBox = false;
                }
            }
            else
            {
                places[currentPlayer] = places[currentPlayer] + roll;
                if (places[currentPlayer] > 11) places[currentPlayer] = places[currentPlayer] - 12;

                Log(players[currentPlayer]
                        + "'s new location is "
                        + places[currentPlayer]);
                Log("The category is " + board[places[currentPlayer]].Category);
                AskQuestion();
            }
        }

        private void AskQuestion()
        {
            var card = board[places[currentPlayer]].AssociatedDeck.Dequeue();
            Log(card);
        }

        public bool CorrectAnswer()
        {
            if (inPenaltyBox[currentPlayer])
            {
                if (isGettingOutOfPenaltyBox)
                {
                    Log("Answer was correct!!!!");
                    purses[currentPlayer]++;
                    Log(players[currentPlayer]
                            + " now has "
                            + purses[currentPlayer]
                            + " Gold Coins.");

                    bool winner = DidPlayerWin();
                    currentPlayer++;
                    if (currentPlayer == players.Count) currentPlayer = 0;

                    return winner;
                }
                else
                {
                    currentPlayer++;
                    if (currentPlayer == players.Count) currentPlayer = 0;
                    return true;
                }
            }
            else
            {
                Log("Answer was corrent!!!!");
                purses[currentPlayer]++;
                Log(players[currentPlayer]
                        + " now has "
                        + purses[currentPlayer]
                        + " Gold Coins.");

                bool winner = DidPlayerWin();
                currentPlayer++;
                if (currentPlayer == players.Count) currentPlayer = 0;

                return winner;
            }
        }

        public bool WrongAnswer()
        {
            Log("Question was incorrectly answered");
            Log(players[currentPlayer] + " was sent to the penalty box");
            inPenaltyBox[currentPlayer] = true;

            currentPlayer++;
            if (currentPlayer == players.Count) currentPlayer = 0;
            return true;
        }

        private bool DidPlayerWin()
        {
            return purses[currentPlayer] != 6;
        }

        private void Log(string message)
        {
            Console.WriteLine(message);
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
