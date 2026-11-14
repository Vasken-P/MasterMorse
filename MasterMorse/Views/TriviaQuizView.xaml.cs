using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MasterMorse.Models.Trivia;
using MasterMorse.Presenters.Trivia;

namespace MasterMorse.Views;

public partial class TriviaQuizView : Window, ITriviaQuizView
{
    private readonly List<RadioButton> _answerButtons = [];
    private int? _selectedAnswerIndex;

    public TriviaQuizView()
    {
        InitializeComponent();
    }

    public event Action<int>? AnswerSelected;
    public event Action<int>? AnswerSubmitted;
    public event Action? NextQuestionRequested;
    public event Action? ReturnToTitleRequested;

    public void DisplayQuestion(TriviaQuestion question, int questionNumber, int totalQuestionCount, int score)
    {
        QuestionPanel.Visibility = Visibility.Visible;
        ResultsPanel.Visibility = Visibility.Collapsed;
        ProgressText.Text = $"Question {questionNumber} of {totalQuestionCount}";
        ScoreText.Text = $"Score: {score}";
        QuestionText.Text = question.Question;
        AnswerFeedbackText.Text = string.Empty;
        AnswerFeedbackText.Foreground = Foreground;
        NextQuestionButton.Visibility = Visibility.Collapsed;
        _selectedAnswerIndex = null;
        _answerButtons.Clear();
        AnswersPanel.Children.Clear();

        for (var index = 0; index < question.Answers.Count; index++)
        {
            var answerButton = new RadioButton
            {
                Content = question.Answers[index],
                Tag = index,
                GroupName = "QuestionAnswers",
                FontSize = 16,
                Margin = new Thickness(0, 6, 0, 6)
            };
            answerButton.Checked += OnAnswerChoiceSelected;
            _answerButtons.Add(answerButton);
            AnswersPanel.Children.Add(answerButton);
        }
    }

    public void SetAnswerSubmissionEnabled(bool isEnabled)
    {
        SubmitAnswerButton.IsEnabled = isEnabled;
    }

    public void DisplayAnswerResult(TriviaAnswerResult result)
    {
        foreach (var answerButton in _answerButtons)
        {
            answerButton.IsEnabled = false;
        }

        SubmitAnswerButton.IsEnabled = false;
        AnswerFeedbackText.Foreground = result.IsCorrect ? Brushes.DarkGreen : Brushes.DarkRed;
        AnswerFeedbackText.Text = result.IsCorrect
            ? $"Correct! +10 points. Score: {result.Score}."
            : $"Incorrect. The answer was {result.CorrectAnswer}. -2 points. Score: {result.Score}.";
        NextQuestionButton.Visibility = Visibility.Visible;
    }

    public void DisplayResults(int finalScore, double averageScore)
    {
        QuestionPanel.Visibility = Visibility.Collapsed;
        ResultsPanel.Visibility = Visibility.Visible;
        FinalScoreText.Text = $"Final score: {finalScore}";
        AverageScoreText.Text = $"Average score per question: {averageScore:F1}";
    }

    public void ShowQuestionBankError(string message)
    {
        MessageBox.Show(this, message, "Quiz unavailable", MessageBoxButton.OK, MessageBoxImage.Warning);
    }

    private void OnAnswerChoiceSelected(object sender, RoutedEventArgs e)
    {
        if (sender is RadioButton { Tag: int answerIndex })
        {
            _selectedAnswerIndex = answerIndex;
            AnswerSelected?.Invoke(answerIndex);
        }
    }

    private void OnSubmitAnswer(object sender, RoutedEventArgs e)
    {
        if (_selectedAnswerIndex is int answerIndex)
        {
            AnswerSubmitted?.Invoke(answerIndex);
        }
    }

    private void OnNextQuestion(object sender, RoutedEventArgs e) =>
        NextQuestionRequested?.Invoke();

    private void OnReturnToTitle(object sender, RoutedEventArgs e) =>
        ReturnToTitleRequested?.Invoke();
}
