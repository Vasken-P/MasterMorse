using MasterMorse.Model;

namespace MasterMorse.Infrastructure;

/// <summary>Plays Morse signals as audio. Implemented by the application layer.</summary>
public interface IMorsePlayer
{
    bool IsPaused { get; }

    Task PlayAsync(IReadOnlyList<SignalPart> signals);

    void Pause();

    void Resume();

    void Stop();
}
