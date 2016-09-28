namespace Trivia.Core
{
    public interface IGame
    {
        void AddPlayer(string name);
        void Roll(int value);
        void WrongAnswer();
        bool CorrectAnswer();
    }

    public class LegacyGameAdapter : IGame
    {
        private readonly Game game;

        public LegacyGameAdapter()
        {
            game = new Game();
        }

        public void AddPlayer(string name)
        {
            game.add(name);
        }

        public void Roll(int value)
        {
            game.roll(value);
        }

        public void WrongAnswer()
        {
            game.wrongAnswer();
        }

        public bool CorrectAnswer()
        {
            return !game.wasCorrectlyAnswered();
        }
    }
}