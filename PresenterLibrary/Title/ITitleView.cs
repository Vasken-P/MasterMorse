namespace MasterMorse.Presenters.Title;

public interface ITitleView
{
    string Title { get; set; }
    event Action? GetStartedRequested;
    event Action? JeopardyQuizRequested;
}
