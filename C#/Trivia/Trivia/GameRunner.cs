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
            using (var stream1 = File.OpenWrite("expected.txt"))
            using (var writer1 = new StreamWriter(stream1))
            using (var stream2 = File.OpenWrite("code.txt"))
            using (var writer2 = new StreamWriter(stream2))
            {
                Console.SetOut(writer1);
                var aGame = new LegacyGameAdapter();

                aGame.AddPlayer("Chet");
                aGame.AddPlayer("Pat");
                aGame.AddPlayer("Sue");

                Random rand = new Random();

                do
                {
                    var roll = rand.Next(5) + 1;
                    aGame.Roll(roll);

                    if (rand.Next(9) == 7)
                    {
                        notAWinner = aGame.WrongAnswer();
                    }
                    else
                    {
                        notAWinner = aGame.CorrectAnswer();
                    }
                } while (notAWinner);
            }
        }
    }
}
