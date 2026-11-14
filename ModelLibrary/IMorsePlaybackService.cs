namespace MasterMorse.Models;

public interface IMorsePlaybackService
{
    Task PlayAsync(string pattern);
}
