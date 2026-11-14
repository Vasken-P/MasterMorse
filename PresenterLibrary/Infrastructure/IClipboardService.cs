namespace MasterMorse.Infrastructure;

/// <summary>Clipboard access. Implemented by the application layer.</summary>
public interface IClipboardService
{
    /// <summary>Returns false if the clipboard could not be written.</summary>
    bool TrySetText(string text);
}
