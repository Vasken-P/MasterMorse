using MasterMorse.Models.Trivia;

namespace MasterMorse.Presenters.Trivia;

public interface ITriviaQuizView
{
    string Title { get; set; }

    event Action<int>? AnswerSelected;
    event Action<int>? AnswerSubmitted;
    event Action? NextQuestionRequested;
    event Action? ReturnToTitleRequested;

    void DisplayQuestion(TriviaQuestion question, int questionNumber, int totalQuestionCount, int score);
    void SetAnswerSubmissionEnabled(bool isEnabled);
    void DisplayAnswerResult(TriviaAnswerResult result);
    void DisplayResults(int finalScore, double averageScore);
    void ShowQuestionBankError(string message);
}
