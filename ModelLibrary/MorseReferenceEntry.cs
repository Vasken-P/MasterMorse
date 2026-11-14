namespace MasterMorse.Models;

public sealed record MorseReferenceEntry(string Label, string Pattern, string? Meaning);

public enum ReferenceMode
{
    Alphabet,
    Abbreviations
}
