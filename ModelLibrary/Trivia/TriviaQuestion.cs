namespace MasterMorse.Models.Trivia;

public sealed record TriviaQuestion(
    string Id,
    string Question,
    IReadOnlyList<string> Answers,
    int CorrectAnswerIndex)
{
    public string CorrectAnswer => Answers[CorrectAnswerIndex];

    public bool IsCorrect(int answerIndex) => answerIndex == CorrectAnswerIndex;
}

public sealed record TriviaAnswerResult(
    bool IsCorrect,
    string CorrectAnswer,
    int Score,
    int AnsweredQuestionCount,
    bool IsComplete);
