using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Slacker.Models;

[Serializable]
public class ToDoList : BaseModel
{
    private string _imagePath;
    private string _name;

    public ToDoList()
    {
        _name = string.Empty;
        _imagePath = string.Empty;
        ToDoLists = new ObservableCollection<ToDoList>();
        Tasks = new ObservableCollection<Task>();

        ToDoLists.CollectionChanged += (sender, args) => OnPropertyChanged();
        Tasks.CollectionChanged += (sender, args) => OnPropertyChanged();
    }


    /// <summary>
    ///     The name of the list
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
    ///     The path to the icon of the list
    /// </summary>
    [XmlElement]
    public string ImagePath
    {
        get => _imagePath;
        set
        {
            _imagePath = value;
            OnPropertyChanged();
        }
    }


    /// <summary>
    ///     The ToDoList children of the list
    /// </summary>
    [XmlArray]
    public ObservableCollection<ToDoList> ToDoLists { get; set; }


    /// <summary>
    ///     The tasks within this list
    /// </summary>
    [XmlArray]
    public ObservableCollection<Task> Tasks { get; set; }
}