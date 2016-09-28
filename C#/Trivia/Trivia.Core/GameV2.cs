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
                    Log("The category is " + CurrentCategory());
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
                Log("The category is " + CurrentCategory());
                AskQuestion();
            }
        }

        private void AskQuestion()
        {
            if (CurrentCategory() == "Pop")
            {
                Log(popQuestions.Dequeue());
            }
            if (CurrentCategory() == "Science")
            {
                Log(scienceQuestions.Dequeue());
            }
            if (CurrentCategory() == "Sports")
            {
                Log(sportsQuestions.Dequeue());
            }
            if (CurrentCategory() == "Rock")
            {
                Log(rockQuestions.Dequeue());
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
}
