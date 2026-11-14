using System.Text.Json;

namespace MasterMorse.Models.Trivia;

public sealed class JsonTriviaQuestionBank : ITriviaQuestionBank
{
    private const int MinimumQuestionCount = 30;
    private readonly string _filePath;

    public JsonTriviaQuestionBank(string? filePath = null)
    {
        _filePath = filePath ?? Path.Combine(AppContext.BaseDirectory, "lib", "questions.json");
    }

    public IReadOnlyList<TriviaQuestion> LoadQuestions()
    {
        var json = File.ReadAllText(_filePath);
        if (string.IsNullOrWhiteSpace(json))
        {
            throw new InvalidDataException("The trivia question file is empty.");
        }

        var document = JsonSerializer.Deserialize<QuestionBankDocument>(
            json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        var questions = document?.Questions;

        if (questions is null || questions.Count < MinimumQuestionCount)
        {
            throw new InvalidDataException(
                $"The trivia question file must contain at least {MinimumQuestionCount} questions.");
        }

        if (questions.Any(question =>
                question is null
                || string.IsNullOrWhiteSpace(question.Id)
                || string.IsNullOrWhiteSpace(question.Category)
                || string.IsNullOrWhiteSpace(question.Clue)
                || string.IsNullOrWhiteSpace(question.CorrectAnswer)
                || question.AcceptedAnswers is null
                || question.AcceptedAnswers.Any(string.IsNullOrWhiteSpace)))
        {
            throw new InvalidDataException(
                "Each trivia question must have an ID, category, clue, correct answer, and valid alternate answers.");
        }

        if (questions.Select(question => question.Id).Distinct(StringComparer.OrdinalIgnoreCase).Count()
            != questions.Count)
        {
            throw new InvalidDataException("Trivia question IDs must be unique.");
        }

        return questions.AsReadOnly();
    }

    private sealed class QuestionBankDocument
    {
        public List<TriviaQuestion>? Questions { get; init; }
    }
}
