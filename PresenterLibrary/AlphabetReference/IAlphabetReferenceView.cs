using MasterMorse.Models;

namespace MasterMorse.Presenters.AlphabetReference;

public interface IAlphabetReferenceView
{
    string Title { get; set; }
    event Action<ReferenceMode>? ModeSelected;
    void DisplayEntries(IReadOnlyList<MorseReferenceEntry> entries, ReferenceMode mode);
}
