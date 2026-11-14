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
    public IAppNavigationBarView NavigationBar => NavigationBarControl;

    private void OnGetStarted(object sender, RoutedEventArgs e) =>
        GetStartedRequested?.Invoke();
}
