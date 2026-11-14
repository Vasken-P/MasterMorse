namespace MasterMorse.Models.Trivia;

public interface ITriviaQuestionBank
{
    IReadOnlyList<TriviaQuestion> LoadQuestions();
}

public interface ITriviaGameSession
{
    int Score { get; }
    int AnsweredQuestionCount { get; }
    bool IsComplete { get; }

    IReadOnlyList<TriviaBoardCategory> GetBoard();
    TriviaQuestion SelectQuestion(string questionId);
    TriviaAnswerResult SubmitAnswer(string answer);
}
