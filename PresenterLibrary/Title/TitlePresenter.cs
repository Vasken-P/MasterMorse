namespace MasterMorse.Presenters.Title;

public sealed class TitlePresenter
{
    private readonly ITitleView _view;
    private readonly Action _openGuidebook;
    private readonly Action _openJeopardyQuiz;

    public TitlePresenter(ITitleView view, Action openGuidebook, Action openJeopardyQuiz)
    {
        _view = view;
        _openGuidebook = openGuidebook;
        _openJeopardyQuiz = openJeopardyQuiz;
    }

    public void Initialize()
    {
        _view.Title = "MorseMaster";
        _view.GetStartedRequested += _openGuidebook;
        _view.JeopardyQuizRequested += _openJeopardyQuiz;
    }
}
