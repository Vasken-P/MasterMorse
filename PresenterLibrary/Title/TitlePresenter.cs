namespace MasterMorse.Presenters.Title;

public sealed class TitlePresenter
{
    private readonly ITitleView _view;
    private readonly Action _openGuidebook;
    private readonly Action _openQuiz;

    public TitlePresenter(ITitleView view, Action openGuidebook, Action openQuiz)
    {
        _view = view;
        _openGuidebook = openGuidebook;
        _openQuiz = openQuiz;
    }

    public void Initialize()
    {
        _view.Title = "MorseMaster";
        _view.GetStartedRequested += _openGuidebook;
        _view.QuizRequested += _openQuiz;
    }
}
