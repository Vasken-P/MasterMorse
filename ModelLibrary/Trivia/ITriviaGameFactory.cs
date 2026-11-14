namespace MasterMorse.Models.Trivia;

public interface ITriviaGameFactory
{
    ITriviaGameSession Create(IReadOnlyList<TriviaQuestion> questionBank);
}
