using System.Windows;
using MasterMorse.Views;

namespace MasterMorse;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, IMainView
{
    public MainWindow()
    {
        InitializeComponent();
    }
}