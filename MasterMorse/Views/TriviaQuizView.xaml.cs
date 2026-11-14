using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MasterMorse.Models.Trivia;
using MasterMorse.Presenters.Trivia;

namespace MasterMorse.Views;

public partial class TriviaQuizView : Window, ITriviaQuizView
{
    public TriviaQuizView()
    {
        InitializeComponent();
    }

    public event Action<string>? ClueSelected;
    public event Action<string>? AnswerSubmitted;
    public event Action? BoardRequested;
    public event Action? ReturnToTitleRequested;

    public void DisplayBoard(
        IReadOnlyList<TriviaBoardCategory> categories,
        int score,
        int answeredQuestionCount,
        int totalQuestionCount)
    {
        BoardPanel.Visibility = Visibility.Visible;
        CluePanel.Visibility = Visibility.Collapsed;
        ResultsPanel.Visibility = Visibility.Collapsed;
        CategoriesList.ItemsSource = categories;
        ScoreText.Text = $"Score: {score}";
        ProgressText.Text = $"Clues answered: {answeredQuestionCount} of {totalQuestionCount}";
    }

    public void DisplayClue(
        TriviaQuestion question,
        int score,
        int answeredQuestionCount,
        int totalQuestionCount)
    {
        BoardPanel.Visibility = Visibility.Collapsed;
        CluePanel.Visibility = Visibility.Visible;
        ResultsPanel.Visibility = Visibility.Collapsed;
        ClueCategoryText.Text = question.Category;
        ClueText.Text = question.Clue;
        ClueScoreText.Text = $"Score: {score}  |  Clues answered: {answeredQuestionCount} of {totalQuestionCount}";
        AnswerInput.Clear();
        AnswerInput.IsEnabled = true;
        SubmitAnswerButton.IsEnabled = true;
        AnswerFeedbackText.Text = string.Empty;
        BackToBoardButton.Visibility = Visibility.Collapsed;
    }

    public void DisplayAnswerResult(TriviaAnswerResult result)
    {
        AnswerInput.IsEnabled = false;
        SubmitAnswerButton.IsEnabled = false;
        AnswerFeedbackText.Foreground = result.IsCorrect ? Brushes.DarkGreen : Brushes.DarkRed;
        AnswerFeedbackText.Text = result.IsCorrect
            ? $"Correct! +10 points. Score: {result.Score}."
            : $"Incorrect. The answer was {result.CorrectAnswer}. -2 points. Score: {result.Score}.";
        BackToBoardButton.Visibility = Visibility.Visible;
    }

    public void DisplayResults(int finalScore, double averageScore)
    {
        BoardPanel.Visibility = Visibility.Collapsed;
        CluePanel.Visibility = Visibility.Collapsed;
        ResultsPanel.Visibility = Visibility.Visible;
        FinalScoreText.Text = $"Final score: {finalScore}";
        AverageScoreText.Text = $"Average score per question: {averageScore:F1}";
    }

    public void ShowQuestionBankError(string message)
    {
        MessageBox.Show(this, message, "Quiz unavailable", MessageBoxButton.OK, MessageBoxImage.Warning);
    }

    private void OnClueTileSelected(object sender, RoutedEventArgs e)
    {
        if (sender is Button { Tag: string questionId })
        {
            ClueSelected?.Invoke(questionId);
        }
    }

    private void OnSubmitAnswer(object sender, RoutedEventArgs e) =>
        AnswerSubmitted?.Invoke(AnswerInput.Text);

    private void OnBackToBoard(object sender, RoutedEventArgs e) =>
        BoardRequested?.Invoke();

    private void OnReturnToTitle(object sender, RoutedEventArgs e) =>
        ReturnToTitleRequested?.Invoke();
}
