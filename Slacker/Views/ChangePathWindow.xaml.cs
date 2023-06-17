using System;
using System.Windows;
using Slacker.Models;

namespace Slacker.Views;

/// <summary>
///     Interaction logic for ChangePathWindow.xaml
/// </summary>
public partial class ChangePathWindow : Window
{
    public ChangePathWindow()
    {
        InitializeComponent();
    }

    public event Action<ToDoList>? SendToDoList;

    private void ToDoListTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        if (e.NewValue is ToDoList toDoList)
        {
            SendToDoList?.Invoke(toDoList);
            Close();
        }
    }
}