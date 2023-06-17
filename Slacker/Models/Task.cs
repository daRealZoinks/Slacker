using Slacker.Repositories;
using System;
using System.Xml.Serialization;

namespace Slacker.Models;

[Serializable]
public class Task : BaseModel
{
    private string _name;
    private string _description;
    private Status _status;
    private Priority _priority;
    private DateTime _deadline;
    private DateTime _doneDate;
    private string _category;

    public Task()
    {
        _name = string.Empty;
        _description = string.Empty;
        _status = Status.Created;
        _priority = Priority.Low;
        _deadline = DateTime.Now;
        _doneDate = DateTime.MaxValue;
        _category = CategoryRepository.Categories[0];
    }


    /// <summary>
    ///     The name of the task
    /// </summary>
    [XmlElement]
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }


    /// <summary>
    ///     The description of the task
    /// </summary>
    [XmlElement]
    public string Description
    {
        get => _description;
        set
        {
            _description = value;
            OnPropertyChanged();
        }
    }


    /// <summary>
    ///     The status of the task
    /// </summary>
    [XmlElement]
    public Status Status
    {
        get => _status;
        set
        {
            _status = value;
            OnPropertyChanged();
        }
    }


    /// <summary>
    ///     The priority of the task
    /// </summary>
    [XmlElement]
    public Priority Priority
    {
        get => _priority;
        set
        {
            _priority = value;
            OnPropertyChanged();
        }
    }


    /// <summary>
    ///     The deadline of the task
    /// </summary>
    [XmlElement]
    public DateTime Deadline
    {
        get => _deadline;
        set
        {
            _deadline = value;
            OnPropertyChanged();
        }
    }


    /// <summary>
    ///     The date the task was set to <seealso cref="Status.Done" />
    /// </summary>
    [XmlElement]
    public DateTime DoneDate
    {
        get => _doneDate;
        set
        {
            _doneDate = value;
            OnPropertyChanged();
        }
    }


    /// <summary>
    ///     The category of the task. Used for easier search
    /// </summary>
    [XmlElement]
    public string Category
    {
        get => _category;
        set
        {
            _category = value;
            OnPropertyChanged();
        }
    }
}

public enum Status
{
    Created = 0,
    InProgress = 1,
    Done = 2
}

public enum Priority
{
    Low = 0,
    Medium = 1,
    High = 2
}