namespace MasterMorse.Models;

public interface IGuidebookContentProvider
{
    IReadOnlyList<GuidebookLesson> GetLessons();
}
