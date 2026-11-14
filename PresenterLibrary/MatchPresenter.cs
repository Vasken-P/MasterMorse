using MasterMorse.Model;
using MasterMorse.Views;

namespace MasterMorse.Presenters;

/// <summary>
/// Runs a local 1v1 Morse duel. It routes each player's input to that player's own state, resolves a round
/// when both players submit or the timer ends, advances through the rounds, and builds the screens for the view.
/// It knows nothing about controls: the view maps keys to <see cref="PlayerInput"/>.
/// </summary>
public sealed class MatchPresenter
{
    private readonly IMatchView _view;
    private readonly MatchEngine _engine;
    private readonly PlayerState _one = new();
    private readonly PlayerState _two = new();
    private DuelMatch? _match;
    private Difficulty _difficulty = Difficulty.Easy;
    private MatchPhase _phase = MatchPhase.Setup;
    private int _roundIndex;
    private long _now;
    private long _deadlineMs;
    private long _revealUntilMs;

    public MatchPresenter(IMatchView view, MatchEngine? engine = null)
    {
        _view = view;
        _engine = engine ?? new MatchEngine();

        _view.StartRequested += StartMatch;
        _view.PlayAgainRequested += PlayAgain;
        _view.LeaveRequested += Leave;
        _view.InputChanged += HandleInput;
        _view.Tick += HandleTick;
    }

    public MatchPhase Phase => _phase;

    public void Initialize()
    {
        _phase = MatchPhase.Setup;
        _match = null;
        _view.ShowSetup();
    }

    /// <summary>Starts a new match with fresh questions. Not allowed while a match is in progress.</summary>
    public void StartMatch(Difficulty difficulty, long nowMs)
    {
        _now = nowMs;
        if (_phase is MatchPhase.Answering or MatchPhase.Revealing)
        {
            _view.ShowMessage("A match is already in progress.");
            return;
        }

        DuelMatch match;
        try
        {
            match = _engine.CreateMatch(difficulty);
        }
        catch (InvalidOperationException ex)
        {
            _view.ShowMessage(ex.Message);
            return;
        }

        _match = match;
        _difficulty = difficulty;
        _one.Reset();
        _two.Reset();
        StartRound(nowMs);
    }

    /// <summary>Starts a fresh match at the same difficulty. Only allowed after a finished match.</summary>
    public void PlayAgain(long nowMs)
    {
        _now = nowMs;
        if (_phase != MatchPhase.Finished)
        {
            return;
        }

        StartMatch(_difficulty, nowMs);
    }

    /// <summary>The players left. Any unfinished round is discarded.</summary>
    public void Leave()
    {
        _phase = MatchPhase.Setup;
        _match = null;
        _one.Reset();
        _two.Reset();
    }

    public void HandleInput(MatchPlayer player, PlayerInput input, bool isDown, long nowMs)
    {
        _now = nowMs;
        if (_phase != MatchPhase.Answering || _match is null)
        {
            return;
        }

        var state = StateOf(player);
        if (state.Submitted)
        {
            return;
        }

        var question = _match.Questions[_roundIndex];
        if (question.Kind == QuestionKind.MorseChoice)
        {
            HandleChoiceInput(player, state, question, input, isDown, nowMs);
        }
        else
        {
            HandleKeyedInput(player, state, input, isDown, nowMs);
        }

        Render();
    }

    public void HandleTick(long nowMs)
    {
        _now = nowMs;
        switch (_phase)
        {
            case MatchPhase.Answering when nowMs >= _deadlineMs:
                ResolveRound(nowMs);
                break;
            case MatchPhase.Revealing when nowMs >= _revealUntilMs:
                AdvanceAfterReveal(nowMs);
                break;
        }

        if (_phase is MatchPhase.Answering or MatchPhase.Revealing)
        {
            Render();
        }
    }

    private void HandleChoiceInput(
        MatchPlayer player, PlayerState state, MatchQuestion question, PlayerInput input, bool isDown, long nowMs)
    {
        if (!isDown)
        {
            return;
        }

        switch (input)
        {
            case PlayerInput.Dot:
                state.MoveChoice(-1, question.Choices.Count);
                break;
            case PlayerInput.Dash:
                state.MoveChoice(1, question.Choices.Count);
                break;
            case PlayerInput.Submit:
                Submit(player, nowMs);
                break;
        }
    }

    private void HandleKeyedInput(MatchPlayer player, PlayerState state, PlayerInput input, bool isDown, long nowMs)
    {
        if (input == PlayerInput.Submit)
        {
            if (isDown)
            {
                Submit(player, nowMs);
            }

            return;
        }

        var element = input switch
        {
            PlayerInput.Dot => (MorseElement?)MorseElement.Dot,
            PlayerInput.Dash => MorseElement.Dash,
            PlayerInput.Space => MorseElement.Space,
            _ => null,
        };

        if (element is not { } key)
        {
            return;
        }

        if (isDown)
        {
            state.Decoder.KeyDown(key, nowMs);
        }
        else
        {
            state.Decoder.KeyUp(key, nowMs);
        }
    }

    private void Submit(MatchPlayer player, long nowMs)
    {
        var state = StateOf(player);
        var question = _match!.Questions[_roundIndex];

        if (question.Kind == QuestionKind.MorseChoice)
        {
            if (state.Choice < 0)
            {
                state.Feedback = "Choose an answer first.";
                return;
            }

            state.Given = question.Choices[state.Choice];
            state.Feedback = "Locked in. Waiting for your opponent...";
        }
        else
        {
            var morse = state.Decoder.Finish();
            if (string.IsNullOrEmpty(morse))
            {
                state.Feedback = "Key your answer before submitting.";
                return;
            }

            state.Given = _engine.DecodeKeyedAnswer(morse);
            state.Feedback = state.Given is null
                ? "Invalid Morse. This counts as wrong."
                : "Locked in. Waiting for your opponent...";
        }

        state.Submitted = true;
        if (_one.Submitted && _two.Submitted)
        {
            ResolveRound(nowMs);
        }
    }

    private void ResolveRound(long nowMs)
    {
        var match = _match!;
        var result = _engine.ScoreRound(
            _roundIndex + 1,
            match.Questions[_roundIndex],
            _one.ToAnswer(),
            _two.ToAnswer());
        match.RecordRound(result);

        _one.Feedback = FeedbackFor(result.PlayerOne);
        _two.Feedback = FeedbackFor(result.PlayerTwo);
        _phase = MatchPhase.Revealing;
        _revealUntilMs = nowMs + (long)DuelRules.RevealDuration.TotalMilliseconds;
    }

    private void AdvanceAfterReveal(long nowMs)
    {
        var match = _match!;
        if (match.IsComplete)
        {
            _phase = MatchPhase.Finished;
            _view.ShowResult(match.ToResult());
            return;
        }

        StartRound(nowMs);
    }

    private void StartRound(long nowMs)
    {
        var match = _match!;
        _roundIndex = match.CurrentRoundIndex;
        _one.Reset();
        _two.Reset();
        _deadlineMs = nowMs + (long)DuelRules.RoundTimeLimit(_difficulty).TotalMilliseconds;
        _phase = MatchPhase.Answering;
        Render();
    }

    private void Render()
    {
        var match = _match;
        if (match is null)
        {
            return;
        }

        var question = match.Questions[_roundIndex];
        var isChoice = question.Kind == QuestionKind.MorseChoice;
        IReadOnlyList<string> choices = isChoice
            ? question.Choices.Select(MorseDisplay.ToDisplay).ToList()
            : Array.Empty<string>();

        var revealed = _phase == MatchPhase.Revealing;
        var remaining = _phase == MatchPhase.Answering
            ? TimeSpan.FromMilliseconds(Math.Max(0, _deadlineMs - _now))
            : TimeSpan.Zero;

        var state = new RoundViewState(
            _roundIndex + 1,
            DuelRules.RoundsPerMatch,
            _difficulty,
            question.Kind,
            question.Prompt,
            choices,
            _phase,
            remaining,
            DuelRules.RoundTimeLimit(_difficulty),
            revealed ? DisplayAnswer(question) : null,
            PlayerView(MatchPlayer.PlayerOne),
            PlayerView(MatchPlayer.PlayerTwo));

        _view.ShowRound(state);
    }

    private PlayerRoundViewState PlayerView(MatchPlayer player)
    {
        var state = StateOf(player);
        var morse = state.Decoder.Morse;
        var isCorrect = _phase == MatchPhase.Revealing
            ? _match!.Rounds[^1].For(player).IsCorrect
            : (bool?)null;

        return new PlayerRoundViewState(
            _match!.Score(player),
            MorseDisplay.ToDisplay(morse),
            _engine.DecodeKeyedAnswer(morse) ?? string.Empty,
            state.Choice,
            state.Submitted,
            state.Feedback,
            isCorrect);
    }

    private static string DisplayAnswer(MatchQuestion question) =>
        question.Kind == QuestionKind.MorseChoice
            ? MorseDisplay.ToDisplay(question.Answer)
            : question.Answer;

    private static string FeedbackFor(PlayerRoundResult result)
    {
        if (result.TimedOut)
        {
            return "Time's up. No answer.";
        }

        if (result.IsCorrect)
        {
            return $"Correct! +{DuelRules.PointsPerCorrectAnswer} point.";
        }

        return result.Given is null ? "No valid answer." : "Wrong.";
    }

    private PlayerState StateOf(MatchPlayer player) =>
        player == MatchPlayer.PlayerOne ? _one : _two;

    /// <summary>One player's answer state for the current round.</summary>
    private sealed class PlayerState
    {
        public MorseInputDecoder Decoder { get; } = new();

        /// <summary>Selected choice index for Easy (multiple choice) rounds, or -1 when nothing is selected.</summary>
        public int Choice { get; set; } = -1;

        public bool Submitted { get; set; }

        public string? Given { get; set; }

        public string Feedback { get; set; } = string.Empty;

        public void MoveChoice(int step, int count)
        {
            if (Choice < 0)
            {
                Choice = step > 0 ? 0 : count - 1;
                return;
            }

            Choice = ((Choice + step) % count + count) % count;
        }

        public PlayerAnswer ToAnswer() => new(Given, TimedOut: !Submitted);

        public void Reset()
        {
            Decoder.Reset();
            Choice = -1;
            Submitted = false;
            Given = null;
            Feedback = string.Empty;
        }
    }
}
