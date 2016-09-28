using System;
using System.Collections.Generic;
using System.Linq;

namespace Trivia.Core
{
    public class GameV2 : IGame
    {
        private readonly List<string> players = new List<string>();

        private readonly int[] places = new int[6];
        private readonly int[] purses = new int[6];

        private readonly bool[] inPenaltyBox = new bool[6];

        private readonly Queue<string> popQuestions = new Queue<string>();
        private readonly Queue<string> scienceQuestions = new Queue<string>();
        private readonly Queue<string> sportsQuestions = new Queue<string>();
        private readonly Queue<string> rockQuestions = new Queue<string>();

        private int currentPlayer = 0;
        private bool isGettingOutOfPenaltyBox;

        public GameV2()
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

        public bool AddPlayer(string playerName)
        {
            players.Add(playerName);
            places[PlayerCount] = 0;
            purses[PlayerCount] = 0;
            inPenaltyBox[PlayerCount] = false;

            Console.WriteLine(playerName + " was added");
            Console.WriteLine("They are player number " + players.Count);
            return true;
        }

        public void Roll(int roll)
        {
            Console.WriteLine(players[currentPlayer] + " is the current player");
            Console.WriteLine("They have rolled a " + roll);

            if (inPenaltyBox[currentPlayer])
            {
                if (roll % 2 != 0)
                {
                    isGettingOutOfPenaltyBox = true;

                    Console.WriteLine(players[currentPlayer] + " is getting out of the penalty box");
                    places[currentPlayer] = places[currentPlayer] + roll;
                    if (places[currentPlayer] > 11) places[currentPlayer] = places[currentPlayer] - 12;

                    Console.WriteLine(players[currentPlayer]
                            + "'s new location is "
                            + places[currentPlayer]);
                    Console.WriteLine("The category is " + CurrentCategory());
                    AskQuestion();
                }
                else
                {
                    Console.WriteLine(players[currentPlayer] + " is not getting out of the penalty box");
                    isGettingOutOfPenaltyBox = false;
                }

            }
            else
            {
                places[currentPlayer] = places[currentPlayer] + roll;
                if (places[currentPlayer] > 11) places[currentPlayer] = places[currentPlayer] - 12;

                Console.WriteLine(players[currentPlayer]
                        + "'s new location is "
                        + places[currentPlayer]);
                Console.WriteLine("The category is " + CurrentCategory());
                AskQuestion();
            }
        }

        private void AskQuestion()
        {
            if (CurrentCategory() == "Pop")
            {
                Console.WriteLine(popQuestions.Dequeue());
            }
            if (CurrentCategory() == "Science")
            {
                Console.WriteLine(scienceQuestions.Dequeue());
            }
            if (CurrentCategory() == "Sports")
            {
                Console.WriteLine(sportsQuestions.Dequeue());
            }
            if (CurrentCategory() == "Rock")
            {
                Console.WriteLine(rockQuestions.Dequeue());
            }
        }


        private string CurrentCategory()
        {
            if (places[currentPlayer] == 0) return "Pop";
            if (places[currentPlayer] == 4) return "Pop";
            if (places[currentPlayer] == 8) return "Pop";
            if (places[currentPlayer] == 1) return "Science";
            if (places[currentPlayer] == 5) return "Science";
            if (places[currentPlayer] == 9) return "Science";
            if (places[currentPlayer] == 2) return "Sports";
            if (places[currentPlayer] == 6) return "Sports";
            if (places[currentPlayer] == 10) return "Sports";
            return "Rock";
        }

        public bool CorrectAnswer()
        {
            if (inPenaltyBox[currentPlayer])
            {
                if (isGettingOutOfPenaltyBox)
                {
                    Console.WriteLine("Answer was correct!!!!");
                    purses[currentPlayer]++;
                    Console.WriteLine(players[currentPlayer]
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
                Console.WriteLine("Answer was corrent!!!!");
                purses[currentPlayer]++;
                Console.WriteLine(players[currentPlayer]
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
            Console.WriteLine("Question was incorrectly answered");
            Console.WriteLine(players[currentPlayer] + " was sent to the penalty box");
            inPenaltyBox[currentPlayer] = true;

            currentPlayer++;
            if (currentPlayer == players.Count) currentPlayer = 0;
            return true;
        }

        private bool DidPlayerWin()
        {
            return purses[currentPlayer] != 6;
        }
    }
}
