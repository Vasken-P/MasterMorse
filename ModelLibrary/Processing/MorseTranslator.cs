namespace MasterMorse.Model;

/// <summary>The output of a translation together with any validation errors.</summary>
public readonly record struct TranslationResult(string Output, IReadOnlyList<string> Errors)
{
    public bool HasErrors => Errors.Count > 0;

    public string ErrorMessage => string.Join(" ", Errors);
}

/// <summary>Converts text to Morse code and Morse code to text. Contains no UI code.</summary>
public sealed class MorseTranslator
{
    private const string LetterSeparator = " ";
    private const string WordSeparator = " / ";

    /// <summary>
    /// Translates text into Morse code. Words are separated by " / ".
    /// Unsupported characters are skipped and reported in the result's errors.
    /// </summary>
    public TranslationResult TextToMorse(string text)
    {
        var words = new List<string>();
        var unsupported = new SortedSet<char>();

        foreach (var word in text.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries))
        {
            var codes = new List<string>();
            foreach (var character in word)
            {
                if (MorseAlphabet.TryGetCode(character, out var code))
                {
                    codes.Add(code);
                }
                else
                {
                    unsupported.Add(character);
                }
            }

            if (codes.Count > 0)
            {
                words.Add(string.Join(LetterSeparator, codes));
            }
        }

        var errors = new List<string>();
        if (unsupported.Count > 0)
        {
            errors.Add($"Unsupported character(s): {string.Join(" ", unsupported.Select(c => $"'{c}'"))}.");
        }

        return new TranslationResult(string.Join(WordSeparator, words), errors);
    }

    /// <summary>
    /// Translates Morse code into text. Letters are separated by spaces and words by '/'.
    /// Invalid sequences are skipped and reported in the result's errors.
    /// </summary>
    public TranslationResult MorseToText(string morse)
    {
        var words = new List<string>();
        var invalid = new SortedSet<string>(StringComparer.Ordinal);

        foreach (var wordText in morse.Split(MorseAlphabet.WordSeparator))
        {
            var letters = new List<char>();
            foreach (var token in wordText.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries))
            {
                if (MorseAlphabet.TryGetCharacter(token, out var character))
                {
                    letters.Add(character);
                }
                else
                {
                    invalid.Add(token);
                }
            }

            if (letters.Count > 0)
            {
                words.Add(new string(letters.ToArray()));
            }
        }

        var errors = new List<string>();
        if (invalid.Count > 0)
        {
            errors.Add($"Invalid Morse sequence(s): {string.Join(" ", invalid)}.");
        }

        return new TranslationResult(string.Join(" ", words), errors);
    }
}
