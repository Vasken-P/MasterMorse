using MasterMorse.Infrastructure;
using MasterMorse.Model;

namespace MasterMorse.Infrastructure;

/// <summary>
/// Plays Morse signals with the Windows system beep. Pause and stop take effect
/// between signal parts; the tone currently sounding is allowed to finish.
/// </summary>
public sealed class BeepMorsePlayer : IMorsePlayer
{
    private const int ToneFrequencyHz = 650;
    private const int UnitMilliseconds = 80;

    private readonly ManualResetEventSlim _resumeSignal = new(initialState: true);
    private CancellationTokenSource? _cts;

    public bool IsPaused => !_resumeSignal.IsSet;

    public async Task PlayAsync(IReadOnlyList<SignalPart> signals)
    {
        Stop();

        var cts = new CancellationTokenSource();
        _cts = cts;
        _resumeSignal.Set();

        try
        {
            await Task.Run(() => PlaySignals(signals, cts.Token));
        }
        catch (OperationCanceledException)
        {
            // Stop was requested; playback ends quietly.
        }
        finally
        {
            if (ReferenceEquals(_cts, cts))
            {
                _cts = null;
            }
        }
    }

    public void Pause() => _resumeSignal.Reset();

    public void Resume() => _resumeSignal.Set();

    public void Stop()
    {
        var cts = _cts;
        _cts = null;
        cts?.Cancel();

        // Wake a paused playback so it can observe the cancellation.
        _resumeSignal.Set();
    }

    private void PlaySignals(IReadOnlyList<SignalPart> signals, CancellationToken token)
    {
        foreach (var part in signals)
        {
            _resumeSignal.Wait(token);
            token.ThrowIfCancellationRequested();

            var duration = part.Units * UnitMilliseconds;
            if (part.Kind == SignalKind.Tone)
            {
                Console.Beep(ToneFrequencyHz, duration);
            }
            else if (token.WaitHandle.WaitOne(duration))
            {
                throw new OperationCanceledException(token);
            }
        }
    }
}
