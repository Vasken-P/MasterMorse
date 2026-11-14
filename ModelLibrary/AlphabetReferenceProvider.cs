namespace MasterMorse.Models;

public sealed class AlphabetReferenceProvider : IAlphabetReferenceProvider
{
    private static readonly IReadOnlyList<AlphabetReferenceEntry> Entries = Array.AsReadOnly(
    [
        CreateEntry('A', "·–"),
        CreateEntry('B', "–···"),
        CreateEntry('C', "–·–·"),
        CreateEntry('D', "–··"),
        CreateEntry('E', "·"),
        CreateEntry('F', "··–·"),
        CreateEntry('G', "––·"),
        CreateEntry('H', "····"),
        CreateEntry('I', "··"),
        CreateEntry('J', "·–––"),
        CreateEntry('K', "–·–"),
        CreateEntry('L', "·–··"),
        CreateEntry('M', "––"),
        CreateEntry('N', "–·"),
        CreateEntry('O', "–––"),
        CreateEntry('P', "·––·"),
        CreateEntry('Q', "––·–"),
        CreateEntry('R', "·–·"),
        CreateEntry('S', "···"),
        CreateEntry('T', "–"),
        CreateEntry('U', "··–"),
        CreateEntry('V', "···–"),
        CreateEntry('W', "·––"),
        CreateEntry('X', "–··–"),
        CreateEntry('Y', "–·––"),
        CreateEntry('Z', "––··")
    ]);

    public IReadOnlyList<AlphabetReferenceEntry> GetEntries() => Entries;

    private static AlphabetReferenceEntry CreateEntry(char character, string pattern) =>
        new(character, pattern);
}
