using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using ProjectMaui.Domain.Models;
using ProjectMaui.Domain.Services;

namespace ProjectMaui.Client.Views;

public partial class AdminCategoriesPage : ContentPage
{
    private readonly CategoryServices _categoryService;

    public AdminCategoriesPage(CategoryServices categoryService)
    {
        InitializeComponent();
        _categoryService = categoryService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadCategories();
    }

    async Task LoadCategories()
    {
        try
        {
            var categories = await _categoryService.GetAllCategories();
            categoriesItemsView.ItemsSource = categories;
            emptyState.IsVisible = categories.Count == 0;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load categories: {ex.Message}", "OK");
        }
    }

    async void OnAddCategoryClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new AdminCategoryFormPage(_categoryService));
    }

    async void OnEditClicked(object? sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is Guid id)
        {
            var categories = await _categoryService.GetAllCategories();
            var category = categories.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                await Navigation.PushAsync(new AdminCategoryFormPage(_categoryService, category));
            }
        }
    }

    async void OnDeleteClicked(object? sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is Guid id)
        {
            var confirm = await DisplayAlert(
                "Confirm Delete",
                "Are you sure you want to delete this category?",
                "Yes",
                "No");

            if (!confirm) return;

            try
            {
                await _categoryService.DeleteCategory(id);
                var toast = Toast.Make("Category deleted successfully", ToastDuration.Short);
                await toast.Show();
                await LoadCategories();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to delete category: {ex.Message}", "OK");
            }
        }
    }

    async void OnBackClicked(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
