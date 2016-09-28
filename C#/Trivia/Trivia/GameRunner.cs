using System;
using System.IO;
using Trivia.Core;

namespace Trivia
{
    public class GameRunner
    {


        public static void Main(string[] args)
        {
            var aGame = new LegacyGameAdapter();

            aGame.AddPlayer("Chet");
            aGame.AddPlayer("Pat");
            aGame.AddPlayer("Sue");

            Random rand = new Random();

            bool gameEnded = false;

            do
            {
                var roll = rand.Next(5) + 1;
                aGame.Roll(roll);

                if (rand.Next(9) == 7)
                {
                    aGame.WrongAnswer();
                }
                else
                {
                    gameEnded = aGame.CorrectAnswer();
                }
            } while (!gameEnded);
        }
    }
}
