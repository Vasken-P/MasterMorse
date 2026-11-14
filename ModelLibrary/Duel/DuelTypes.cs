namespace MasterMorse.Model;

/// <summary>The two players in a local duel.</summary>
public enum MatchPlayer
{
    PlayerOne,
    PlayerTwo,
}

/// <summary>
/// The game mode. Easy is multiple choice; Hard is typed (keyed) entry of the answer.
/// The mode chooses the question type and the round time limit.
/// </summary>
public enum Difficulty
{
    /// <summary>Multiple choice: given a letter, pick its Morse code from four options.</summary>
    Easy,

    /// <summary>Typed answer: given a phrase, key its Morse abbreviation.</summary>
    Hard,
}

public enum QuestionKind
{
    /// <summary>Given a letter, pick its Morse code from several choices.</summary>
    MorseChoice,

    /// <summary>Given a word or phrase, key its Morse abbreviation.</summary>
    AbbreviationKeying,
}

/// <summary>
/// One question, shared by both players in a round. Morse codes are stored with '.' and '-'.
/// For <see cref="QuestionKind.MorseChoice"/>, Choices holds the Morse options and Answer is the correct one.
/// </summary>
public sealed record MatchQuestion(
    QuestionKind Kind,
    string Prompt,
    string Answer,
    IReadOnlyList<string> Choices);

/// <summary>
/// What a player handed in for a round. Given is null when nothing valid was submitted;
/// TimedOut is true when the player did not submit before the round timer ended.
/// </summary>
public sealed record PlayerAnswer(string? Given, bool TimedOut);

/// <summary>A player's scored answer for one round.</summary>
public sealed record PlayerRoundResult(string? Given, bool IsCorrect, bool TimedOut)
{
    public int Points => IsCorrect ? DuelRules.PointsPerCorrectAnswer : 0;
}

public sealed record RoundResult(
    int RoundNumber,
    MatchQuestion Question,
    PlayerRoundResult PlayerOne,
    PlayerRoundResult PlayerTwo)
{
    public PlayerRoundResult For(MatchPlayer player) =>
        player == MatchPlayer.PlayerOne ? PlayerOne : PlayerTwo;
}

public sealed record MatchResult(
    Difficulty Difficulty,
    IReadOnlyList<RoundResult> Rounds,
    int PlayerOneScore,
    int PlayerTwoScore)
{
    /// <summary>The player with the higher score, or null when the scores are equal.</summary>
    public MatchPlayer? Winner
    {
        get
        {
            if (PlayerOneScore == PlayerTwoScore)
            {
                return null;
            }

            return PlayerOneScore > PlayerTwoScore ? MatchPlayer.PlayerOne : MatchPlayer.PlayerTwo;
        }
    }
}
