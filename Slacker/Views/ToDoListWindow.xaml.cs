using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Slacker.Models;

namespace Slacker.Views;

/// <summary>
///     Interaction logic for ToDoListWindow.xaml
/// </summary>
public partial class ToDoListWindow : Window
{
    private readonly string[] _paths;

    private int _selectedImageIndex;

    public ToDoListWindow()
    {
        InitializeComponent();

        _paths = Directory.GetFiles("Assets");

        for (var i = 0; i < _paths.Length; i++) _paths[i] = "/" + _paths[i];

        SelectedImageIndex = 0;
    }

    public int SelectedImageIndex
    {
        get => _selectedImageIndex;
        set
        {
            if (value < 0)
                _selectedImageIndex = _paths.Length - 1;
            else
                _selectedImageIndex = value >= _paths.Length ? 0 : value;

            ToDoListImage.Source = new BitmapImage(new Uri(_paths[_selectedImageIndex], UriKind.Relative));
        }
    }

    public event Action<ToDoList>? SendToDoList;

    private void NextImageButton_Click(object sender, RoutedEventArgs e)
    {
        SelectedImageIndex--;
    }

    private void PreviousImageButton_Click(object sender, RoutedEventArgs e)
    {
        SelectedImageIndex++;
    }

    private void ConfirmButton_Click(object sender, RoutedEventArgs e)
    {
        var name = ToDoListName.Text;
        var image = (ToDoListImage.Source as BitmapImage)?.UriSource.ToString();

        if (image == null) return;

        ToDoList newToDoList = new()
        {
            Name = name,
            ImagePath = image
        };

        SendToDoList?.Invoke(newToDoList);
        Close();
    }
}