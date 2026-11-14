using System.Windows;
using System.Windows.Controls;
using MasterMorse.Presenters.Navigation;

namespace MasterMorse.Views.Navigation;

public partial class AppNavigationBar : UserControl, IAppNavigationBarView
{
    public AppNavigationBar()
    {
        InitializeComponent();
    }

    public event Action<AppFeature>? NavigationRequested;

    public void SetCurrentFeature(AppFeature feature)
    {
        HomeButton.IsEnabled = feature != AppFeature.Home;
        GuidebookButton.IsEnabled = feature != AppFeature.Guidebook;
        QuizButton.IsEnabled = feature != AppFeature.Quiz;
        ReferenceButton.IsEnabled = feature != AppFeature.Reference;
    }

    private void OnHomeRequested(object sender, RoutedEventArgs e) =>
        NavigationRequested?.Invoke(AppFeature.Home);

    private void OnGuidebookRequested(object sender, RoutedEventArgs e) =>
        NavigationRequested?.Invoke(AppFeature.Guidebook);

    private void OnQuizRequested(object sender, RoutedEventArgs e) =>
        NavigationRequested?.Invoke(AppFeature.Quiz);

    private void OnReferenceRequested(object sender, RoutedEventArgs e) =>
        NavigationRequested?.Invoke(AppFeature.Reference);
}
