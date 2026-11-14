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

    public void DisplayEntries(IReadOnlyList<AlphabetReferenceEntry> entries)
    {
        EntriesList.ItemsSource = entries;
    }
}
