using MasterMorse.Models;

namespace MasterMorse.Presenters.AlphabetReference;

public interface IAlphabetReferenceView
{
    string Title { get; set; }
    event Action<ReferenceMode>? ModeSelected;
    event Action<MorseReferenceEntry>? EntrySelected;
    void DisplaySections(IReadOnlyList<MorseReferenceSection> sections, ReferenceMode mode);
    void SetPlaybackEnabled(bool isEnabled);
    void ShowPlaybackError(string message);
}
