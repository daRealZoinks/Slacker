using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Input;

namespace Slacker.ViewModels;

public class HelpViewModel
{
    public static ICommand AboutCommand => new RelayCommand(() =>
    {
        var nume = "Murarasu Vlad";
        var grupa = "10LF212";
        var email = "vlad.murarasu@student.unitbv.ro";
        _ = MessageBox.Show($"Nume: {nume}\n" +
                            $"Grupa: {grupa}\n" +
                            $"Adresa email institutionala: {email}", "About");
    });
}