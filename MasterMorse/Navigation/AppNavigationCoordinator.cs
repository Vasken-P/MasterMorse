using System.Windows;
using MasterMorse.Models;
using MasterMorse.Models.Trivia;
using MasterMorse.Presenters.AlphabetReference;
using MasterMorse.Presenters.Guidebook;
using MasterMorse.Presenters.Navigation;
using MasterMorse.Presenters.Title;
using MasterMorse.Presenters.Trivia;
using MasterMorse.Views;
using MasterMorse.Views.Navigation;

namespace MasterMorse.Navigation;

public sealed class AppNavigationCoordinator
{
    private readonly TitleView _titleView;
    private GuidebookView? _guidebookView;
    private TriviaQuizView? _quizView;
    private AlphabetReferenceView? _referenceView;
    private Window? _activeWindow;

    public AppNavigationCoordinator(TitleView titleView)
    {
        _titleView = titleView;
    }

    public void Initialize()
    {
        _titleView.NavigationBar.NavigationRequested += Navigate;
        var titlePresenter = new TitlePresenter(
            _titleView, () => Navigate(AppFeature.Guidebook));
        titlePresenter.Initialize();
        Navigate(AppFeature.Home);
    }

    private void Navigate(AppFeature feature)
    {
        Window? target = feature switch
        {
            AppFeature.Home => _titleView,
            AppFeature.Guidebook => GetGuidebookView(),
            AppFeature.Quiz => GetQuizView(),
            AppFeature.Reference => GetReferenceView(),
            _ => throw new ArgumentOutOfRangeException(nameof(feature), feature, "Unknown app feature.")
        };

        if (target is null || ReferenceEquals(target, _activeWindow))
        {
            return;
        }

        _activeWindow?.Hide();
        _activeWindow = target;
        GetNavigationBar(target).SetCurrentFeature(feature);
        if (!target.IsVisible)
        {
            target.Show();
        }

        target.Activate();
    }

    private GuidebookView GetGuidebookView()
    {
        if (_guidebookView is not null)
        {
            return _guidebookView;
        }

        var view = new GuidebookView();
        var presenter = new GuidebookPresenter(
            view, new GuidebookContentProvider(), () => Navigate(AppFeature.Home));
        presenter.Initialize();
        view.NavigationBar.NavigationRequested += Navigate;
        view.Closed += (_, _) =>
        {
            _guidebookView = null;
            ReturnHomeIfClosed(view);
        };
        _guidebookView = view;
        return view;
    }

    private TriviaQuizView? GetQuizView()
    {
        if (_quizView is not null)
        {
            return _quizView;
        }

        var view = new TriviaQuizView();
        var presenter = new TriviaQuizPresenter(
            view,
            new JsonTriviaQuestionBank(),
            new TriviaQuizSessionFactory(),
            () => Navigate(AppFeature.Home));
        if (!presenter.Initialize())
        {
            return null;
        }

        view.NavigationBar.NavigationRequested += Navigate;
        view.Closed += (_, _) =>
        {
            _quizView = null;
            ReturnHomeIfClosed(view);
        };
        _quizView = view;
        return view;
    }

    private AlphabetReferenceView GetReferenceView()
    {
        if (_referenceView is not null)
        {
            return _referenceView;
        }

        var view = new AlphabetReferenceView();
        var presenter = new AlphabetReferencePresenter(
            view, new AlphabetReferenceProvider(), new MorsePlaybackService());
        presenter.Initialize();
        view.NavigationBar.NavigationRequested += Navigate;
        view.Closed += (_, _) =>
        {
            _referenceView = null;
            ReturnHomeIfClosed(view);
        };
        _referenceView = view;
        return view;
    }

    private static IAppNavigationBarView GetNavigationBar(Window window) =>
        window switch
        {
            TitleView title => title.NavigationBar,
            GuidebookView guidebook => guidebook.NavigationBar,
            TriviaQuizView quiz => quiz.NavigationBar,
            AlphabetReferenceView reference => reference.NavigationBar,
            _ => throw new ArgumentOutOfRangeException(nameof(window), "Unknown app window.")
        };

    private void ReturnHomeIfClosed(Window window)
    {
        if (ReferenceEquals(window, _activeWindow))
        {
            _activeWindow = null;
            Navigate(AppFeature.Home);
        }
    }
}
