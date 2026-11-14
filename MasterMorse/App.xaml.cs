using System.Windows;
using MasterMorse.Presenters;

namespace MasterMorse;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var view = new MainWindow();
        var presenter = new MainPresenter(view);
        presenter.Initialize();
        MainWindow = view;
        view.Show();
    }
}
