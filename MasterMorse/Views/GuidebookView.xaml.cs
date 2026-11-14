using System.Windows;
using System.Windows.Media;
using MasterMorse.Models;
using MasterMorse.Presenters.Guidebook;

namespace MasterMorse.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class GuidebookView : Window, IGuidebookView
{
    public GuidebookView()
    {
        InitializeComponent();
    }

    public event Action? PreviousLessonRequested;
    public event Action? NextLessonRequested;
    public event Action<string>? PracticeAnswerSubmitted;

    public void DisplayLesson(GuidebookLesson lesson, int lessonNumber, int totalLessons)
    {
        LessonCountText.Text = $"Lesson {lessonNumber} of {totalLessons}";
        LessonTitleText.Text = lesson.Title;
        LessonExplanationText.Text = lesson.Explanation;
        ExamplesList.ItemsSource = lesson.Examples;
        PracticePromptText.Text = lesson.PracticeQuestion.Prompt;
        PracticeAnswerText.Clear();
        PracticeFeedbackText.Text = string.Empty;
    }

    public void ShowPracticeFeedback(string message, bool isCorrect)
    {
        PracticeFeedbackText.Text = message;
        PracticeFeedbackText.Foreground = isCorrect ? Brushes.DarkGreen : Brushes.DarkRed;
    }

    public void SetNavigationEnabled(bool canGoPrevious, bool canGoNext)
    {
        PreviousLessonButton.IsEnabled = canGoPrevious;
        NextLessonButton.IsEnabled = canGoNext;
    }

    public void SetPracticeSubmissionEnabled(bool isEnabled)
    {
        SubmitAnswerButton.IsEnabled = isEnabled;
    }

    private void OnPreviousLesson(object sender, RoutedEventArgs e) =>
        PreviousLessonRequested?.Invoke();

    private void OnNextLesson(object sender, RoutedEventArgs e) =>
        NextLessonRequested?.Invoke();

    private void OnSubmitAnswer(object sender, RoutedEventArgs e) =>
        PracticeAnswerSubmitted?.Invoke(PracticeAnswerText.Text);
}