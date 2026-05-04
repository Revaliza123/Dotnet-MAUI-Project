using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Microsoft.Extensions.DependencyInjection;
using ProjectMaui.Domain.Models;
using ProjectMaui.Domain.Services;

namespace ProjectMaui.Client.Views;

public partial class AdminInventoryPage : ContentPage
{
    private readonly IInventoryManager _inventoryManager;
    private readonly IProductService _productService;
    private readonly IServiceProvider _serviceProvider;

    public AdminInventoryPage(IInventoryManager inventoryManager, IProductService productService, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _inventoryManager = inventoryManager;
        _productService = productService;
        _serviceProvider = serviceProvider;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadInventory();
    }

    async Task LoadInventory()
    {
        try
        {
            var inventories = await _inventoryManager.GetAllInventoryAsync();
            var products = await _productService.GetAllProducts();

            var displayList = inventories.Select(inv =>
            {
                var product = products.FirstOrDefault(p => p.Id == inv.ProductId);
                bool isLowStock = inv.CurrentStock <= inv.MinimumStock;

                return new
                {
                    inv.Id,
                    inv.ProductId,
                    inv.CurrentStock,
                    inv.MinimumStock,
                    inv.LastUpdated,
                    ProductName = product?.Name ?? $"Product ({inv.ProductId})",
                    StockStatus = isLowStock ? "Low Stock" : "In Stock",
                    StockStatusColor = isLowStock ? Colors.Red : Colors.Green
                };
            }).ToList();

            inventoryItemsView.ItemsSource = displayList;
            emptyState.IsVisible = displayList.Count == 0;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load inventory: {ex.Message}", "OK");
        }
    }

    async void OnAddInventoryClicked(object? sender, TappedEventArgs e)
    {
        var formPage = new AdminInventoryFormPage(_serviceProvider);
        await Navigation.PushAsync(formPage);
    }

    async void OnEditClicked(object? sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is Guid id)
        {
            var inventory = await _inventoryManager.GetAllInventoryAsync()
                .ContinueWith(t => t.Result.FirstOrDefault(i => i.Id == id));

            if (inventory != null)
            {
                var products = await _productService.GetAllProducts();
                var formPage = new AdminInventoryFormPage(_serviceProvider, inventory, products);
                await Navigation.PushAsync(formPage);
            }
        }
    }

    async void OnDeleteClicked(object? sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is Guid id)
        {
            var confirmed = await DisplayAlert(
                "Confirm Delete",
                "Are you sure you want to delete this inventory record?",
                "Yes", "No");

            if (confirmed)
            {
                try
                {
                    await _inventoryManager.DeleteInventoryAsync(id);
                    var toast = Toast.Make("Inventory deleted successfully!", ToastDuration.Short);
                    await toast.Show();
                    await LoadInventory();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Failed to delete inventory: {ex.Message}", "OK");
                }
            }
        }
    }
}
