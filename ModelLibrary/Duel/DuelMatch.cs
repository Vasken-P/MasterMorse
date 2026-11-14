namespace MasterMorse.Model;

/// <summary>The state of one match: its fixed questions and the rounds recorded so far.</summary>
public sealed class DuelMatch
{
    private readonly List<RoundResult> _rounds = new();

    public DuelMatch(Difficulty difficulty, IReadOnlyList<MatchQuestion> questions)
    {
        if (questions.Count == 0)
        {
            throw new ArgumentException("A match needs at least one question.", nameof(questions));
        }

        Difficulty = difficulty;
        Questions = questions;
    }

    public Difficulty Difficulty { get; }

    public IReadOnlyList<MatchQuestion> Questions { get; }

    public IReadOnlyList<RoundResult> Rounds => _rounds;

    /// <summary>Zero-based index of the round being played, which equals the number of recorded rounds.</summary>
    public int CurrentRoundIndex => _rounds.Count;

    public bool IsComplete => _rounds.Count >= Questions.Count;

    /// <summary>The question for the round being played, or null when every round is recorded.</summary>
    public MatchQuestion? CurrentQuestion => IsComplete ? null : Questions[_rounds.Count];

    /// <summary>Records a finished round. Rounds must be recorded in order, once each.</summary>
    public void RecordRound(RoundResult result)
    {
        if (IsComplete)
        {
            throw new InvalidOperationException("The match is already complete.");
        }

        if (result.RoundNumber != _rounds.Count + 1)
        {
            throw new InvalidOperationException(
                $"Expected round {_rounds.Count + 1} but received round {result.RoundNumber}.");
        }

        _rounds.Add(result);
    }

    public int Score(MatchPlayer player) => _rounds.Sum(round => round.For(player).Points);

    /// <summary>Builds the final result. Throws while rounds are still unfinished.</summary>
    public MatchResult ToResult()
    {
        if (!IsComplete)
        {
            throw new InvalidOperationException("The match still has unfinished rounds.");
        }

        return new MatchResult(
            Difficulty,
            Rounds.ToList(),
            Score(MatchPlayer.PlayerOne),
            Score(MatchPlayer.PlayerTwo));
    }
}
