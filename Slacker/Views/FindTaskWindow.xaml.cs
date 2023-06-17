using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Slacker.Models;
using Slacker.Repositories;

namespace Slacker.Views;

/// <summary>
///     Interaction logic for FindTaskAfterName.xaml
/// </summary>
public partial class FindTaskWindow
{
    private readonly List<Task> _allTasks;
    public Action<Task>? OnTaskSelected;

    public FindTaskWindow()
    {
        InitializeComponent();

        _allTasks = new List<Task>(ToDoListRepository.GetAllTasks());
        ShownTasks = new ObservableCollection<Task>(_allTasks);

        TaskDataGrid.ItemsSource = ShownTasks;
    }

    private ObservableCollection<Task> ShownTasks { get; }

    private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        ShownTasks.Clear();

        foreach (var task in _allTasks.Where(task => task.Name.Contains(NameSearchTextBox.Text))) ShownTasks.Add(task);
    }

    private void DeadlineSearchCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
    {
        ShownTasks.Clear();

        foreach (var task in _allTasks.Where(task => task.Deadline.Date == DeadlineSearchCalendar.SelectedDate))
            ShownTasks.Add(task);
    }

    private void SelectTaskButton_Click(object sender, RoutedEventArgs e)
    {
        OnTaskSelected?.Invoke((Task)TaskDataGrid.SelectedItem);
        Close();
    }
}