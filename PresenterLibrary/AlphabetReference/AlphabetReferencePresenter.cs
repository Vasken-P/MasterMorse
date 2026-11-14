using MasterMorse.Models;

namespace MasterMorse.Presenters.AlphabetReference;

public sealed class AlphabetReferencePresenter
{
    private readonly IAlphabetReferenceView _view;
    private readonly IAlphabetReferenceProvider _provider;

    public AlphabetReferencePresenter(IAlphabetReferenceView view, IAlphabetReferenceProvider provider)
    {
        _view = view;
        _provider = provider;
    }

    public void Initialize()
    {
        _view.Title = "MorseMaster - Alphabet reference";
        _view.DisplayEntries(_provider.GetEntries());
    }
}
