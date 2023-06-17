using Slacker.Models;
using Slacker.Repositories;
using System;

namespace Slacker.Views;

/// <summary>
///     Interaction logic for SlackerUserControl.xaml
/// </summary>
public partial class SlackerUserControl
{
    public SlackerUserControl()
    {
        InitializeComponent();

        StatusColumn.ItemsSource = Enum.GetValues(typeof(Status));
        PriorityColumn.ItemsSource = Enum.GetValues(typeof(Priority));
        CategoryColumn.ItemsSource = CategoryRepository.Categories;

        ToDoListRepository.RepositoryUpdated += UpdateStatus;
    }

    private void UpdateStatus()
    {
        TasksDueTodayTextBlock.Text = $"Tasks due today: {ToDoListRepository.TasksDueToday}";
        TasksDueTomorrowTextBlock.Text = $"Tasks due tomorrow: {ToDoListRepository.TasksDueTomorrow}";
        TasksOverdueTextBlock.Text = $"Tasks overdue: {ToDoListRepository.TasksOverdue}";
        TasksDoneTextBlock.Text = $"Tasks done: {ToDoListRepository.TasksDone}";
        TasksToBeDoneTextBlock.Text = $"Tasks to be done: {ToDoListRepository.TasksToBeDone}";
    }
}