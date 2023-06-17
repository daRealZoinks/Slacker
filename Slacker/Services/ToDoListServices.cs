using Slacker.Models;
using Slacker.Repositories;

namespace Slacker.Services;

public static class ToDoListServices
{
    public static void AddRootToDoList(ToDoList toDoList)
    {
        ToDoListRepository.AddRootToDoList(toDoList);
    }

    public static void AddSubToDoList(ToDoList toDoList, ToDoList parentToDoList)
    {
        ToDoListRepository.AddSubToDoList(toDoList, parentToDoList);
    }

    public static void ChangePathToDoList(ToDoList toDoList, ToDoList parentToDoList)
    {
        ToDoListRepository.ChangePathToDoList(toDoList, parentToDoList);
    }

    public static void DeleteToDoList(ToDoList toDoList)
    {
        ToDoListRepository.DeleteToDoList(toDoList);
    }

    public static void EditToDoList(ToDoList toDoListToBeChanged, ToDoList toDoListInfo)
    {
        ToDoListRepository.EditToDoList(toDoListToBeChanged, toDoListInfo);
    }

    public static void MoveUpToDoList(ToDoList toDoList)
    {
        ToDoListRepository.MoveUpToDoList(toDoList);
    }

    public static void MoveDownToDoList(ToDoList toDoList)
    {
        ToDoListRepository.MoveDownToDoList(toDoList);
    }
}