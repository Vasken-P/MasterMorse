using System.Windows;
using MasterMorse.Presenters.Navigation;
using MasterMorse.Presenters.Title;

namespace MasterMorse.Views;

public partial class TitleView : Window, ITitleView
{
    public TitleView()
    {
        InitializeComponent();
    }

    public event Action? GetStartedRequested;
    public event Action? QuizRequested;
    public IAppNavigationBarView NavigationBar => NavigationBarControl;

    private void OnGetStarted(object sender, RoutedEventArgs e) =>
        GetStartedRequested?.Invoke();

    private void OnQuizRequested(object sender, RoutedEventArgs e) =>
        QuizRequested?.Invoke();
}
