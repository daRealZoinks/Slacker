using Slacker.Services;
using System.ComponentModel;
using System.Windows;

namespace Slacker;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        if (!FileServices.Saved)
        {
            var result = MessageBox.Show("Do you want to save the database?",
                "Slacker",
                MessageBoxButton.YesNoCancel);

            switch (result)
            {
                case MessageBoxResult.Cancel:
                    e.Cancel = true;
                    break;
                case MessageBoxResult.Yes:
                    FileServices.Save();
                    break;
                case MessageBoxResult.No:
                    break;
                case MessageBoxResult.None:
                    break;
                case MessageBoxResult.OK:
                    break;
                default:
                    e.Cancel = true;
                    break;
            }
        }

        base.OnClosing(e);
    }
}
