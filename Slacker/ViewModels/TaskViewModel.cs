using CommunityToolkit.Mvvm.Input;
using Slacker.Models;
using Slacker.Repositories;
using Slacker.Services;
using Slacker.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Slacker.ViewModels;

public class TaskViewModel
{
    public TaskViewModel()
    {
        // Modifiying the description of a task in real time
        if (Application.Current.MainWindow is not MainWindow mainWindow) return;

        mainWindow.Loaded += (sender, e) =>
        {
            if (sender is not MainWindow mainWindow) return;

            mainWindow.SlackerUserControl.DescriptionTextBox.TextChanged += (sender, e) =>
            {
                if (mainWindow.SlackerUserControl.TaskDataGrid.SelectedItem is Task selectedTask)
                    selectedTask.Description = mainWindow.SlackerUserControl.DescriptionTextBox.Text;
            };
        };
    }

    public static CategoryWindow? CategoryWindow { get; set; }
    public static FindTaskWindow? FindTaskWindow { get; set; }


    public static ICommand AddTaskCommand => new RelayCommand(() =>
    {
        MessageBox.Show("Select a ToDoList and then press on the empty row to add a task");
    });

    public static ICommand DeleteTaskCommand => new RelayCommand(() =>
    {
        MessageBox.Show("Press the delete key to delete a task");
    });

    public static ICommand EditTaskCommand => new RelayCommand(() =>
    {
        MessageBox.Show("Click on any property of a task to edit it");
    });

    public static ICommand FindTaskCommand => new RelayCommand(() =>
    {
        if (FindTaskWindow is not { IsLoaded: true })
        {
            FindTaskWindow = new FindTaskWindow();
            FindTaskWindow.OnTaskSelected += FindTaskAfterName_OnTaskSelected;
            FindTaskWindow.Show();
        }

        _ = FindTaskWindow.Focus();
    });

    public static ICommand ManageCategoryTaskCommand => new RelayCommand(() =>
    {
        if (CategoryWindow is not { IsLoaded: true })
        {
            CategoryWindow = new CategoryWindow();
            CategoryWindow.Show();
        }

        _ = CategoryWindow.Focus();
    });

    public static ICommand MoveDownTaskCommand => new RelayCommand<object>((object? parameter) =>
    {
        if (parameter is not Task task)
        {
            MessageBox.Show("Select a task first");
            return;
        }

        TaskServices.MoveDownTask(task);
    });

    public static ICommand MoveUpTaskCommand => new RelayCommand<object>((object? parameter) =>
    {
        if (parameter is not Task task)
        {
            MessageBox.Show("Select a task first");
            return;
        }

        TaskServices.MoveUpTask(task);
    });

    public static ICommand SetDoneTaskCommand => new RelayCommand(() =>
    {
        MessageBox.Show("Click on the status of a task to set it to done");
    });

    private static void FindTaskAfterName_OnTaskSelected(Task obj)
    {
        var toDoList = ToDoListRepository.GetToDoListContainingTask(obj);

        if (((MainWindow)Application.Current.MainWindow).SlackerUserControl.ToDoListTreeView.ItemContainerGenerator.ContainerFromItem(toDoList) is TreeViewItem treeViewItem)
        {
            treeViewItem.IsSelected = true;
        }
        ((MainWindow)Application.Current.MainWindow).SlackerUserControl.TaskDataGrid.SelectedItem = obj;
    }
}