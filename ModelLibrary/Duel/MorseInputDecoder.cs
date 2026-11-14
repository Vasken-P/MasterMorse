using System.Text;

namespace MasterMorse.Model;

/// <summary>The keys a player uses to key Morse.</summary>
public enum MorseElement
{
    Dot,
    Dash,
    Space,
}

/// <summary>
/// Turns timed key presses and releases into Morse for one player. The dot and dash keys set the symbol;
/// the time between keys sets the letter and word breaks. One unit is <see cref="DefaultUnitMilliseconds"/>.
/// <list type="bullet">
/// <item>A gap under 2 units after an element stays in the same letter.</item>
/// <item>A gap of 2 units or more starts a new letter.</item>
/// <item>A gap of 5 units or more starts a new word.</item>
/// <item>Tapping space starts a new letter. Holding space for 5 units or more starts a new word.</item>
/// </list>
/// Only one key counts at a time, so overlapping presses are ignored. Hold length of dot and dash keys is not
/// penalized, because the key itself sets the symbol.
/// </summary>
public sealed class MorseInputDecoder
{
    public const int DefaultUnitMilliseconds = 150;

    private const double LetterBreakUnits = 2.0;
    private const double WordBreakUnits = 5.0;

    private readonly int _unitMilliseconds;
    private readonly List<List<string>> _completedWords = new();
    private readonly List<string> _currentWord = new();
    private readonly StringBuilder _currentLetter = new();
    private MorseElement? _heldKey;
    private long _heldSince;
    private long? _lastReleased;

    public MorseInputDecoder(int unitMilliseconds = DefaultUnitMilliseconds)
    {
        if (unitMilliseconds <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(unitMilliseconds), "The timing unit must be positive.");
        }

        _unitMilliseconds = unitMilliseconds;
    }

    /// <summary>Morse keyed so far, using the separators from <see cref="MorseAlphabet"/>. Includes the letter in progress.</summary>
    public string Morse
    {
        get
        {
            var words = _completedWords.Select(word => string.Join(" ", word)).ToList();

            var currentWord = new List<string>(_currentWord);
            if (_currentLetter.Length > 0)
            {
                currentWord.Add(_currentLetter.ToString());
            }

            if (currentWord.Count > 0)
            {
                words.Add(string.Join(" ", currentWord));
            }

            return string.Join($" {MorseAlphabet.WordSeparator} ", words);
        }
    }

    public void KeyDown(MorseElement key, long timestampMs)
    {
        if (_heldKey is not null)
        {
            return;
        }

        _heldKey = key;
        _heldSince = timestampMs;

        if (key == MorseElement.Space)
        {
            CloseLetter();
            return;
        }

        if (_currentLetter.Length > 0 && _lastReleased is long released)
        {
            var gapUnits = (timestampMs - released) / (double)_unitMilliseconds;
            if (gapUnits >= WordBreakUnits)
            {
                CloseWord();
            }
            else if (gapUnits >= LetterBreakUnits)
            {
                CloseLetter();
            }
        }

        _currentLetter.Append(key == MorseElement.Dot ? '.' : '-');
    }

    public void KeyUp(MorseElement key, long timestampMs)
    {
        if (_heldKey != key)
        {
            return;
        }

        var heldUnits = (timestampMs - _heldSince) / (double)_unitMilliseconds;
        _heldKey = null;
        _lastReleased = timestampMs;

        if (key == MorseElement.Space && heldUnits >= WordBreakUnits)
        {
            CloseWord();
        }
    }

    /// <summary>Ends the input, closing the letter and word in progress, and returns the Morse.</summary>
    public string Finish()
    {
        CloseWord();
        return Morse;
    }

    public void Reset()
    {
        _completedWords.Clear();
        _currentWord.Clear();
        _currentLetter.Clear();
        _heldKey = null;
        _lastReleased = null;
    }

    private void CloseLetter()
    {
        if (_currentLetter.Length == 0)
        {
            return;
        }

        _currentWord.Add(_currentLetter.ToString());
        _currentLetter.Clear();
    }

    private void CloseWord()
    {
        CloseLetter();
        if (_currentWord.Count == 0)
        {
            return;
        }

        _completedWords.Add(new List<string>(_currentWord));
        _currentWord.Clear();
    }
}
