namespace MasterMorse.Models;

public sealed record MorseReferenceSection(string Title, IReadOnlyList<MorseReferenceEntry> Entries);
