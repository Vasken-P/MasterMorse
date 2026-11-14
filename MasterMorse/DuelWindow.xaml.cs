using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MasterMorse.Model;
using MasterMorse.Views;

namespace MasterMorse;

/// <summary>
/// The 1v1 duel view. It maps keys to players, runs the clock, and draws whatever the presenter sends.
/// It holds no game logic: no question selection, validation, scoring or round flow.
/// </summary>
public partial class DuelWindow : Window, IMatchView
{
    /// <summary>Looks up a brush from Theme.xaml so the palette lives in one place.</summary>
    private static Brush ThemeBrush(string key) => (Brush)Application.Current.FindResource(key);

    private readonly Stopwatch _clock = Stopwatch.StartNew();
    private readonly DispatcherTimer _timer;
    private readonly PlayerCard _oneCard;
    private readonly PlayerCard _twoCard;
    private readonly Difficulty _difficulty;

    /// <summary>Opens the duel in one fixed mode. The mode is chosen on the translator window.</summary>
    public DuelWindow(Difficulty difficulty)
    {
        InitializeComponent();

        _difficulty = difficulty;
        var isEasy = difficulty == Difficulty.Easy;
        Title = isEasy ? "1v1 Morse Duel - Easy" : "1v1 Morse Duel - Hard";
        SetupTitle.Text = isEasy ? "1v1 Easy: Multiple Choice" : "1v1 Hard: Typed Answer";
        EasyControls.Visibility = isEasy ? Visibility.Visible : Visibility.Collapsed;
        HardControls.Visibility = isEasy ? Visibility.Collapsed : Visibility.Visible;

        _oneCard = new PlayerCard(
            OneScore, OneTimer, OneChoices, OneKeyedArea, OneKeyed, OneDecoded, OneFeedback,
            ThemeBrush("PrimaryBrush"), ThemeBrush("PrimaryContainerBrush"));
        _twoCard = new PlayerCard(
            TwoScore, TwoTimer, TwoChoices, TwoKeyedArea, TwoKeyed, TwoDecoded, TwoFeedback,
            ThemeBrush("Player2Brush"), ThemeBrush("Player2ContainerBrush"));

        _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(50) };
        _timer.Tick += (_, _) => Tick?.Invoke(Now);
        _timer.Start();

        Closed += (_, _) =>
        {
            _timer.Stop();
            LeaveRequested?.Invoke();
        };
    }

    public event Action<Difficulty, long>? StartRequested;

    public event Action<long>? PlayAgainRequested;

    public event Action? LeaveRequested;

    public event Action<MatchPlayer, PlayerInput, bool, long>? InputChanged;

    public event Action<long>? Tick;

    private long Now => _clock.ElapsedMilliseconds;

    public void ShowSetup()
    {
        SetupPanel.Visibility = Visibility.Visible;
        MatchPanel.Visibility = Visibility.Collapsed;
        ResultPanel.Visibility = Visibility.Collapsed;
    }

    public void ShowRound(RoundViewState state)
    {
        SetupPanel.Visibility = Visibility.Collapsed;
        ResultPanel.Visibility = Visibility.Collapsed;
        MatchPanel.Visibility = Visibility.Visible;

        RoundText.Text = $"Round {state.RoundNumber} of {state.TotalRounds}  ·  {state.Difficulty}";
        PromptText.Text = state.Prompt;
        TimerText.Text = state.Phase == MatchPhase.Answering
            ? $"{Math.Ceiling(state.TimeRemaining.TotalSeconds):0}s"
            : string.Empty;
        RevealText.Text = state.RevealedAnswer is null ? string.Empty : $"Answer: {state.RevealedAnswer}";

        _oneCard.Update(state.PlayerOne, state);
        _twoCard.Update(state.PlayerTwo, state);
    }

    public void ShowResult(MatchResult result)
    {
        SetupPanel.Visibility = Visibility.Collapsed;
        MatchPanel.Visibility = Visibility.Collapsed;
        ResultPanel.Visibility = Visibility.Visible;

        WinnerText.Text = result.Winner is { } winner
            ? $"{PlayerName(winner)} wins!"
            : "It's a draw!";
        FinalScoreText.Text = $"Player 1: {result.PlayerOneScore}   ·   Player 2: {result.PlayerTwoScore}";

        RoundsGrid.ItemsSource = result.Rounds
            .Select(round => new RoundRow(
                round.RoundNumber,
                round.Question.Prompt,
                DisplayAnswer(round.Question),
                Describe(round.PlayerOne, round.Question.Kind),
                Describe(round.PlayerTwo, round.Question.Kind)))
            .ToList();
    }

    public void ShowMessage(string message) => StatusText.Text = message;

    protected override void OnPreviewKeyDown(KeyEventArgs e)
    {
        base.OnPreviewKeyDown(e);

        // Keep arrows and Enter working for the setup controls before a match starts.
        if (SetupPanel.Visibility == Visibility.Visible)
        {
            return;
        }

        if (!TryMapKey(e.Key, out var player, out var input))
        {
            return;
        }

        e.Handled = true;
        if (!e.IsRepeat)
        {
            InputChanged?.Invoke(player, input, true, Now);
        }
    }

    protected override void OnPreviewKeyUp(KeyEventArgs e)
    {
        base.OnPreviewKeyUp(e);

        if (SetupPanel.Visibility == Visibility.Visible)
        {
            return;
        }

        if (!TryMapKey(e.Key, out var player, out var input))
        {
            return;
        }

        e.Handled = true;
        InputChanged?.Invoke(player, input, false, Now);
    }

    private void Start_Click(object sender, RoutedEventArgs e) =>
        StartRequested?.Invoke(_difficulty, Now);

    private void PlayAgain_Click(object sender, RoutedEventArgs e) =>
        PlayAgainRequested?.Invoke(Now);

    private void Leave_Click(object sender, RoutedEventArgs e) => Close();

    private void Close_Click(object sender, RoutedEventArgs e) => Close();

    /// <summary>Player 1 uses W/A/S/D and player 2 uses the arrows and Enter. The two sets never overlap.</summary>
    private static bool TryMapKey(Key key, out MatchPlayer player, out PlayerInput input)
    {
        switch (key)
        {
            case Key.W:
                (player, input) = (MatchPlayer.PlayerOne, PlayerInput.Dot);
                return true;
            case Key.A:
                (player, input) = (MatchPlayer.PlayerOne, PlayerInput.Dash);
                return true;
            case Key.S:
                (player, input) = (MatchPlayer.PlayerOne, PlayerInput.Space);
                return true;
            case Key.D:
                (player, input) = (MatchPlayer.PlayerOne, PlayerInput.Submit);
                return true;
            case Key.Up:
                (player, input) = (MatchPlayer.PlayerTwo, PlayerInput.Dot);
                return true;
            case Key.Down:
                (player, input) = (MatchPlayer.PlayerTwo, PlayerInput.Dash);
                return true;
            case Key.Right:
                (player, input) = (MatchPlayer.PlayerTwo, PlayerInput.Space);
                return true;
            case Key.Enter:
                (player, input) = (MatchPlayer.PlayerTwo, PlayerInput.Submit);
                return true;
            default:
                player = default;
                input = default;
                return false;
        }
    }

    private static string PlayerName(MatchPlayer player) =>
        player == MatchPlayer.PlayerOne ? "Player 1" : "Player 2";

    private static string DisplayAnswer(MatchQuestion question) =>
        question.Kind == QuestionKind.MorseChoice
            ? MorseDisplay.ToDisplay(question.Answer)
            : question.Answer;

    private static string Describe(PlayerRoundResult result, QuestionKind kind)
    {
        if (result.TimedOut)
        {
            return "Time's up (+0)";
        }

        if (result.Given is null)
        {
            return "Invalid (+0)";
        }

        var shown = kind == QuestionKind.MorseChoice
            ? MorseDisplay.ToDisplay(result.Given)
            : result.Given;
        return result.IsCorrect
            ? $"{shown}  correct (+{result.Points})"
            : $"{shown}  wrong (+0)";
    }

    private sealed record RoundRow(int Round, string Prompt, string Answer, string PlayerOne, string PlayerTwo);

    /// <summary>The widgets for one player's panel. It only displays the state it is given.</summary>
    private sealed class PlayerCard
    {
        private readonly TextBlock _score;
        private readonly TextBlock _timer;
        private readonly StackPanel _choices;
        private readonly StackPanel _keyedArea;
        private readonly TextBlock _keyed;
        private readonly TextBlock _decoded;
        private readonly TextBlock _feedback;
        private readonly Brush _accent;
        private readonly Brush _accentContainer;

        public PlayerCard(
            TextBlock score,
            TextBlock timer,
            StackPanel choices,
            StackPanel keyedArea,
            TextBlock keyed,
            TextBlock decoded,
            TextBlock feedback,
            Brush accent,
            Brush accentContainer)
        {
            _accent = accent;
            _accentContainer = accentContainer;
            _score = score;
            _timer = timer;
            _choices = choices;
            _keyedArea = keyedArea;
            _keyed = keyed;
            _decoded = decoded;
            _feedback = feedback;
        }

        public void Update(PlayerRoundViewState state, RoundViewState round)
        {
            _score.Text = $"Score: {state.Score}";
            _timer.Text = round.Phase switch
            {
                MatchPhase.Answering => $"Time left: {Math.Ceiling(round.TimeRemaining.TotalSeconds):0}s",
                MatchPhase.Revealing => "Round over",
                _ => string.Empty,
            };

            var isChoice = round.Kind == QuestionKind.MorseChoice;
            _choices.Visibility = isChoice ? Visibility.Visible : Visibility.Collapsed;
            _keyedArea.Visibility = isChoice ? Visibility.Collapsed : Visibility.Visible;

            if (isChoice)
            {
                RenderChoices(state, round);
            }
            else
            {
                _keyed.Text = state.KeyedMorse;
                _decoded.Text = state.KeyedText.Length > 0 ? $"Reads as: {state.KeyedText}" : string.Empty;
            }

            _feedback.Text = state.Feedback;
        }

        private void RenderChoices(PlayerRoundViewState state, RoundViewState round)
        {
            _choices.Children.Clear();
            for (var i = 0; i < round.Choices.Count; i++)
            {
                var code = round.Choices[i];
                var isSelected = i == state.SelectedChoice;
                var isCorrect = round.RevealedAnswer is not null && code == round.RevealedAnswer;

                // Default: white card with a hairline outline. The selection uses the player's colour.
                var fill = ThemeBrush("SurfaceBrush");
                var border = ThemeBrush("OutlineBrush");
                var thickness = 1.0;
                if (round.Phase == MatchPhase.Revealing && isCorrect)
                {
                    fill = ThemeBrush("SuccessContainerBrush");
                    border = ThemeBrush("SuccessBrush");
                    thickness = 2;
                }
                else if (round.Phase == MatchPhase.Revealing && isSelected)
                {
                    fill = ThemeBrush("ErrorContainerBrush");
                    border = ThemeBrush("ErrorBrush");
                    thickness = 2;
                }
                else if (isSelected)
                {
                    fill = _accentContainer;
                    border = _accent;
                    thickness = 2;
                }

                var label = new TextBlock
                {
                    Text = $"{i + 1}.  {code}",
                    FontFamily = (FontFamily)Application.Current.FindResource("MonoFont"),
                    Foreground = ThemeBrush("TextPrimaryBrush"),
                    FontSize = 22,
                    VerticalAlignment = VerticalAlignment.Center,
                };

                _choices.Children.Add(new Border
                {
                    CornerRadius = new CornerRadius(10),
                    BorderThickness = new Thickness(thickness),
                    BorderBrush = border,
                    Background = fill,
                    Padding = new Thickness(14, 9, 14, 9),
                    Margin = new Thickness(0, 0, 0, 6),
                    Child = label,
                });
            }
        }
    }
}
