using CommunityToolkit.Mvvm.Input;
using Slacker.Views;
using System.Windows;
using System.Windows.Input;

namespace Slacker.ViewModels;

public class ViewViewModel
{
    private static FilterWindow? FilterWindow { get; set; }

    public static ICommand FilterCommand => new RelayCommand(() =>
    {
        if (FilterWindow is not { IsLoaded: true })
        {
            FilterWindow = new FilterWindow();
            FilterWindow.Show();
        }

        _ = FilterWindow.Focus();
    });

    public static ICommand SortCommand => new RelayCommand(() =>
    {
        MessageBox.Show("You can sort directly from the top of the task list");
    });

    public static ICommand StatisticsCommand => new RelayCommand(() =>
    {
        MessageBox.Show("Statistics can be seen in the bottom left corner");
    });
}