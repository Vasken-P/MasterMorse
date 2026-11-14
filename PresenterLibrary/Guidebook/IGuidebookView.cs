using MasterMorse.Models;

namespace MasterMorse.Views;

public interface IGuidebookView
{
    string Title { get; set; }

    event Action<string>? PracticeAnswerSubmitted;

    void DisplayLessons(IReadOnlyList<GuidebookLesson> lessons);
    void DisplayPracticeQuestion(string prompt);
    void ShowPracticeFeedback(string message, bool isCorrect);
    void SetPracticeSubmissionEnabled(bool isEnabled);
}
