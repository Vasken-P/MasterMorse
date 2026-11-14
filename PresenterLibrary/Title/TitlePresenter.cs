namespace MasterMorse.Presenters.Title;

public sealed class TitlePresenter
{
    private readonly ITitleView _view;
    private readonly Action _openGuidebook;

    public TitlePresenter(ITitleView view, Action openGuidebook)
    {
        _view = view;
        _openGuidebook = openGuidebook;
    }

    public void Initialize()
    {
        _view.Title = "MorseMaster";
        _view.GetStartedRequested += _openGuidebook;
    }
}
