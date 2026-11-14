namespace MasterMorse.Models.Trivia;

public sealed class TriviaQuizSessionFactory : ITriviaGameFactory
{
    public ITriviaGameSession Create(IReadOnlyList<TriviaQuestion> questionBank) =>
        new TriviaQuizSession(questionBank);
}
