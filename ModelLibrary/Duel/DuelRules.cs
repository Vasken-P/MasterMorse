namespace MasterMorse.Model;

/// <summary>The fixed rules of a duel match.</summary>
public static class DuelRules
{
    public const int RoundsPerMatch = 5;
    public const int PointsPerCorrectAnswer = 1;
    public const int ChoiceCount = 4;

    /// <summary>The time each player has to answer a round.</summary>
    public static TimeSpan RoundTimeLimit(Difficulty difficulty) => difficulty switch
    {
        Difficulty.Easy => TimeSpan.FromSeconds(20),
        Difficulty.Hard => TimeSpan.FromSeconds(15),
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty)),
    };

    /// <summary>How long the correct answers stay on screen before the next round starts.</summary>
    public static readonly TimeSpan RevealDuration = TimeSpan.FromSeconds(3);
}
