namespace MasterMorse.Models;

public sealed record GuidebookLesson(
    string Title,
    string Explanation,
    IReadOnlyList<MorseExample> Examples);

public sealed record MorseExample(char Character, string Pattern, string Reading);

public sealed record MorsePracticeQuestion(
    string Prompt,
    string ExpectedAnswer,
    string CorrectFeedback,
    string IncorrectFeedback)
{
    public bool IsCorrect(string answer) =>
        string.Equals(answer.Trim(), ExpectedAnswer, StringComparison.OrdinalIgnoreCase);
}
