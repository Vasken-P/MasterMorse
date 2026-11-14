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

    private static readonly IReadOnlyList<MorseReferenceEntry> GeneralEntries = Array.AsReadOnly(
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

    private static readonly IReadOnlyList<MorseReferenceEntry> MedicalEntries = Array.AsReadOnly(
    [
        CreateEntry("HLP", ".... .-.. .--.", "Help / Need assistance"),
        CreateEntry("PAIN", ".--. .- .. -.", "I am in pain"),
        CreateEntry("AIR", ".- .. .-.", "Short of breath / Need oxygen"),
        CreateEntry("SUCT", "... ..- -.-. -", "Suction (clear airway/mucus)"),
        CreateEntry("MED", "-- . -..", "Medication"),
        CreateEntry("DOC", "-.. --- -.-.", "Call doctor / nurse"),
        CreateEntry("VOM", "...- --- --", "Nauseous / Going to vomit"),
        CreateEntry("POS", ".--. --- ...", "Reposition / Turn me"),
        CreateEntry("WTR", ".-- - .-.", "Water / Thirsty"),
        CreateEntry("BTH", "-... - ....", "Bathroom / Bedpan"),
        CreateEntry("COLD", "-.-. --- .-.. -..", "I am too cold"),
        CreateEntry("HOT", ".... --- -", "I am too hot"),
        CreateEntry("Y", "-.--", "Yes / Confirm"),
        CreateEntry("N", "-.", "No / Stop")
    ]);

    private static readonly IReadOnlyList<MorseReferenceSection> AlphabetSections = Array.AsReadOnly(
    [
        new MorseReferenceSection(string.Empty, AlphabetEntries)
    ]);

    private static readonly IReadOnlyList<MorseReferenceSection> AbbreviationSections = Array.AsReadOnly(
    [
        new MorseReferenceSection("General", GeneralEntries),
        new MorseReferenceSection("Medical", MedicalEntries)
    ]);

    public IReadOnlyList<MorseReferenceSection> GetSections(ReferenceMode mode) =>
        mode switch
        {
            ReferenceMode.Alphabet => AlphabetSections,
            ReferenceMode.Abbreviations => AbbreviationSections,
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, "Unknown reference mode.")
        };

    private static MorseReferenceEntry CreateEntry(string label, string pattern, string? meaning = null) =>
        new(label, pattern.Replace('.', '·'), meaning);
}
