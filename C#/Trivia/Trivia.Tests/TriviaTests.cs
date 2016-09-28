using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using Trivia.Core;
using Xunit;

namespace Trivia.Tests
{
    public class TriviaTests
    {
        [Fact]
        public void TestLegacyGame()
        {
            using (var file = File.OpenWrite("subject.txt"))
            using (var writer = new StreamWriter(file))
            {
                Console.SetOut(writer);
                Helper.Run(new LegacyGameAdapter());
            }

            var subject = File.ReadAllLines("subject.txt").ToArray();

            File.WriteAllText("expected.txt", Helper.ExpectedContents);
            var expectedLines = File.ReadAllLines("expected.txt").ToArray();

            subject.ShouldAllBeEquivalentTo(expectedLines);
        }

        [Fact]
        public void TestV2MatchLegacyGameResults()
        {
            using (var file = File.Create("legacy.txt"))
            using (var writer = new StreamWriter(file))
            {
                Console.SetOut(writer);
                Helper.Run(new LegacyGameAdapter());
            }

            using (var file = File.Create("v2.txt"))
            using (var writer = new StreamWriter(file))
            {
                Console.SetOut(writer);
                Helper.Run(new GameV2());
            }

            var subject = File.ReadAllLines("v2.txt").ToArray();
            var expected = File.ReadAllLines("legacy.txt").ToArray();

            subject.ShouldAllBeEquivalentTo(expected);
        }
    }
}
