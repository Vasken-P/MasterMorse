namespace MasterMorse.Models;

public interface IAlphabetReferenceProvider
{
    IReadOnlyList<MorseReferenceSection> GetSections(ReferenceMode mode);
}
