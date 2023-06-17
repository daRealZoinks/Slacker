using System;
using Slacker.Models;
using Slacker.Repositories;

namespace Slacker.Services;

public class TaskServices
{
    public static void AddTask(ToDoList toDoList, Task task)
    {
        ToDoListRepository.AddTask(toDoList, task);
    }

    public static void DeleteTask(Task task)
    {
        ToDoListRepository.DeleteTask(task);
    }

    public static void EditTask(Task taskToEdit, Task taskInfo)
    {
        ToDoListRepository.EditTask(taskToEdit, taskInfo);
    }

    public static void FindAfterNameTask(string name)
    {
        ToDoListRepository.FindAfterNameTask(name);
    }

    public static void FindAfterDeadlineTask(DateTime deadline)
    {
        ToDoListRepository.FindAfterDeadlineTask(deadline);
    }

    public static void AddCategory(string category)
    {
        CategoryRepository.AddCategory(category);
    }

    public static void ModifyCategory(string categoryToEdit, string categoryInfo)
    {
        CategoryRepository.ModifyCategory(categoryToEdit, categoryInfo);
    }

    public static void DeleteCategory(string category)
    {
        CategoryRepository.DeleteCategory(category);
    }

    public static void MoveDownTask(Task task)
    {
        ToDoListRepository.MoveDownTask(task);
    }

    public static void MoveUpTask(Task task)
    {
        ToDoListRepository.MoveUpTask(task);
    }

    public static void SetDoneTask(Task task)
    {
        ToDoListRepository.SetDoneTask(task);
    }
}