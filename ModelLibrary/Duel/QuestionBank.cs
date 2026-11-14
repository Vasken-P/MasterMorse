namespace MasterMorse.Model;

/// <summary>A phrase and the Morse abbreviation players must key for it.</summary>
public sealed record PhraseAbbreviation(string Phrase, string Abbreviation);

/// <summary>
/// Question data for the abbreviation rounds. To add a question, add a row here; the game logic does not change.
/// Abbreviations may use any character that <see cref="MorseAlphabet"/> supports.
/// Letter questions come from <see cref="MorseAlphabet.Entries"/>, so new letters are picked up automatically.
/// </summary>
public static class QuestionBank
{
    public static IReadOnlyList<PhraseAbbreviation> Abbreviations { get; } = new PhraseAbbreviation[]
    {
        new("Goodbye", "GB"),
        new("Best regards", "73"),
        new("See you later", "CUL"),
        new("Received", "R"),
        new("Go ahead", "K"),
        new("Break", "BK"),
        new("Good morning", "GM"),
        new("Good evening", "GE"),
        new("Good night", "GN"),
        new("Thank you", "TU"),
        new("Love and kisses", "88"),
        new("Wait", "AS"),
    };
}
