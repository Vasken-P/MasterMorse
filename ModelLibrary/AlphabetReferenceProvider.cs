namespace MasterMorse.Models;

public sealed class AlphabetReferenceProvider : IAlphabetReferenceProvider
{
    private static readonly IReadOnlyList<MorseReferenceEntry> AlphabetEntries = Array.AsReadOnly(
    [
        CreateEntry("A", ".-"),
        CreateEntry("B", "-..."),
        CreateEntry("C", "-.-."),
        CreateEntry("D", "-.."),
        CreateEntry("E", "."),
        CreateEntry("F", "..-."),
        CreateEntry("G", "--."),
        CreateEntry("H", "...."),
        CreateEntry("I", ".."),
        CreateEntry("J", ".---"),
        CreateEntry("K", "-.-"),
        CreateEntry("L", ".-.."),
        CreateEntry("M", "--"),
        CreateEntry("N", "-."),
        CreateEntry("O", "---"),
        CreateEntry("P", ".--."),
        CreateEntry("Q", "--.-"),
        CreateEntry("R", ".-."),
        CreateEntry("S", "..."),
        CreateEntry("T", "-"),
        CreateEntry("U", "..-"),
        CreateEntry("V", "...-"),
        CreateEntry("W", ".--"),
        CreateEntry("X", "-..-"),
        CreateEntry("Y", "-.--"),
        CreateEntry("Z", "--..")
    ]);

    private static readonly IReadOnlyList<MorseReferenceEntry> AbbreviationEntries = Array.AsReadOnly(
    [
        CreateEntry("SOS", "... --- ...", "Universal distress signal"),
        CreateEntry("CQ", "-.-. --.-", "Calling any station"),
        CreateEntry("73", "--... ...--", "Best regards / Goodbye"),
        CreateEntry("88", "---.. ---..", "Love and kisses"),
        CreateEntry("K", "-.-", "Over / Go ahead; invitation to transmit"),
        CreateEntry("R", ".-.", "Roger / Received"),
        CreateEntry("TNX", "- -. -..-", "Thanks"),
        CreateEntry("QTH", "--.- - ....", "What is your location?"),
        CreateEntry("QSL", "--.- ... .-..", "I acknowledge / Message confirmed"),
        CreateEntry("SK", "... -.-", "End of contact / Signing off")
    ]);

    public IReadOnlyList<MorseReferenceEntry> GetEntries(ReferenceMode mode) =>
        mode switch
        {
            ReferenceMode.Alphabet => AlphabetEntries,
            ReferenceMode.Abbreviations => AbbreviationEntries,
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, "Unknown reference mode.")
        };

    private static MorseReferenceEntry CreateEntry(string label, string pattern, string? meaning = null) =>
        new(label, pattern.Replace('.', '·'), meaning);
}
