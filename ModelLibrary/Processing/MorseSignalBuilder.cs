namespace MasterMorse.Model;

public enum SignalKind
{
    Tone,
    Gap,
}

/// <summary>One audible or silent part of a Morse message, measured in time units.</summary>
public readonly record struct SignalPart(SignalKind Kind, int Units);

/// <summary>
/// Turns valid Morse code into timed signal parts using the standard timing:
/// dot 1, dash 3, gap within a letter 1, gap between letters 3, gap between words 7.
/// </summary>
public static class MorseSignalBuilder
{
    public static IReadOnlyList<SignalPart> Build(string morse)
    {
        var words = morse
            .Split(MorseAlphabet.WordSeparator)
            .Select(word => word.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries))
            .Where(letters => letters.Length > 0)
            .ToList();

        var parts = new List<SignalPart>();
        for (var w = 0; w < words.Count; w++)
        {
            var letters = words[w];
            for (var l = 0; l < letters.Length; l++)
            {
                var code = letters[l];
                for (var i = 0; i < code.Length; i++)
                {
                    var element = code[i];
                    if (element != '.' && element != '-')
                    {
                        throw new ArgumentException($"Invalid Morse element '{element}'.", nameof(morse));
                    }

                    parts.Add(new SignalPart(SignalKind.Tone, element == '.' ? 1 : 3));
                    if (i < code.Length - 1)
                    {
                        parts.Add(new SignalPart(SignalKind.Gap, 1));
                    }
                }

                if (l < letters.Length - 1)
                {
                    parts.Add(new SignalPart(SignalKind.Gap, 3));
                }
            }

            if (w < words.Count - 1)
            {
                parts.Add(new SignalPart(SignalKind.Gap, 7));
            }
        }

        return parts;
    }
}
