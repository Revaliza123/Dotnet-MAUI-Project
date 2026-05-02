using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using ProjectMaui.Domain.Models;
using ProjectMaui.Domain.Services;

namespace ProjectMaui.Client.Views;

public partial class AdminCategoryFormPage : ContentPage
{
    private readonly CategoryServices _categoryService;
    private Category? _editingCategory;

    public AdminCategoryFormPage(CategoryServices categoryService, Category? category = null)
    {
        InitializeComponent();
        _categoryService = categoryService;
        _editingCategory = category;

        if (_editingCategory != null)
        {
            NameEntry.Text = _editingCategory.Name;
            DescriptionEditor.Text = _editingCategory.Description;
            SaveButton.Text = "Update";
        }
    }

    async void OnCancelClicked(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    async void OnSaveClicked(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameEntry.Text))
        {
            await DisplayAlert("Validation Error", "Category name is required.", "OK");
            return;
        }

        try
        {
            if (_editingCategory == null)
            {
                var category = new Category(NameEntry.Text!, DescriptionEditor.Text ?? "");
                await _categoryService.AddCategory(category);
                var toast = Toast.Make("Category added successfully!", ToastDuration.Short);
                await toast.Show();
            }
            else
            {
                _editingCategory.Name = NameEntry.Text!;
                _editingCategory.Description = DescriptionEditor.Text ?? "";
                await _categoryService.UpdateCategory(_editingCategory);
                var toast = Toast.Make("Category updated successfully!", ToastDuration.Short);
                await toast.Show();
            }

            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to save category: {ex.Message}", "OK");
        }
    }
}
