using MasterMorse.Model;

namespace MasterMorse.Views;

/// <summary>
/// The duel view contract. The view raises events with timestamps in milliseconds from its own clock,
/// and the presenter sets the screen through the methods below.
/// </summary>
public interface IMatchView
{
    /// <summary>The player chose a difficulty and started a match.</summary>
    event Action<Difficulty, long>? StartRequested;

    /// <summary>The players asked for a new match at the same difficulty.</summary>
    event Action<long>? PlayAgainRequested;

    /// <summary>The players left the duel.</summary>
    event Action? LeaveRequested;

    /// <summary>A mapped key went down (true) or up (false). Parameters: player, input, isDown, timestamp.</summary>
    event Action<MatchPlayer, PlayerInput, bool, long>? InputChanged;

    /// <summary>A timer tick, used for round deadlines and reveal timing.</summary>
    event Action<long>? Tick;

    void ShowSetup();

    void ShowRound(RoundViewState state);

    void ShowResult(MatchResult result);

    void ShowMessage(string message);
}
