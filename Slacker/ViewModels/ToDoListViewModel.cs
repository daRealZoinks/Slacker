using CommunityToolkit.Mvvm.Input;
using Slacker.Models;
using Slacker.Services;
using Slacker.Views;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Slacker.ViewModels;

public class ToDoListViewModel
{
    public static ToDoListWindow? ToDoListWindow { get; set; }
    public static ChangePathWindow? ChangePathWindow { get; set; }
    public static ToDoList? ToDoList { get; set; }

    public static ICommand AddRootToDoListCommand => new RelayCommand<object>((object? parameter) =>
    {
        if (ToDoListWindow is not { IsLoaded: true })
        {
            ToDoListWindow = new ToDoListWindow();
            ToDoListWindow.SendToDoList += (ToDoList obj) => { ToDoListServices.AddRootToDoList(obj); };
            ToDoListWindow.Show();
        }

        _ = ToDoListWindow.Focus();
    });

    public static ICommand AddSubToDoListCommand => new RelayCommand<object>((object? parameter) =>
    {
        if (ToDoListWindow is not { IsLoaded: true })
        {
            ToDoListWindow = new ToDoListWindow();
            ToDoList = parameter as ToDoList;
            ToDoListWindow.SendToDoList += (ToDoList obj) =>
            {
                if (ToDoList != null)
                {
                    ToDoListServices.AddSubToDoList(obj, ToDoList);
                }
            };
            ToDoListWindow.Show();
        }

        _ = ToDoListWindow.Focus();
    });

    public static ICommand ChangePathToDoListCommand => new RelayCommand<object>((object? parameter) =>
    {
        ToDoList = parameter as ToDoList;

        if (ToDoList == null)
        {
            _ = MessageBox.Show("You need to select a ToDoList first");
            return;
        }

        if (ChangePathWindow is not { IsLoaded: true })
        {
            ChangePathWindow = new ChangePathWindow();
            ChangePathWindow.SendToDoList += (ToDoList obj) =>
            {
                if (ToDoList != null)
                {
                    ToDoListServices.ChangePathToDoList(ToDoList, obj);
                }
            };
            ChangePathWindow.Show();
        }

        _ = ChangePathWindow.Focus();
    });

    public static ICommand DeleteToDoListCommand => new RelayCommand<object>((object? parameter) =>
    {
        if (parameter is ToDoList toDoList)
        {
            ToDoListServices.DeleteToDoList(toDoList);
        }
    });

    public static ICommand EditToDoListCommand => new RelayCommand<object>((object? parameter) =>
    {
        if (ToDoListWindow is not { IsLoaded: true })
        {
            ToDoList = parameter as ToDoList;

            if (ToDoList == null)
            {
                _ = MessageBox.Show("You need to select a ToDoList");
                return;
            }

            ToDoListWindow = new ToDoListWindow
            {
                ToDoListName =
                {
                    Text = ToDoList.Name
                },
                ToDoListImage =
                {
                    Source = new BitmapImage(new Uri(ToDoList.ImagePath, UriKind.Relative))
                }
            };

            ToDoListWindow.SendToDoList += (ToDoList obj) =>
            {
                if (ToDoList != null)
                {
                    ToDoListServices.EditToDoList(ToDoList, obj);
                }
            };
            ToDoListWindow.Show();
        }

        _ = ToDoListWindow.Focus();
    });

    public static ICommand MoveUpToDoListCommand => new RelayCommand<object>((object? parameter) =>
    {
        if (parameter is ToDoList toDoList)
        {
            ToDoListServices.MoveUpToDoList(toDoList);
        }
    });

    public static ICommand MoveDownToDoListCommand => new RelayCommand<object>((object? parameter) =>
    {
        if (parameter is ToDoList toDoList)
        {
            ToDoListServices.MoveDownToDoList(toDoList);
        }
    });
}
