namespace MasterMorse.Models;

public interface IAlphabetReferenceProvider
{
    IReadOnlyList<AlphabetReferenceEntry> GetEntries();
}
