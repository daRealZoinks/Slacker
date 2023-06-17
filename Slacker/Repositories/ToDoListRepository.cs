using Slacker.Models;
using Slacker.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Slacker.Repositories;

/// <summary>
///     Represents the hierarchy of ToDoLists and provides functions for high-level access
/// </summary>
public class ToDoListRepository
{
    public ToDoListRepository()
    {
        RootToDoLists.CollectionChanged += (_, _) => { Update(); };

        // Dirty the file whenever the RootToDoLists gets changed
        RepositoryUpdated += () => FileServices.Saved = false;
    }


    /// <summary>
    ///     The root ToDoLists
    /// </summary>
    public static ObservableCollection<ToDoList> RootToDoLists { get; } = new();


    /// <summary>
    ///     Gets called whenever the repository changes
    /// </summary>
    public static event Action? RepositoryUpdated;


    /// <summary>
    ///     Invokes <see cref="RepositoryUpdated" />, should be called only by <see cref="BaseModel" />
    /// </summary>
    public static void Update()
    {
        RepositoryUpdated?.Invoke();
    }


    #region Statistics

    /// <summary>
    ///     The number of tasks due today
    /// </summary>
    public static int TasksDueToday => GetAllTasks().Count(task => task.Deadline.Date == DateTime.Now.Date);


    /// <summary>
    ///     The number of tasks due tomorrow
    /// </summary>
    public static int TasksDueTomorrow => GetAllTasks().Count(task => task.Deadline.Date == DateTime.Now.Date.AddDays(1));


    /// <summary>
    ///     The number of tasks overdue
    /// </summary>
    public static int TasksOverdue => GetAllTasks().Count(task => task.Deadline.Date < DateTime.Now.Date);


    /// <summary>
    ///     The number of tasks done
    /// </summary>
    public static int TasksDone => GetAllTasks().Count(task => task.Status == Status.Done);


    /// <summary>
    ///     The number of tasks to be done
    /// </summary>
    public static int TasksToBeDone => GetAllTasks().Count(task => task.Status != Status.Done);


    /// <summary>
    ///     Gets all tasks in the hierarchy
    /// </summary>
    /// <param name="toDoList">
    ///     The ToDoList to start from, if null, the root ToDoLists will be used
    /// </param>
    /// <returns>
    ///     All tasks in the hierarchy
    /// </returns>
    public static List<Task> GetAllTasks(ToDoList? toDoList = null)
    {
        List<Task> allTasks = new();

        var list = toDoList == null ? RootToDoLists : toDoList.ToDoLists;

        foreach (var subToDoList in list)
        {
            allTasks.AddRange(subToDoList.Tasks);
            allTasks.AddRange(GetAllTasks(subToDoList));
        }

        return allTasks;
    }

    #endregion

    #region ToDoListFunctions

    /// <summary>
    ///     Adds a ToDoList to <seealso cref="RootToDoLists" />
    /// </summary>
    /// <param name="toDoList"> The ToDoList to add </param>
    public static void AddRootToDoList(ToDoList toDoList)
    {
        if (RootToDoLists.Any(rootToDoList => rootToDoList.Name == toDoList.Name))
        {
            _ = MessageBox.Show("You can't add a ToDoList with the same name");
            return;
        }

        RootToDoLists.Add(toDoList);
    }


    /// <summary>
    ///     Adds a ToDoList to another ToDoList
    /// </summary>
    /// <param name="toDoList"> The ToDoList to add </param>
    /// <param name="parentToDoList"> The parent ToDoList </param>
    public static void AddSubToDoList(ToDoList toDoList, ToDoList parentToDoList)
    {
        if (parentToDoList.ToDoLists.Any(childToDoList => childToDoList.Name == toDoList.Name))
        {
            _ = MessageBox.Show("You can't add a ToDoList with the same name");
            return;
        }

        parentToDoList.ToDoLists.Add(toDoList);
    }


    /// <summary>
    ///     Parents a ToDoList to another ToDoList
    /// </summary>
    /// <param name="toDoListToChange"> The ToDoList to change </param>
    /// <param name="newParentToDoList"> The new parent ToDoList </param>
    public static void ChangePathToDoList(ToDoList toDoListToChange, ToDoList newParentToDoList)
    {
        if (newParentToDoList.ToDoLists.Any(childToDoList => childToDoList.Name == toDoListToChange.Name))
        {
            _ = MessageBox.Show("You can't add a ToDoList with the same name");
            return;
        }

        var toDoList = newParentToDoList;

        while (toDoList != null)
        {
            if (toDoList == toDoListToChange)
            {
                _ = MessageBox.Show("You can't add a ToDoList to a child ToDoList");
                return;
            }

            toDoList = GetParentToDoList(toDoList);
        }

        DeleteToDoList(toDoListToChange);
        AddSubToDoList(toDoListToChange, newParentToDoList);
    }


    /// <summary>
    ///     Deletes a ToDoList
    /// </summary>
    /// <param name="toDoListToDelete"> The ToDoList to delete </param>
    public static void DeleteToDoList(ToDoList toDoListToDelete)
    {
        var parentToDoList = GetParentToDoList(toDoListToDelete);

        (parentToDoList == null ? RootToDoLists : parentToDoList.ToDoLists).Remove(toDoListToDelete);
    }


    /// <summary>
    ///     Gets the parent ToDoList of a ToDoList using recursion
    /// </summary>
    /// <param name="toDoList"> The ToDoList to find the parent of </param>
    /// <param name="parentToDoList"> The start of the search, defaults to <seealso cref="RootToDoLists" /> </param>
    /// <returns> The parent ToDoList or null if the ToDoList is a root ToDoList </returns>
    private static ToDoList? GetParentToDoList(ToDoList toDoList, ToDoList? parentToDoList = null)
    {
        ToDoList? foundToDoList = null;

        foreach (var childToDoList in parentToDoList == null ? RootToDoLists : parentToDoList.ToDoLists)
        {
            if (childToDoList == toDoList)
            {
                foundToDoList = parentToDoList;
                break;
            }

            foundToDoList = GetParentToDoList(toDoList, childToDoList);

            if (foundToDoList != null) break;
        }

        return foundToDoList;
    }


    /// <summary>
    ///     Edits a ToDoLists name and image using a new ToDoList
    /// </summary>
    /// <param name="toDoListToBeChanged"> The ToDoList to change </param>
    /// <param name="toDoListInfo"> A ToDoList with the new properties </param>
    public static void EditToDoList(ToDoList toDoListToBeChanged, ToDoList toDoListInfo)
    {
        if (toDoListInfo.ToDoLists.Any(childToDoList => childToDoList.Name == toDoListInfo.Name))
        {
            _ = MessageBox.Show("You can't have a ToDoList with the same name");
            return;
        }

        toDoListToBeChanged.Name = toDoListInfo.Name;
        toDoListToBeChanged.ImagePath = toDoListInfo.ImagePath;
    }


    /// <summary>
    ///     Move the ToDoList up in the tree view
    /// </summary>
    /// <param name="toDoList"> The ToDoList that should be moved </param>
    public static void MoveUpToDoList(ToDoList toDoList)
    {
        var parentToDoList = GetParentToDoList(toDoList);
        var list = parentToDoList == null ? RootToDoLists : parentToDoList.ToDoLists;
        var oldIndex = list.IndexOf(toDoList);
        if (oldIndex != 0) list.Move(oldIndex, oldIndex - 1);
    }


    /// <summary>
    ///     Move the ToDoList down in the tree view
    /// </summary>
    /// <param name="toDoList"> The ToDoList that should be moved </param>
    public static void MoveDownToDoList(ToDoList toDoList)
    {
        var parentToDoList = GetParentToDoList(toDoList);
        var list = parentToDoList == null ? RootToDoLists : parentToDoList.ToDoLists;
        var oldIndex = list.IndexOf(toDoList);
        if (oldIndex != RootToDoLists.Count - 1) list.Move(oldIndex, oldIndex + 1);
    }

    #endregion

    #region TaskFunctions

    /// <summary>
    ///     Add a task to a ToDoList
    /// </summary>
    /// <param name="toDoList"> The ToDoList to add the task to </param>
    /// <param name="task"> The task that is being added </param>
    public static void AddTask(ToDoList toDoList, Task task)
    {
        toDoList.Tasks.Add(task);
    }


    /// <summary>
    ///     Deletes a task from a ToDoList
    /// </summary>
    /// <param name="task"> The task to delete </param>
    public static void DeleteTask(Task task)
    {
        var toDoList = GetToDoListContainingTask(task);

        if (toDoList == null)
        {
            return;
        }

        toDoList.Tasks.Remove(task);
    }


    /// <summary>
    ///     Edits a task by transfering the properties from taskInfo to taskToEdit
    /// </summary>
    /// <param name="taskToEdit"> The task to edit </param>
    /// <param name="taskInfo"> The task containing the information </param>
    public static void EditTask(Task taskToEdit, Task taskInfo)
    {
        taskToEdit.Name = taskInfo.Name;
        taskToEdit.Description = taskInfo.Description;
        taskToEdit.Status = taskInfo.Status;
        taskToEdit.Priority = taskInfo.Priority;
        taskToEdit.Deadline = taskInfo.Deadline;
        taskToEdit.DoneDate = taskInfo.DoneDate;
        taskToEdit.Category = taskInfo.Category;
    }


    /// <summary>
    ///     Finds a task after its name
    /// </summary>
    /// <param name="name"> The name to find the task after </param>
    public static void FindAfterNameTask(string name)
    {
        _ = GetAllTasks().Where(task => task.Name.Contains(name)).ToList();
    }


    /// <summary>
    ///     Finds a task after the deadline
    /// </summary>
    /// <param name="deadline"> The deadline to find the task after </param>
    public static void FindAfterDeadlineTask(DateTime deadline)
    {
        _ = GetAllTasks().Where(task => task.Deadline == deadline).ToList();
    }


    /// <summary>
    ///     Move the task up in the data grid
    /// </summary>
    /// <param name="task"> The task that should be moved </param>
    public static void MoveUpTask(Task task)
    {
        var toDoList = GetToDoListContainingTask(task);

        if (toDoList == null)
        {
            MessageBox.Show("Task not found");
            return;
        }

        var oldIndex = toDoList.Tasks.IndexOf(task);
        if (oldIndex != 0) toDoList.Tasks.Move(oldIndex, oldIndex - 1);
    }


    /// <summary>
    ///     Move the task down in the data grid
    /// </summary>
    /// <param name="task"> The task that should be moved </param>
    public static void MoveDownTask(Task task)
    {
        var toDoList = GetToDoListContainingTask(task);

        if (toDoList == null)
        {
            MessageBox.Show("Task not found");
            return;
        }

        var oldIndex = toDoList.Tasks.IndexOf(task);
        if (oldIndex != toDoList.Tasks.Count - 1) toDoList.Tasks.Move(oldIndex, oldIndex + 1);
    }


    /// <summary>
    ///     Gets the parent <seealso cref="ToDoList" /> of a <seealso cref="Task" /> using recursion
    /// </summary>
    /// <param name="task"> The task to find the parent ToDoList of </param>
    /// <param name="rootToDoList"> The ToDoList the searching starts from, if null it starts from <seealso cref="RootToDoLists" /> </param>
    /// <returns> The parent ToDoList of the task, if it exists </returns>
    public static ToDoList? GetToDoListContainingTask(Task task, ToDoList? rootToDoList = null)
    {
        var list = rootToDoList == null ? RootToDoLists : rootToDoList.ToDoLists;

        var toDoList = list.FirstOrDefault(childToDoList => childToDoList.Tasks.Contains(task));

        if (toDoList != null) return toDoList;

        foreach (var childToDoList in list)
        {
            toDoList = GetToDoListContainingTask(task, childToDoList);

            if (toDoList != null) break;
        }

        return toDoList;
    }


    /// <summary>
    ///     Sets a task with the status of done
    /// </summary>
    /// <param name="task"> The task to set done </param>
    public static void SetDoneTask(Task task)
    {
        task.Status = Status.Done;
    }

    #endregion
}