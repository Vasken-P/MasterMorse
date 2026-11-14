namespace MasterMorse.Models;

public interface IAlphabetReferenceProvider
{
    IReadOnlyList<MorseReferenceEntry> GetEntries(ReferenceMode mode);
}
