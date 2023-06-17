using Slacker.Services;
using System.Windows;
using System.Windows.Input;

namespace Slacker.ViewModels;

public class FileViewModel
{
    public FileViewModel()
    {
        if (Application.Current.MainWindow is not MainWindow mainWindow)
        {
            return;
        }

        mainWindow.Loaded += (sender, _) =>
        {
            if (sender is not MainWindow window)
            {
                return;
            }

            CommandBinding closeCommandBinding = new(ApplicationCommands.Close,
                (object sender, ExecutedRoutedEventArgs e) => { Application.Current.MainWindow?.Close(); },
                CanExecute);
            CommandBinding newCommandBinding = new(ApplicationCommands.New,
                (object sender, ExecutedRoutedEventArgs e) => { FileServices.New(); },
                CanExecute);
            CommandBinding openCommandBinding = new(ApplicationCommands.Open,
                (object sender, ExecutedRoutedEventArgs e) => { FileServices.Open(); },
                CanExecute);
            CommandBinding saveCommandBinding = new(ApplicationCommands.Save,
                (object sender, ExecutedRoutedEventArgs e) => { FileServices.Save(); },
                CanExecute);
            CommandBinding saveAsCommandBinding = new(ApplicationCommands.SaveAs,
                (object sender, ExecutedRoutedEventArgs e) => { FileServices.SaveAs(); },
                CanExecute);

            window.SlackerUserControl.CommandBindings.Add(closeCommandBinding);
            window.SlackerUserControl.CommandBindings.Add(newCommandBinding);
            window.SlackerUserControl.CommandBindings.Add(openCommandBinding);
            window.SlackerUserControl.CommandBindings.Add(saveCommandBinding);
            window.SlackerUserControl.CommandBindings.Add(saveAsCommandBinding);
        };
    }

    public static void CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }
}