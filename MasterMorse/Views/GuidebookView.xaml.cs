using System.Windows;
using System.Windows.Media;
using MasterMorse.Models;
using MasterMorse.Presenters.Navigation;
using MasterMorse.Presenters.Guidebook;

namespace MasterMorse.Views;

/// <summary>
/// Interaction logic for GuidebookView.xaml
/// </summary>
public partial class GuidebookView : Window, IGuidebookView
{
    public GuidebookView()
    {
        InitializeComponent();
    }

    public event Action<string>? PracticeAnswerSubmitted;
    public event Action? ReturnRequested;
    public IAppNavigationBarView NavigationBar => NavigationBarControl;

    public void DisplayLessons(IReadOnlyList<GuidebookLesson> lessons)
    {
        LessonsList.ItemsSource = lessons;
    }

    public void DisplayPracticeQuestion(string prompt)
    {
        PracticePromptText.Text = prompt;
        PracticeAnswerText.Clear();
        PracticeFeedbackText.Text = string.Empty;
    }

    public void ShowPracticeFeedback(string message, bool isCorrect)
    {
        PracticeFeedbackText.Text = message;
        PracticeFeedbackText.Foreground = isCorrect ? Brushes.DarkGreen : Brushes.DarkRed;
    }

    public void SetPracticeSubmissionEnabled(bool isEnabled)
    {
        SubmitAnswerButton.IsEnabled = isEnabled;
    }

    private void OnSubmitAnswer(object sender, RoutedEventArgs e) =>
        PracticeAnswerSubmitted?.Invoke(PracticeAnswerText.Text);

    private void OnReturnToTitle(object sender, RoutedEventArgs e) =>
        ReturnRequested?.Invoke();
}