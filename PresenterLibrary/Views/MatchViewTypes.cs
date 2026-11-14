using MasterMorse.Model;

namespace MasterMorse.Views;

/// <summary>A key the duel window forwards to the presenter. The window maps physical keys to these per player.</summary>
public enum PlayerInput
{
    Dot,
    Dash,
    Space,
    Submit,
}

/// <summary>Where the duel is: setting up, answering a round, showing a reveal, or finished.</summary>
public enum MatchPhase
{
    Setup,
    Answering,
    Revealing,
    Finished,
}

/// <summary>What one player's panel shows. Built by the presenter; the view only displays it.</summary>
/// <param name="KeyedMorse">Keyed Morse so far, shown with the display glyphs.</param>
/// <param name="KeyedText">What the keyed Morse decodes to, or empty while it is incomplete.</param>
/// <param name="SelectedChoice">Selected choice index for Easy (multiple choice) rounds, or -1.</param>
/// <param name="IsCorrect">Whether the answer was correct; null until the round is revealed.</param>
public sealed record PlayerRoundViewState(
    int Score,
    string KeyedMorse,
    string KeyedText,
    int SelectedChoice,
    bool Submitted,
    string Feedback,
    bool? IsCorrect);

/// <summary>The round screen. Choices are already in display form.</summary>
public sealed record RoundViewState(
    int RoundNumber,
    int TotalRounds,
    Difficulty Difficulty,
    QuestionKind Kind,
    string Prompt,
    IReadOnlyList<string> Choices,
    MatchPhase Phase,
    TimeSpan TimeRemaining,
    TimeSpan TimeLimit,
    string? RevealedAnswer,
    PlayerRoundViewState PlayerOne,
    PlayerRoundViewState PlayerTwo);
