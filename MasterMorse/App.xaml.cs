using System.Windows;
using MasterMorse.Navigation;
using MasterMorse.Views;

namespace MasterMorse;

/// <summary>
/// Composition root: creates the view and its infrastructure services,
/// then connects them to the presenter.
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var titleView = new TitleView();
        MainWindow = titleView;
        new AppNavigationCoordinator(titleView).Initialize();
    }
}
