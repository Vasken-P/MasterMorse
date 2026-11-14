using System.Windows;
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

    public void DisplaySections(IReadOnlyList<MorseReferenceSection> sections, ReferenceMode mode)
    {
        SectionsList.ItemsSource = sections;
        AlphabetModeButton.IsChecked = mode == ReferenceMode.Alphabet;
        AbbreviationsModeButton.IsChecked = mode == ReferenceMode.Abbreviations;
    }

    private void OnAlphabetModeSelected(object sender, RoutedEventArgs e) =>
        ModeSelected?.Invoke(ReferenceMode.Alphabet);

    private void OnAbbreviationsModeSelected(object sender, RoutedEventArgs e) =>
        ModeSelected?.Invoke(ReferenceMode.Abbreviations);
}
