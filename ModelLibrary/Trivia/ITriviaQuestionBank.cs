namespace MasterMorse.Models.Trivia;

public interface ITriviaQuestionBank
{
    IReadOnlyList<TriviaQuestion> LoadQuestions();
}
