using Slacker.Models;
using Slacker.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Slacker.Views;

public partial class FilterWindow : Window
{
    private readonly List<Task> _allTasks;
    private ObservableCollection<Task> ShownTasks { get; }

    public FilterWindow()
    {
        InitializeComponent();

        _allTasks = new List<Task>(ToDoListRepository.GetAllTasks());
        ShownTasks = new ObservableCollection<Task>(_allTasks);

        CategoryComboBox.ItemsSource = CategoryRepository.Categories;

        TaskDataGrid.ItemsSource = ShownTasks;
    }

    private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ShownTasks.Clear();

        foreach (var task in _allTasks)
        {
            if (task.Category == (CategoryComboBox.SelectedItem as string))
            {
                ShownTasks.Add(task);
            }
        }
    }

    private void CategoryRadioButton_Checked(object sender, RoutedEventArgs e)
    {
        ShownTasks.Clear();

        foreach (var task in _allTasks)
        {
            if (task.Category == (CategoryComboBox.SelectedItem as string))
            {
                ShownTasks.Add(task);
            }
        }
    }

    private void DoneRadioButton_Checked(object sender, RoutedEventArgs e)
    {
        ShownTasks.Clear();

        foreach (var task in _allTasks)
        {
            if (task.Status == Status.Done)
            {
                ShownTasks.Add(task);
            }
        }
    }

    private void OverdueRadioButton_Checked(object sender, RoutedEventArgs e)
    {
        ShownTasks.Clear();

        foreach (var task in _allTasks)
        {
            if (task.Deadline < DateTime.Now)
            {
                ShownTasks.Add(task);
            }
        }
    }

    private void OverdueInProgressRadioButton_Checked(object sender, RoutedEventArgs e)
    {
        ShownTasks.Clear();

        foreach (var task in _allTasks)
        {
            if (task.Status != Status.Done && task.Deadline < DateTime.Now)
            {
                ShownTasks.Add(task);
            }
        }
    }

    private void NotDoneRadioButton_Checked(object sender, RoutedEventArgs e)
    {
        ShownTasks.Clear();

        foreach (var task in _allTasks)
        {
            if (task.Status != Status.Done && task.Deadline > DateTime.Now)
            {
                ShownTasks.Add(task);
            }
        }
    }
}