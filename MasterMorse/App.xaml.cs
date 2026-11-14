using System.Windows;
using MasterMorse.Models;
using MasterMorse.Presenters.Guidebook;
using MasterMorse.Presenters.Title;
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
        var titlePresenter = new TitlePresenter(titleView, () => OpenGuidebook(titleView));
        titlePresenter.Initialize();
        MainWindow = titleView;
        titleView.Show();
    }

    private static void OpenGuidebook(TitleView titleView)
    {
        var guidebookView = new GuidebookView();
        var presenter = new GuidebookPresenter(
            guidebookView, new GuidebookContentProvider(), guidebookView.Close);
        presenter.Initialize();
        guidebookView.Closed += (_, _) => titleView.Show();
        guidebookView.Show();
        titleView.Hide();
    }
}
