using MasterMorse.Models;
using MasterMorse.Views;

namespace MasterMorse.Presenters.Guidebook;

public sealed class GuidebookPresenter
{
    private readonly IGuidebookView _view;
    private readonly IGuidebookContentProvider _contentProvider;
    private MorsePracticeQuestion? _practiceQuestion;

    public GuidebookPresenter(IGuidebookView view, IGuidebookContentProvider contentProvider)
    {
        _view = view;
        _contentProvider = contentProvider;
    }

    public void Initialize()
    {
        _view.Title = "MasterMorse";
        var lessons = _contentProvider.GetLessons();

        if (lessons.Count == 0)
        {
            throw new InvalidOperationException("The guidebook must contain at least one lesson.");
        }

        _practiceQuestion = _contentProvider.GetPracticeQuestion();
        _view.PracticeAnswerSubmitted += SubmitPracticeAnswer;
        _view.DisplayLessons(lessons);
        _view.DisplayPracticeQuestion(_practiceQuestion.Prompt);
        _view.SetPracticeSubmissionEnabled(true);
    }

    private void SubmitPracticeAnswer(string answer)
    {
        var question = _practiceQuestion
            ?? throw new InvalidOperationException("The guidebook has not been initialized.");
        var isCorrect = question.IsCorrect(answer);

        _view.ShowPracticeFeedback(
            isCorrect ? question.CorrectFeedback : question.IncorrectFeedback,
            isCorrect);

        if (isCorrect)
        {
            _view.SetPracticeSubmissionEnabled(false);
        }
    }
}
