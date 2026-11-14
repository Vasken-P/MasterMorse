using MasterMorse.Models;

namespace MasterMorse.Views;

public interface IGuidebookView
{
    string Title { get; set; }

    event Action? PreviousLessonRequested;
    event Action? NextLessonRequested;
    event Action<string>? PracticeAnswerSubmitted;

    void DisplayLesson(GuidebookLesson lesson, int lessonNumber, int totalLessons);
    void ShowPracticeFeedback(string message, bool isCorrect);
    void SetNavigationEnabled(bool canGoPrevious, bool canGoNext);
    void SetPracticeSubmissionEnabled(bool isEnabled);
}
