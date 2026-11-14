namespace MasterMorse.Model;

/// <summary>
/// Converts between the plain Morse dot ('.') used by the model and the large dot shown to the user.
/// The model always works with '.'; only the text shown in Morse boxes uses the display glyph.
/// </summary>
public static class MorseDisplay
{
    /// <summary>U+25CF BLACK CIRCLE: the dot shown in Morse text.</summary>
    public const char DotGlyph = '●';

    /// <summary>U+25AC BLACK RECTANGLE: the thick dash shown in Morse text (available in Consolas).</summary>
    public const char DashGlyph = '▬';

    /// <summary>Replaces every Morse dot and dash with its display glyph.</summary>
    public static string ToDisplay(string morse) =>
        morse.Replace('.', DotGlyph).Replace('-', DashGlyph);

    /// <summary>
    /// Replaces display dots and dashes (and common look-alikes) with the plain Morse symbols,
    /// so the translator and player can read the text.
    /// </summary>
    public static string FromDisplay(string text) =>
        text.Replace(DotGlyph, '.')
            .Replace('•', '.')
            .Replace('∙', '.')
            .Replace(DashGlyph, '-')
            .Replace('━', '-')
            .Replace('—', '-')
            .Replace('–', '-');
}
