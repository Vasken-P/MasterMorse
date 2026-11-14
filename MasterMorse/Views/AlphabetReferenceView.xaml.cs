using System.Windows;
using System.Windows.Controls;
using MasterMorse.Models;
using MasterMorse.Presenters.AlphabetReference;

namespace MasterMorse.Views;

public partial class AlphabetReferenceView : Window, IAlphabetReferenceView
{
    public AlphabetReferenceView()
    {
        InitializeComponent();
    }

    public event Action<ReferenceMode>? ModeSelected;
    public event Action<MorseReferenceEntry>? EntrySelected;

    public void DisplaySections(IReadOnlyList<MorseReferenceSection> sections, ReferenceMode mode)
    {
        SectionsList.ItemsSource = sections;
        AlphabetModeButton.IsChecked = mode == ReferenceMode.Alphabet;
        AbbreviationsModeButton.IsChecked = mode == ReferenceMode.Abbreviations;
    }

    public void SetPlaybackEnabled(bool isEnabled)
    {
        SectionsList.IsEnabled = isEnabled;
        AlphabetModeButton.IsEnabled = isEnabled;
        AbbreviationsModeButton.IsEnabled = isEnabled;
    }

    public void ShowPlaybackError(string message)
    {
        MessageBox.Show(this, message, "Morse audio unavailable",
            MessageBoxButton.OK, MessageBoxImage.Warning);
    }

    private void OnEntryClicked(object sender, RoutedEventArgs e)
    {
        if (sender is Button { Tag: MorseReferenceEntry entry })
        {
            EntrySelected?.Invoke(entry);
        }
    }

    private void OnAlphabetModeSelected(object sender, RoutedEventArgs e) =>
        ModeSelected?.Invoke(ReferenceMode.Alphabet);

    private void OnAbbreviationsModeSelected(object sender, RoutedEventArgs e) =>
        ModeSelected?.Invoke(ReferenceMode.Abbreviations);
}
