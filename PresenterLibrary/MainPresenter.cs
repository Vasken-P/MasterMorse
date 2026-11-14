using MasterMorse.Infrastructure;
using MasterMorse.Model;
using MasterMorse.Views;

namespace MasterMorse.Presenters;

public enum PlaybackState
{
    Stopped,
    Playing,
    Paused,
}

/// <summary>Coordinates the view, the Morse model and the infrastructure services.</summary>
public sealed class MainPresenter
{
    private readonly IMainView _view;
    private readonly IMorsePlayer _player;
    private readonly IClipboardService _clipboard;
    private readonly MorseTranslator _translator;
    private bool _isTextToMorse = true;
    private PlaybackState _state = PlaybackState.Stopped;
    private int _playbackSession;

    public MainPresenter(
        IMainView view,
        IMorsePlayer player,
        IClipboardService clipboard,
        MorseTranslator? translator = null)
    {
        _view = view;
        _player = player;
        _clipboard = clipboard;
        _translator = translator ?? new MorseTranslator();

        _view.InputChanged += Translate;
        _view.CommandRequested += HandleCommand;
    }

    public bool IsTextToMorse => _isTextToMorse;

    public PlaybackState State => _state;

    public void Initialize()
    {
        _view.Title = "Morse Code Translator";
        _view.PauseButtonText = "Pause";
        _view.StatusMessage = "Ready.";
        ApplyDirectionLabels();
    }

    /// <summary>Translates the input in the current direction and updates the output and messages.</summary>
    public void Translate(string input)
    {
        var result = _isTextToMorse
            ? _translator.TextToMorse(input)
            : _translator.MorseToText(MorseDisplay.FromDisplay(input));

        _view.OutputText = _isTextToMorse
            ? MorseDisplay.ToDisplay(result.Output)
            : result.Output;
        _view.ValidationMessage = result.ErrorMessage;
    }

    private void HandleCommand(MainCommand command)
    {
        switch (command)
        {
            case MainCommand.Translate:
                Translate(_view.InputText);
                break;
            case MainCommand.Swap:
                SwapDirection();
                break;
            case MainCommand.Clear:
                Clear();
                break;
            case MainCommand.Copy:
                CopyOutput();
                break;
            case MainCommand.Play:
                _ = PlayAsync();
                break;
            case MainCommand.Pause:
                TogglePause();
                break;
            case MainCommand.Stop:
                StopPlayback();
                break;
        }
    }

    private void SwapDirection()
    {
        StopPlayback();

        _isTextToMorse = !_isTextToMorse;
        var newInput = _view.OutputText;
        ApplyDirectionLabels();
        _view.InputText = newInput;
        Translate(newInput);
        _view.StatusMessage = "Direction swapped.";
    }

    private void Clear()
    {
        StopPlayback();

        _view.InputText = string.Empty;
        _view.OutputText = string.Empty;
        _view.ValidationMessage = string.Empty;
        _view.StatusMessage = "Cleared.";
    }

    private void CopyOutput()
    {
        if (string.IsNullOrEmpty(_view.OutputText))
        {
            _view.StatusMessage = "Nothing to copy.";
            return;
        }

        _view.StatusMessage = _clipboard.TrySetText(_view.OutputText)
            ? "Copied to clipboard."
            : "Could not access the clipboard.";
    }

    private async Task PlayAsync()
    {
        if (_state == PlaybackState.Paused)
        {
            TogglePause();
            return;
        }

        if (_state == PlaybackState.Playing)
        {
            return;
        }

        var morse = MorseDisplay.FromDisplay(_isTextToMorse ? _view.OutputText : _view.InputText);
        if (string.IsNullOrWhiteSpace(morse))
        {
            _view.StatusMessage = "Nothing to play.";
            return;
        }

        var check = _translator.MorseToText(morse);
        if (check.HasErrors)
        {
            _view.ValidationMessage = check.ErrorMessage;
            _view.StatusMessage = "Cannot play invalid Morse code.";
            return;
        }

        var signals = MorseSignalBuilder.Build(morse);
        var session = ++_playbackSession;
        _state = PlaybackState.Playing;
        _view.PauseButtonText = "Pause";
        _view.StatusMessage = "Playing...";

        await _player.PlayAsync(signals);

        // A stop or a newer playback has already updated the state.
        if (session != _playbackSession)
        {
            return;
        }

        _state = PlaybackState.Stopped;
        _view.PauseButtonText = "Pause";
        _view.StatusMessage = "Playback finished.";
    }

    private void TogglePause()
    {
        switch (_state)
        {
            case PlaybackState.Playing:
                _player.Pause();
                _state = PlaybackState.Paused;
                _view.PauseButtonText = "Resume";
                _view.StatusMessage = "Paused.";
                break;
            case PlaybackState.Paused:
                _player.Resume();
                _state = PlaybackState.Playing;
                _view.PauseButtonText = "Pause";
                _view.StatusMessage = "Playing...";
                break;
        }
    }

    private void StopPlayback()
    {
        if (_state == PlaybackState.Stopped)
        {
            return;
        }

        _playbackSession++;
        _player.Stop();
        _state = PlaybackState.Stopped;
        _view.PauseButtonText = "Pause";
        _view.StatusMessage = "Playback stopped.";
    }

    private void ApplyDirectionLabels()
    {
        _view.IsMorseInput = !_isTextToMorse;
        _view.InputHeader = _isTextToMorse ? "Text input" : "Morse code input";
        _view.OutputHeader = _isTextToMorse ? "Morse code output" : "Text output";
    }
}
