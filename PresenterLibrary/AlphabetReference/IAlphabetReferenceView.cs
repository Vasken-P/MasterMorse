using MasterMorse.Models;

namespace MasterMorse.Presenters.AlphabetReference;

public interface IAlphabetReferenceView
{
    string Title { get; set; }
    void DisplayEntries(IReadOnlyList<AlphabetReferenceEntry> entries);
}
