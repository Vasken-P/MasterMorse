namespace MasterMorse.Models.Trivia;

public sealed class JeopardyQuizGameFactory : ITriviaGameFactory
{
    public ITriviaGameSession Create(IReadOnlyList<TriviaQuestion> questionBank) =>
        new JeopardyQuizSession(questionBank);
}
