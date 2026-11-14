using MasterMorse.Models.Trivia;

namespace MasterMorse.Presenters.Trivia;

public interface ITriviaQuizView
{
    string Title { get; set; }

    event Action<string>? ClueSelected;
    event Action<string>? AnswerSubmitted;
    event Action? BoardRequested;
    event Action? ReturnToTitleRequested;

    void DisplayBoard(
        IReadOnlyList<TriviaBoardCategory> categories,
        int score,
        int answeredQuestionCount,
        int totalQuestionCount);
    void DisplayClue(TriviaQuestion question, int score, int answeredQuestionCount, int totalQuestionCount);
    void DisplayAnswerResult(TriviaAnswerResult result);
    void DisplayResults(int finalScore, double averageScore);
    void ShowQuestionBankError(string message);
}
