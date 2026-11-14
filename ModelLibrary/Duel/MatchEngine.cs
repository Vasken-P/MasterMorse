namespace MasterMorse.Model;

/// <summary>
/// Creates matches, validates answers and scores rounds. It holds no UI code, and it reuses
/// <see cref="MorseTranslator"/> for decoding keyed Morse.
/// </summary>
public sealed class MatchEngine
{
    private readonly QuestionFactory _questions;
    private readonly MorseTranslator _translator;

    public MatchEngine(QuestionFactory? questions = null, MorseTranslator? translator = null)
    {
        _questions = questions ?? new QuestionFactory();
        _translator = translator ?? new MorseTranslator();
    }

    /// <summary>Creates a match with fresh questions for every round.</summary>
    public DuelMatch CreateMatch(Difficulty difficulty) =>
        new(difficulty, _questions.CreateMatch(difficulty, DuelRules.RoundsPerMatch));

    /// <summary>
    /// Decodes keyed Morse into text. Returns null when the Morse is empty, invalid, or decodes to nothing.
    /// </summary>
    public string? DecodeKeyedAnswer(string morse)
    {
        if (string.IsNullOrWhiteSpace(morse))
        {
            return null;
        }

        var result = _translator.MorseToText(morse);
        return result.HasErrors || string.IsNullOrWhiteSpace(result.Output) ? null : result.Output;
    }

    /// <summary>Case-insensitive, ignores leading and trailing whitespace. A null answer is never correct.</summary>
    public bool IsCorrect(MatchQuestion question, string? given)
    {
        if (given is null)
        {
            return false;
        }

        return Normalize(given) == Normalize(question.Answer);
    }

    public RoundResult ScoreRound(int roundNumber, MatchQuestion question, PlayerAnswer one, PlayerAnswer two) =>
        new(roundNumber, question, Score(question, one), Score(question, two));

    private PlayerRoundResult Score(MatchQuestion question, PlayerAnswer answer)
    {
        var correct = !answer.TimedOut && IsCorrect(question, answer.Given);
        return new PlayerRoundResult(answer.Given, correct, answer.TimedOut);
    }

    private static string Normalize(string value) => value.Trim().ToUpperInvariant();
}
