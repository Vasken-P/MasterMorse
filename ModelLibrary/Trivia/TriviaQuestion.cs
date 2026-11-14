namespace MasterMorse.Models.Trivia;

public sealed record TriviaQuestion(
    string Id,
    string Category,
    string Clue,
    string CorrectAnswer,
    IReadOnlyList<string> AcceptedAnswers)
{
    public bool IsCorrect(string answer)
    {
        var normalizedAnswer = answer.Trim();
        return string.Equals(normalizedAnswer, CorrectAnswer, StringComparison.OrdinalIgnoreCase)
            || AcceptedAnswers.Any(alternate =>
                string.Equals(normalizedAnswer, alternate.Trim(), StringComparison.OrdinalIgnoreCase));
    }
}

public sealed record TriviaBoardTile(string QuestionId, int ClueNumber, bool IsUsed)
{
    public bool CanSelect => !IsUsed;
}

public sealed record TriviaBoardCategory(string Name, IReadOnlyList<TriviaBoardTile> Tiles);

public sealed record TriviaAnswerResult(
    bool IsCorrect,
    string CorrectAnswer,
    int Score,
    int AnsweredQuestionCount,
    bool IsComplete);
