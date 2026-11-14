using System.Windows;
using MasterMorse.Models;
using MasterMorse.Presenters.Guidebook;
using MasterMorse.Presenters.Title;
using MasterMorse.Presenters.Trivia;
using MasterMorse.Models.Trivia;
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

        var titleView = new TitleView();
        var titlePresenter = new TitlePresenter(
            titleView,
            () => OpenGuidebook(titleView),
            () => OpenJeopardyQuiz(titleView));
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

    private static void OpenJeopardyQuiz(TitleView titleView)
    {
        var quizView = new TriviaQuizView();
        var presenter = new TriviaQuizPresenter(
            quizView,
            new JsonTriviaQuestionBank(),
            new JeopardyQuizGameFactory(),
            quizView.Close);
        if (!presenter.Initialize())
        {
            return;
        }

        quizView.Closed += (_, _) => titleView.Show();
        quizView.Show();
        titleView.Hide();
    }
}
