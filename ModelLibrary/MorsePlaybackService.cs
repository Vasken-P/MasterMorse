namespace MasterMorse.Models;

public sealed class MorsePlaybackService : IMorsePlaybackService
{
    private const int FrequencyHertz = 800;
    private const int DotDurationMilliseconds = 100;
    private const int DashDurationMilliseconds = 3 * DotDurationMilliseconds;
    private const int SignalPauseMilliseconds = DotDurationMilliseconds;
    private const int LetterPauseMilliseconds = 3 * DotDurationMilliseconds;
    private const int WordPauseMilliseconds = 7 * DotDurationMilliseconds;

    private static readonly SemaphoreSlim PlaybackLock = new(1, 1);

    public async Task PlayAsync(string pattern)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(pattern);

        await PlaybackLock.WaitAsync().ConfigureAwait(false);
        try
        {
            await Task.Run(() => PlayPattern(pattern)).ConfigureAwait(false);
        }
        finally
        {
            PlaybackLock.Release();
        }
    }

    private static void PlayPattern(string pattern)
    {
        if (!OperatingSystem.IsWindows())
        {
            throw new PlatformNotSupportedException("Console.Beep playback requires Windows.");
        }

        for (var index = 0; index < pattern.Length; index++)
        {
            switch (pattern[index])
            {
                case '·':
                case '.':
                    Console.Beep(FrequencyHertz, DotDurationMilliseconds);
                    break;
                case '-':
                    Console.Beep(FrequencyHertz, DashDurationMilliseconds);
                    break;
                default:
                    throw new ArgumentException(
                        $"Unsupported Morse pattern character: '{pattern[index]}'.",
                        nameof(pattern));
            }

            var nextIndex = index + 1;
            if (nextIndex == pattern.Length)
            {
                continue;
            }

            if (IsSignal(pattern[nextIndex]))
            {
                Thread.Sleep(SignalPauseMilliseconds);
                continue;
            }

            if (!char.IsWhiteSpace(pattern[nextIndex]))
            {
                throw new ArgumentException(
                    $"Unsupported Morse pattern character: '{pattern[nextIndex]}'.",
                    nameof(pattern));
            }

            var separatorLength = 0;
            while (nextIndex + separatorLength < pattern.Length
                   && char.IsWhiteSpace(pattern[nextIndex + separatorLength]))
            {
                separatorLength++;
            }

            var followingSignalIndex = nextIndex + separatorLength;
            if (followingSignalIndex == pattern.Length)
            {
                break;
            }

            if (!IsSignal(pattern[followingSignalIndex]))
            {
                throw new ArgumentException(
                    $"Unsupported Morse pattern character: '{pattern[followingSignalIndex]}'.",
                    nameof(pattern));
            }

            Thread.Sleep(separatorLength == 1
                ? LetterPauseMilliseconds
                : WordPauseMilliseconds);
            index = followingSignalIndex - 1;
        }
    }

    private static bool IsSignal(char character) =>
        character is '·' or '.' or '-';
}
