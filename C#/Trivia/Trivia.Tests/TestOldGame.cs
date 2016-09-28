using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Trivia.Core;
using Xunit;

namespace Trivia.Tests
{
    public class TestOldGame
    {
        [Fact]
        public void TestGame()
        {
            var game = new Game();

            using (var file = File.OpenWrite("subject.txt"))
            using (var writer = new StreamWriter(file))
            {
                Console.SetOut(writer);

                game.add("Chet");
                game.add("Pat");
                game.add("Sue");
                game.roll(4);
                game.wasCorrectlyAnswered();
                game.roll(5);
                game.wasCorrectlyAnswered();
                game.roll(2);
                game.wasCorrectlyAnswered();
                game.roll(4);
                game.wrongAnswer();
                game.roll(4);
                game.wasCorrectlyAnswered();
                game.roll(5);
                game.wasCorrectlyAnswered();
                game.roll(2);
                game.wasCorrectlyAnswered();
                game.roll(2);
                game.wasCorrectlyAnswered();
                game.roll(2);
                game.wasCorrectlyAnswered();
                game.roll(1);
                game.wasCorrectlyAnswered();
                game.roll(1);
                game.wasCorrectlyAnswered();
                game.roll(3);
                game.wrongAnswer();
                game.roll(3);
                game.wrongAnswer();
                game.roll(1);
                game.wasCorrectlyAnswered();
                game.roll(5);
                game.wasCorrectlyAnswered();
                game.roll(3);
                game.wasCorrectlyAnswered();
                game.roll(5);
                game.wasCorrectlyAnswered();
            }

            var subject = File.ReadAllLines("subject.txt").ToArray();

            File.WriteAllText("expected.txt", ExpectedContents);
            var expectedLines = File.ReadAllLines("expected.txt").ToArray();

            subject.ShouldAllBeEquivalentTo(expectedLines);
        }

        private const string ExpectedContents = 
@"Chet was added
They are player number 1
Pat was added
They are player number 2
Sue was added
They are player number 3
Chet is the current player
They have rolled a 4
Chet's new location is 4
The category is Pop
Pop Question 0
Answer was corrent!!!!
Chet now has 1 Gold Coins.
Pat is the current player
They have rolled a 5
Pat's new location is 5
The category is Science
Science Question 0
Answer was corrent!!!!
Pat now has 1 Gold Coins.
Sue is the current player
They have rolled a 2
Sue's new location is 2
The category is Sports
Sports Question 0
Answer was corrent!!!!
Sue now has 1 Gold Coins.
Chet is the current player
They have rolled a 4
Chet's new location is 8
The category is Pop
Pop Question 1
Question was incorrectly answered
Chet was sent to the penalty box
Pat is the current player
They have rolled a 4
Pat's new location is 9
The category is Science
Science Question 1
Answer was corrent!!!!
Pat now has 2 Gold Coins.
Sue is the current player
They have rolled a 5
Sue's new location is 7
The category is Rock
Rock Question 0
Answer was corrent!!!!
Sue now has 2 Gold Coins.
Chet is the current player
They have rolled a 2
Chet is not getting out of the penalty box
Pat is the current player
They have rolled a 2
Pat's new location is 11
The category is Rock
Rock Question 1
Answer was corrent!!!!
Pat now has 3 Gold Coins.
Sue is the current player
They have rolled a 2
Sue's new location is 9
The category is Science
Science Question 2
Answer was corrent!!!!
Sue now has 3 Gold Coins.
Chet is the current player
They have rolled a 1
Chet is getting out of the penalty box
Chet's new location is 9
The category is Science
Science Question 3
Answer was correct!!!!
Chet now has 2 Gold Coins.
Pat is the current player
They have rolled a 1
Pat's new location is 0
The category is Pop
Pop Question 2
Answer was corrent!!!!
Pat now has 4 Gold Coins.
Sue is the current player
They have rolled a 3
Sue's new location is 0
The category is Pop
Pop Question 3
Question was incorrectly answered
Sue was sent to the penalty box
Chet is the current player
They have rolled a 3
Chet is getting out of the penalty box
Chet's new location is 0
The category is Pop
Pop Question 4
Question was incorrectly answered
Chet was sent to the penalty box
Pat is the current player
They have rolled a 1
Pat's new location is 1
The category is Science
Science Question 4
Answer was corrent!!!!
Pat now has 5 Gold Coins.
Sue is the current player
They have rolled a 5
Sue is getting out of the penalty box
Sue's new location is 5
The category is Science
Science Question 5
Answer was correct!!!!
Sue now has 4 Gold Coins.
Chet is the current player
They have rolled a 3
Chet is getting out of the penalty box
Chet's new location is 3
The category is Rock
Rock Question 2
Answer was correct!!!!
Chet now has 3 Gold Coins.
Pat is the current player
They have rolled a 5
Pat's new location is 6
The category is Sports
Sports Question 1
Answer was corrent!!!!
Pat now has 6 Gold Coins.";
    }
}
