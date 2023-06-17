using System.Collections.ObjectModel;

namespace Slacker.Repositories;

public class CategoryRepository
{
    public static ObservableCollection<string> Categories { get; set; } = new()
    {
        "None"
    };

    public static void AddCategory(string category)
    {
        Categories.Add(category);
    }

    public static void ModifyCategory(string categoryToEdit, string categoryInfo)
    {
        _ = Categories.Remove(categoryToEdit);
        Categories.Add(categoryInfo);
    }

    public static void DeleteCategory(string category)
    {
        _ = Categories.Remove(category);
    }
}