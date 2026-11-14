using System.Windows;
using MasterMorse.Models;
using MasterMorse.Presenters.Guidebook;
using MasterMorse.Views;

namespace MasterMorse;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var view = new GuidebookView();
        var presenter = new GuidebookPresenter(view, new GuidebookContentProvider());
        presenter.Initialize();
        MainWindow = view;
        view.Show();
    }
}
