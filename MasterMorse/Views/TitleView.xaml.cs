using System.Windows;
using MasterMorse.Presenters.Title;

namespace MasterMorse.Views;

public partial class TitleView : Window, ITitleView
{
    public TitleView()
    {
        InitializeComponent();
    }

    public event Action? GetStartedRequested;
    public event Action? JeopardyQuizRequested;

    private void OnGetStarted(object sender, RoutedEventArgs e) =>
        GetStartedRequested?.Invoke();

    private void OnQuizRequested(object sender, RoutedEventArgs e) =>
        JeopardyQuizRequested?.Invoke();
}
