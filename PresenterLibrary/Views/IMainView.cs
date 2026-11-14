using MasterMorse.Model;

namespace MasterMorse.Views;

/// <summary>The view contract. The view raises events; the presenter updates its properties.</summary>
public interface IMainView
{
    event Action<string>? InputChanged;

    event Action<MainCommand>? CommandRequested;

    /// <summary>The user asked to open a 1v1 duel in the given mode. The composition root creates that window.</summary>
    event Action<Difficulty>? DuelRequested;

    string Title { get; set; }

    string InputHeader { get; set; }

    string OutputHeader { get; set; }

    string InputText { get; set; }

    string OutputText { get; set; }

    string ValidationMessage { get; set; }

    string StatusMessage { get; set; }

    string PauseButtonText { get; set; }

    /// <summary>True when the input is Morse code, so typed dots should be shown as the large dot.</summary>
    bool IsMorseInput { get; set; }
}
