using MasterMorse.Views;

namespace MasterMorse.Presenters;

public sealed class MainPresenter
{
    private readonly IMainView _view;

    public MainPresenter(IMainView view)
    {
        _view = view;
    }

    public void Initialize()
    {
        _view.Title = "MasterMorse";
    }
}
