using System.ComponentModel;
using System.Runtime.CompilerServices;
using Slacker.Repositories;
using Slacker.Services;

namespace Slacker.Models;

public class BaseModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        FileServices.Saved = false;

        ToDoListRepository.Update();
    }
}