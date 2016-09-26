using System;
using System.IO;
using Trivia.Core;

namespace Trivia
{
    public class GameRunner
    {

        private static bool notAWinner;

        public static void Main(string[] args)
        {
            using (var stream = File.OpenWrite("output.txt"))
            using (var writer = new StreamWriter(stream))
            {
                Console.SetOut(writer);
                Game aGame = new Game();

                aGame.add("Chet");
                aGame.add("Pat");
                aGame.add("Sue");

                Random rand = new Random();

                do
                {
                    aGame.roll(rand.Next(5) + 1);

                    if (rand.Next(9) == 7)
                    {
                        notAWinner = aGame.wrongAnswer();
                    }
                    else
                    {
                        notAWinner = aGame.wasCorrectlyAnswered();
                    }
                } while (notAWinner);
            }
        }
    }
}

