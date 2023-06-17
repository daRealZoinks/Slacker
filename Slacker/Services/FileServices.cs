using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Win32;
using Slacker.Models;
using Slacker.Repositories;

namespace Slacker.Services;

/// <summary>
///     The service that handles the file operations
/// </summary>
public class FileServices
{
    private static string _fileName = "NewSlackerList";

    private static bool _saved = true;

    private static string _filePath = "NewSlackerList";

    /// <summary>
    ///     The save state of the ToDoList hierarchy
    /// </summary>
    public static bool Saved
    {
        get => _saved;
        set
        {
            _saved = value;

            var mainWindow = Application.Current.MainWindow;

            if (mainWindow != null) mainWindow.Title = $"{_fileName}{(value ? string.Empty : "*")}";
        }
    }


    /// <summary>
    ///     The current file
    /// </summary>
    public static string FilePath
    {
        get => _filePath;
        set
        {
            _filePath = value;
            _fileName = value.Split('\\')[^1].Split('.')[0];

            var mainWindow = Application.Current.MainWindow;

            if (mainWindow != null) mainWindow.Title = $"{_fileName}{(Saved ? string.Empty : "*")}";
        }
    }


    /// <summary>
    ///     Creates a new empty hierarchy and initializes everything
    /// </summary>
    public static void New()
    {
        ToDoListRepository.RootToDoLists.Clear();
        FilePath = "NewSlackerList";
        Saved = false;
    }


    /// <summary>
    ///     Opens a file and reads an <seealso cref="ObservableCollection<ToDoList>"/> from it
    /// </summary>
    public static void Open()
    {
        OpenFileDialog dialog = new()
        {
            DefaultExt = ".slkr",
            Filter = "Slacker File|*.slkr"
        };
        var result = dialog.ShowDialog();

        if (result == null || !result.Value) return;

        FilePath = dialog.FileName;

        XmlSerializer xmlSerializer = new(typeof(FileContent));

        try
        {
            using (var reader = XmlReader.Create(FilePath))
            {
                var fileContent = xmlSerializer.Deserialize(reader) as FileContent ??
                                  throw new InvalidOperationException("File corrupted");

                ToDoListRepository.RootToDoLists.Clear();

                if (fileContent.ToDoLists == null || fileContent.Categories == null)
                    throw new InvalidOperationException("File corrupted");

                foreach (var item in fileContent.ToDoLists) ToDoListRepository.RootToDoLists.Add(item);

                CategoryRepository.Categories.Clear();

                foreach (var item in fileContent.Categories) CategoryRepository.Categories.Add(item);
            }

            Saved = true;
        }
        catch (Exception e)
        {
            _ = MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }


    /// <summary>
    ///     Saves the hierarchy in <seealso cref="FilePath" />. If the file doesn't exist, it's gonna call
    ///     <seealso cref="SaveAs" /> by default
    /// </summary>
    public static void Save()
    {
        if (!File.Exists(FilePath))
            SaveAs();
        else
            WriteToFile();
    }


    /// <summary>
    ///     Saves the hierarchy in a new file
    /// </summary>
    public static void SaveAs()
    {
        CreateFile();

        WriteToFile();
    }


    /// <summary>
    ///     Creates a new file using the context of <seealso cref="FileServices" />. Should be used only by
    ///     <seealso cref="SaveAs" />
    /// </summary>
    private static void CreateFile()
    {
        SaveFileDialog dialog = new()
        {
            DefaultExt = ".slkr",
            Filter = "Slacker File|*.slkr",
            FileName = $"{_fileName}.slkr"
        };
        var result = dialog.ShowDialog();

        if (result == null || !result.Value) return;

        using (_ = File.Create(dialog.FileName))
        {
            // WriteToFile() throws an exception because the created file is "in use"
            // These curly brackets should make sure that once the file was created, it's not in use anymore
        }

        FilePath = dialog.FileName;
    }


    /// <summary>
    ///     Serializes <seealso cref="ToDoListRepository.RootToDoLists" /> into the current file. Should be used only by one of
    ///     the save functions
    /// </summary>
    private static void WriteToFile()
    {
        XmlSerializer xmlSerializer = new(typeof(FileContent));

        using var writer = XmlWriter.Create(FilePath, new XmlWriterSettings { Indent = true });
        {
            FileContent fileContent = new()
            {
                Categories = CategoryRepository.Categories,
                ToDoLists = ToDoListRepository.RootToDoLists
            };

            xmlSerializer.Serialize(writer, fileContent);
        }

        Saved = true;
    }


    /// <summary>
    ///     The serializable class that contains the hierarchy and the categories
    /// </summary>
    public class FileContent
    {
        /// <summary>
        ///     The ToDoList hierarchy
        /// </summary>
        [XmlArray]
        public ObservableCollection<ToDoList>? ToDoLists { get; set; }


        /// <summary>
        ///     The task categories
        /// </summary>
        [XmlArray]
        public ObservableCollection<string>? Categories { get; set; }
    }
}