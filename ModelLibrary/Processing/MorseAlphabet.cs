namespace MasterMorse.Model;

/// <summary>A single character and its International Morse code.</summary>
public readonly record struct MorseEntry(char Character, string Code)
{
    public string Symbol => Character.ToString();
}

/// <summary>
/// International Morse code mappings for A–Z, 0–9 and common punctuation.
/// '/' is reserved as the word separator and is therefore not a mapped character.
/// </summary>
public static class MorseAlphabet
{
    public const char WordSeparator = '/';

    public static IReadOnlyList<MorseEntry> Entries { get; } = new MorseEntry[]
    {
        new('A', ".-"),     new('B', "-..."),   new('C', "-.-."),   new('D', "-.."),    new('E', "."),
        new('F', "..-."),   new('G', "--."),    new('H', "...."),   new('I', ".."),    new('J', ".---"),
        new('K', "-.-"),    new('L', ".-.."),   new('M', "--"),     new('N', "-."),     new('O', "---"),
        new('P', ".--."),   new('Q', "--.-"),   new('R', ".-."),    new('S', "..."),    new('T', "-"),
        new('U', "..-"),    new('V', "...-"),   new('W', ".--"),    new('X', "-..-"),   new('Y', "-.--"),
        new('Z', "--.."),
        new('0', "-----"),  new('1', ".----"),  new('2', "..---"),  new('3', "...--"),  new('4', "....-"),
        new('5', "....."),  new('6', "-...."),  new('7', "--..."),  new('8', "---.."),  new('9', "----."),
        new('.', ".-.-.-"), new(',', "--..--"), new('?', "..--.."), new('\'', ".----."), new('!', "-.-.--"),
        new('(', "-.--."),  new(')', "-.--.-"), new('&', ".-..."),  new(':', "---..."), new(';', "-.-.-."),
        new('=', "-...-"),  new('+', ".-.-."),  new('-', "-....-"), new('_', "..--.-"), new('"', ".-..-."),
        new('$', "...-..-"), new('@', ".--.-."),
    };

    private static readonly Dictionary<char, string> ByCharacter =
        Entries.ToDictionary(entry => entry.Character, entry => entry.Code);

    private static readonly Dictionary<string, char> ByCode =
        Entries.ToDictionary(entry => entry.Code, entry => entry.Character);

    public static bool TryGetCode(char character, out string code)
    {
        if (ByCharacter.TryGetValue(char.ToUpperInvariant(character), out var found))
        {
            code = found;
            return true;
        }

        code = string.Empty;
        return false;
    }

    public static bool TryGetCharacter(string code, out char character) =>
        ByCode.TryGetValue(code, out character);
}
