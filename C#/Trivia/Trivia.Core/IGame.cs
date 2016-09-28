namespace Trivia.Core
{
    public interface IGame
    {
        bool AddPlayer(string name);
        void Roll(int value);
        bool WrongAnswer();
        bool CorrectAnswer();
    }

    public class LegacyGameAdapter : IGame
    {
        private readonly Game game;

        public LegacyGameAdapter()
        {
            game = new Game();
        }

        public bool AddPlayer(string name)
        {
            return game.add(name);
        }

        public void Roll(int value)
        {
            game.roll(value);
        }

        public bool WrongAnswer()
        {
            return game.wrongAnswer();
        }

        public bool CorrectAnswer()
        {
            return game.wasCorrectlyAnswered();
        }
    }
}