namespace MasterMorse.Model;

/// <summary>Builds the shared, distinct question list for a match from the question data.</summary>
public sealed class QuestionFactory
{
    private readonly Random _random;

    public QuestionFactory(Random? random = null)
    {
        _random = random ?? new Random();
    }

    /// <summary>
    /// Creates <paramref name="count"/> distinct questions. Easy uses letter questions;
    /// Hard uses abbreviation questions.
    /// </summary>
    public IReadOnlyList<MatchQuestion> CreateMatch(Difficulty difficulty, int count)
    {
        if (count <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count), "A match needs at least one question.");
        }

        return difficulty == Difficulty.Easy
            ? CreateLetterQuestions(count)
            : CreateAbbreviationQuestions(count);
    }

    private IReadOnlyList<MatchQuestion> CreateLetterQuestions(int count)
    {
        var letters = MorseAlphabet.Entries
            .Where(entry => entry.Character is >= 'A' and <= 'Z')
            .ToList();
        EnsureAvailable(letters.Count, count, "letters");

        return Shuffle(letters)
            .Take(count)
            .Select(letter => CreateChoiceQuestion(letter, letters))
            .ToList();
    }

    private MatchQuestion CreateChoiceQuestion(MorseEntry target, IReadOnlyList<MorseEntry> pool)
    {
        var distractors = Shuffle(pool
                .Where(entry => entry.Code != target.Code)
                .Select(entry => entry.Code)
                .Distinct())
            .Take(DuelRules.ChoiceCount - 1);

        var choices = Shuffle(distractors.Append(target.Code)).ToList();
        if (choices.Count != DuelRules.ChoiceCount)
        {
            throw new InvalidOperationException(
                $"Cannot build a choice question for '{target.Symbol}': not enough distinct Morse codes.");
        }

        return new MatchQuestion(QuestionKind.MorseChoice, target.Symbol, target.Code, choices);
    }

    private IReadOnlyList<MatchQuestion> CreateAbbreviationQuestions(int count)
    {
        var pool = QuestionBank.Abbreviations;
        EnsureAvailable(pool.Count, count, "abbreviation questions");

        return Shuffle(pool)
            .Take(count)
            .Select(item => new MatchQuestion(
                QuestionKind.AbbreviationKeying,
                item.Phrase,
                item.Abbreviation,
                Array.Empty<string>()))
            .ToList();
    }

    private IEnumerable<T> Shuffle<T>(IEnumerable<T> items) =>
        items.OrderBy(_ => _random.Next());

    private static void EnsureAvailable(int available, int needed, string what)
    {
        if (available < needed)
        {
            throw new InvalidOperationException(
                $"Cannot build a match: {needed} {what} needed, but only {available} available.");
        }
    }
}
