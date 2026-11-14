using System.Runtime.InteropServices;
using System.Windows;

namespace MasterMorse.Infrastructure;

/// <summary>Writes text to the Windows clipboard through WPF. Must run on the UI thread.</summary>
public sealed class WpfClipboardService : IClipboardService
{
    public bool TrySetText(string text)
    {
        try
        {
            Clipboard.SetText(text);
            return true;
        }
        catch (ExternalException)
        {
            return false;
        }
    }
}
