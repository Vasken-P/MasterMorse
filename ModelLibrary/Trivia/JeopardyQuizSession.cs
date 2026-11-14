namespace MasterMorse.Models.Trivia;

public sealed class JeopardyQuizSession : ITriviaGameSession
{
    public const int QuestionsPerGame = 10;

    private readonly IReadOnlyList<TriviaQuestion> _questions;
    private readonly HashSet<string> _usedQuestionIds = new(StringComparer.OrdinalIgnoreCase);
    private TriviaQuestion? _selectedQuestion;

    public JeopardyQuizSession(IReadOnlyList<TriviaQuestion> questionBank, Random? random = null)
    {
        if (questionBank.Count < QuestionsPerGame)
        {
            throw new ArgumentException(
                $"At least {QuestionsPerGame} questions are required to start a game.",
                nameof(questionBank));
        }

        if (questionBank.Select(question => question.Id)
            .Distinct(StringComparer.OrdinalIgnoreCase).Count() != questionBank.Count)
        {
            throw new ArgumentException("Trivia question IDs must be unique.", nameof(questionBank));
        }

        var shuffledQuestions = questionBank.ToArray();
        Shuffle(shuffledQuestions, random ?? Random.Shared);
        _questions = Array.AsReadOnly(shuffledQuestions.Take(QuestionsPerGame).ToArray());
    }

    public int Score { get; private set; }
    public int AnsweredQuestionCount => _usedQuestionIds.Count;
    public int TotalQuestionCount => QuestionsPerGame;
    public bool IsComplete => AnsweredQuestionCount == QuestionsPerGame;

    public IReadOnlyList<TriviaBoardCategory> GetBoard() =>
        Array.AsReadOnly(_questions
            .GroupBy(question => question.Category)
            .Select(group => new TriviaBoardCategory(
                group.Key,
                Array.AsReadOnly(group.Select((question, index) =>
                    new TriviaBoardTile(
                        question.Id,
                        index + 1,
                        _usedQuestionIds.Contains(question.Id))).ToArray())))
            .ToArray());

    public TriviaQuestion SelectQuestion(string questionId)
    {
        if (IsComplete)
        {
            throw new InvalidOperationException("The quiz is complete.");
        }

        if (_usedQuestionIds.Contains(questionId))
        {
            throw new InvalidOperationException("This clue has already been answered.");
        }

        _selectedQuestion = _questions.FirstOrDefault(question =>
            string.Equals(question.Id, questionId, StringComparison.OrdinalIgnoreCase))
            ?? throw new ArgumentException("The selected clue does not belong to this game.", nameof(questionId));

        return _selectedQuestion;
    }

    public TriviaAnswerResult SubmitAnswer(string answer)
    {
        var question = _selectedQuestion
            ?? throw new InvalidOperationException("Select a clue before submitting an answer.");

        var isCorrect = question.IsCorrect(answer);
        Score += isCorrect ? 10 : -2;
        _usedQuestionIds.Add(question.Id);
        _selectedQuestion = null;

        return new TriviaAnswerResult(
            isCorrect,
            question.CorrectAnswer,
            Score,
            AnsweredQuestionCount,
            IsComplete);
    }

    private static void Shuffle<T>(T[] items, Random random)
    {
        for (var index = items.Length - 1; index > 0; index--)
        {
            var otherIndex = random.Next(index + 1);
            (items[index], items[otherIndex]) = (items[otherIndex], items[index]);
        }
    }
}
