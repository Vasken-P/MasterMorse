using System.Windows;
using MasterMorse.Infrastructure;
using MasterMorse.Model;
using MasterMorse.Presenters;
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

        var view = new MainWindow();
        var presenter = new MainPresenter(view, new BeepMorsePlayer(), new WpfClipboardService());
        presenter.Initialize();
        view.DuelRequested += OpenDuel;
        MainWindow = view;
        view.Show();
    }

    /// <summary>Opens a new 1v1 duel window in the chosen mode, with its own presenter.</summary>
    private static void OpenDuel(Difficulty difficulty)
    {
        var duel = new DuelWindow(difficulty);
        var duelPresenter = new MatchPresenter(duel);
        duelPresenter.Initialize();
        duel.Show();
    }
}
