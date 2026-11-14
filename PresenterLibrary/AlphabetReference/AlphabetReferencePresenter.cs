using MasterMorse.Models;

namespace MasterMorse.Presenters.AlphabetReference;

public sealed class AlphabetReferencePresenter
{
    private readonly IAlphabetReferenceView _view;
    private readonly IAlphabetReferenceProvider _provider;
    private readonly IMorsePlaybackService _playbackService;

    public AlphabetReferencePresenter(
        IAlphabetReferenceView view,
        IAlphabetReferenceProvider provider,
        IMorsePlaybackService playbackService)
    {
        _view = view;
        _provider = provider;
        _playbackService = playbackService;
    }

    public void Initialize()
    {
        _view.Title = "MorseMaster - Alphabet reference";
        _view.ModeSelected += ShowMode;
        _view.EntrySelected += PlayEntry;
        ShowMode(ReferenceMode.Alphabet);
    }

    private void ShowMode(ReferenceMode mode)
    {
        _view.DisplaySections(_provider.GetSections(mode), mode);
    }

    private async void PlayEntry(MorseReferenceEntry entry)
    {
        _view.SetPlaybackEnabled(false);
        try
        {
            await _playbackService.PlayAsync(entry.Pattern);
        }
        catch (Exception exception) when (
            exception is PlatformNotSupportedException
                or System.ComponentModel.Win32Exception
                or InvalidOperationException)
        {
            _view.ShowPlaybackError(
                "Morse audio playback is unavailable. Check that the device supports audio beeps.");
        }
        finally
        {
            _view.SetPlaybackEnabled(true);
        }
    }
}
