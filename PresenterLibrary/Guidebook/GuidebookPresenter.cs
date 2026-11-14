using MasterMorse.Models;
using MasterMorse.Views;

namespace MasterMorse.Presenters.Guidebook;

public sealed class GuidebookPresenter
{
    private readonly IGuidebookView _view;
    private readonly IGuidebookContentProvider _contentProvider;
    private IReadOnlyList<GuidebookLesson> _lessons = Array.Empty<GuidebookLesson>();
    private int _currentLessonIndex;

    public GuidebookPresenter(IGuidebookView view, IGuidebookContentProvider contentProvider)
    {
        _view = view;
        _contentProvider = contentProvider;
    }

    public void Initialize()
    {
        _view.Title = "MasterMorse";
        _lessons = _contentProvider.GetLessons();

        if (_lessons.Count == 0)
        {
            throw new InvalidOperationException("The guidebook must contain at least one lesson.");
        }

        _view.PreviousLessonRequested += ShowPreviousLesson;
        _view.NextLessonRequested += ShowNextLesson;
        _view.PracticeAnswerSubmitted += SubmitPracticeAnswer;
        ShowCurrentLesson();
    }

    private void ShowPreviousLesson()
    {
        if (_currentLessonIndex > 0)
        {
            _currentLessonIndex--;
            ShowCurrentLesson();
        }
    }

    private void ShowNextLesson()
    {
        if (_currentLessonIndex < _lessons.Count - 1)
        {
            _currentLessonIndex++;
            ShowCurrentLesson();
        }
    }

    private void SubmitPracticeAnswer(string answer)
    {
        var question = _lessons[_currentLessonIndex].PracticeQuestion;
        var isCorrect = question.IsCorrect(answer);

        _view.ShowPracticeFeedback(
            isCorrect ? question.CorrectFeedback : question.IncorrectFeedback,
            isCorrect);

        if (isCorrect)
        {
            _view.SetPracticeSubmissionEnabled(false);
        }
    }

    private void ShowCurrentLesson()
    {
        var lesson = _lessons[_currentLessonIndex];
        _view.DisplayLesson(lesson, _currentLessonIndex + 1, _lessons.Count);
        _view.SetNavigationEnabled(
            canGoPrevious: _currentLessonIndex > 0,
            canGoNext: _currentLessonIndex < _lessons.Count - 1);
        _view.SetPracticeSubmissionEnabled(true);
    }
}
