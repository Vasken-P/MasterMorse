namespace MasterMorse.Models.Trivia;

public sealed class TriviaQuizSession : ITriviaGameSession
{
    public const int QuestionsPerGame = 10;

    private readonly IReadOnlyList<TriviaQuestion> _questions;
    private int _currentQuestionIndex;
    private bool _currentQuestionAnswered;

    public TriviaQuizSession(IReadOnlyList<TriviaQuestion> questionBank, Random? random = null)
    {
        if (questionBank.Count < QuestionsPerGame)
        {
            throw new ArgumentException(
                $"At least {QuestionsPerGame} questions are required to start a quiz.",
                nameof(questionBank));
        }

        if (questionBank.Select(question => question.Id)
            .Distinct(StringComparer.OrdinalIgnoreCase).Count() != questionBank.Count)
        {
            throw new ArgumentException("Quiz question IDs must be unique.", nameof(questionBank));
        }

        if (questionBank.Any(question =>
                question.Answers.Count != 4
                || question.Answers.Any(string.IsNullOrWhiteSpace)
                || question.CorrectAnswerIndex is < 0 or >= 4))
        {
            throw new ArgumentException(
                "Each quiz question must have four answers and a correct-answer index from 0 to 3.",
                nameof(questionBank));
        }

        var shuffledQuestions = questionBank.ToArray();
        Shuffle(shuffledQuestions, random ?? Random.Shared);
        _questions = Array.AsReadOnly(shuffledQuestions.Take(QuestionsPerGame).ToArray());
    }

    public int Score { get; private set; }
    public int AnsweredQuestionCount => _currentQuestionIndex + (_currentQuestionAnswered ? 1 : 0);
    public int TotalQuestionCount => QuestionsPerGame;
    public bool IsComplete => AnsweredQuestionCount == QuestionsPerGame;

    public TriviaQuestion CurrentQuestion =>
        IsComplete
            ? throw new InvalidOperationException("The quiz is complete.")
            : _questions[_currentQuestionIndex];

    public TriviaAnswerResult SubmitAnswer(int answerIndex)
    {
        if (_currentQuestionAnswered)
        {
            throw new InvalidOperationException("This question has already been answered.");
        }

        if (answerIndex < 0 || answerIndex >= CurrentQuestion.Answers.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(answerIndex), "Select one of the available answers.");
        }

        var question = CurrentQuestion;
        var isCorrect = question.IsCorrect(answerIndex);
        Score += isCorrect ? 10 : -2;
        _currentQuestionAnswered = true;

        return new TriviaAnswerResult(
            isCorrect,
            question.CorrectAnswer,
            Score,
            AnsweredQuestionCount,
            IsComplete);
    }

    public TriviaQuestion AdvanceToNextQuestion()
    {
        if (!_currentQuestionAnswered)
        {
            throw new InvalidOperationException("Answer the current question before continuing.");
        }

        if (IsComplete)
        {
            throw new InvalidOperationException("The quiz is complete.");
        }

        _currentQuestionIndex++;
        _currentQuestionAnswered = false;
        return CurrentQuestion;
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
