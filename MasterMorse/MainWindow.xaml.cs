using System.Windows;
using System.Windows.Controls;
using MasterMorse.Model;
using MasterMorse.Views;

namespace MasterMorse;

/// <summary>
/// The view. It holds no translation logic: it forwards user input to the presenter
/// through events and displays whatever the presenter sets.
/// </summary>
public partial class MainWindow : Window, IMainView
{
    public MainWindow()
    {
        InitializeComponent();
    }

    public event Action<string>? InputChanged;

    public event Action<MainCommand>? CommandRequested;

    public event Action<Difficulty>? DuelRequested;

    public string InputHeader
    {
        get => InputHeaderText.Text;
        set => InputHeaderText.Text = value;
    }

    public string OutputHeader
    {
        get => OutputHeaderText.Text;
        set => OutputHeaderText.Text = value;
    }

    public string InputText
    {
        get => InputTextBox.Text;
        set => InputTextBox.Text = value;
    }

    public string OutputText
    {
        get => OutputTextBox.Text;
        set => OutputTextBox.Text = value;
    }

    public string ValidationMessage
    {
        get => ValidationText.Text;
        set => ValidationText.Text = value;
    }

    public string StatusMessage
    {
        get => StatusText.Text;
        set => StatusText.Text = value;
    }

    public string PauseButtonText
    {
        get => (string)PauseButton.Content;
        set => PauseButton.Content = value;
    }

    public bool IsMorseInput { get; set; }

    private bool _normalizingInput;

    private void InputTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (_normalizingInput)
        {
            return;
        }

        var text = InputTextBox.Text;
        if (IsMorseInput)
        {
            // Every dot typed or pasted into Morse input becomes the large dot glyph.
            // The replacement keeps the same length, so the caret position is preserved.
            var display = MorseDisplay.ToDisplay(MorseDisplay.FromDisplay(text));
            if (display != text)
            {
                var caret = InputTextBox.CaretIndex;
                _normalizingInput = true;
                try
                {
                    InputTextBox.Text = display;
                    InputTextBox.CaretIndex = caret;
                }
                finally
                {
                    _normalizingInput = false;
                }

                text = display;
            }
        }

        InputChanged?.Invoke(text);
    }

    private void Translate_Click(object sender, RoutedEventArgs e) => Raise(MainCommand.Translate);

    private void Swap_Click(object sender, RoutedEventArgs e) => Raise(MainCommand.Swap);

    private void Clear_Click(object sender, RoutedEventArgs e) => Raise(MainCommand.Clear);

    private void Copy_Click(object sender, RoutedEventArgs e) => Raise(MainCommand.Copy);

    private void Play_Click(object sender, RoutedEventArgs e) => Raise(MainCommand.Play);

    private void Pause_Click(object sender, RoutedEventArgs e) => Raise(MainCommand.Pause);

    private void Stop_Click(object sender, RoutedEventArgs e) => Raise(MainCommand.Stop);

    private void DuelEasy_Click(object sender, RoutedEventArgs e) => DuelRequested?.Invoke(Difficulty.Easy);

    private void DuelHard_Click(object sender, RoutedEventArgs e) => DuelRequested?.Invoke(Difficulty.Hard);

    private void Raise(MainCommand command) => CommandRequested?.Invoke(command);
}
