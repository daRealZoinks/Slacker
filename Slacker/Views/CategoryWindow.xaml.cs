using Slacker.Repositories;
using System.Windows;
using System.Windows.Controls;

namespace Slacker.Views;

/// <summary>
///     Interaction logic for CategoryWindow.xaml
/// </summary>
public partial class CategoryWindow : Window
{
    public CategoryWindow()
    {
        InitializeComponent();
    }

    private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
    {
        if (CategoryNameTextBox.Text != string.Empty)
        {
            CategoryRepository.AddCategory(CategoryNameTextBox.Text);
            CategoryNameTextBox.Text = string.Empty;
        }
    }

    private void EditCategoryButton_Click(object sender, RoutedEventArgs e)
    {
        if (CategoryNameListBox.SelectedItem != null)
        {
            CategoryRepository.ModifyCategory(CategoryNameListBox.SelectedItem.ToString(), CategoryNameTextBox.Text);
        }
    }

    private void DeleteCategoryButton_Click(object sender, RoutedEventArgs e)
    {
        if (CategoryNameListBox.SelectedItem != null)
        {
            CategoryRepository.DeleteCategory(CategoryNameListBox.SelectedItem.ToString());
            CategoryNameTextBox.Text = string.Empty;
        }
    }

    private void CategoryNameListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        CategoryNameTextBox.Text = CategoryNameListBox.SelectedItem?.ToString();
    }
}