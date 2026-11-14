namespace MasterMorse.Models.Trivia;

public interface ITriviaGameSession
{
    int Score { get; }
    int AnsweredQuestionCount { get; }
    int TotalQuestionCount { get; }
    bool IsComplete { get; }

    IReadOnlyList<TriviaBoardCategory> GetBoard();
    TriviaQuestion SelectQuestion(string questionId);
    TriviaAnswerResult SubmitAnswer(string answer);
}
