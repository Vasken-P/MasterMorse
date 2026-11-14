namespace MasterMorse.Presenters.Navigation;

public interface IAppNavigationBarView
{
    event Action<AppFeature>? NavigationRequested;
    void SetCurrentFeature(AppFeature feature);
}
