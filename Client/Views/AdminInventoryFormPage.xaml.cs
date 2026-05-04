using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Microsoft.Extensions.DependencyInjection;
using ProjectMaui.Domain.Models;
using ProjectMaui.Domain.Services;

namespace ProjectMaui.Client.Views;

public partial class AdminInventoryFormPage : ContentPage
{
    private readonly IInventoryManager _inventoryManager;
    private readonly IProductService _productService;
    private Inventory? _editingInventory;
    private List<Product> _products = new();

    public AdminInventoryFormPage(IServiceProvider serviceProvider, Inventory? inventory = null, List<Product>? products = null)
    {
        _inventoryManager = serviceProvider.GetRequiredService<IInventoryManager>();
        _productService = serviceProvider.GetRequiredService<IProductService>();
        _editingInventory = inventory;
        InitializeComponent();

        if (_editingInventory != null && products != null)
        {
            _products = products;
            PopulateForm(_editingInventory);
        }
        else
        {
            LoadProducts();
        }
    }

    async void LoadProducts()
    {
        try
        {
            _products = await _productService.GetAllProducts();
            ProductPicker.ItemsSource = _products.Select(p => p.Name).ToList();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load products: {ex.Message}", "OK");
        }
    }

    void PopulateForm(Inventory inventory)
    {
        var productName = _products.FirstOrDefault(p => p.Id == inventory.ProductId)?.Name;

        ProductPicker.ItemsSource = _products.Select(p => p.Name).ToList();

        if (productName != null && _products.Select(p => p.Name).ToList().Contains(productName))
        {
            ProductPicker.SelectedIndex = _products.Select(p => p.Name).ToList().IndexOf(productName);
        }

        CurrentStockEntry.Text = inventory.CurrentStock.ToString();
        MinimumStockEntry.Text = inventory.MinimumStock.ToString();
    }

    async void OnSaveClicked(object? sender, EventArgs e)
    {
        if (ProductPicker.SelectedIndex < 0)
        {
            await DisplayAlert("Error", "Please select a product.", "OK");
            return;
        }

        if (!int.TryParse(CurrentStockEntry.Text, out int currentStock) || currentStock < 0)
        {
            await DisplayAlert("Error", "Current stock must be a valid number.", "OK");
            return;
        }

        if (!int.TryParse(MinimumStockEntry.Text, out int minimumStock) || minimumStock < 0)
        {
            await DisplayAlert("Error", "Minimum stock must be a valid number.", "OK");
            return;
        }

        try
        {
            if (_editingInventory != null)
            {
                // Update existing record
                _editingInventory.SetCurrentStock(currentStock);
                _editingInventory.SetMinimumStock(minimumStock);
                await _inventoryManager.UpdateInventoryAsync(_editingInventory);

                var toast = Toast.Make("Inventory updated successfully!", ToastDuration.Short);
                await toast.Show();
            }
            else
            {
                // Create new record
                var selectedProduct = _products[ProductPicker.SelectedIndex];
                var inventory = new Inventory(selectedProduct.Id, currentStock, minimumStock);
                await _inventoryManager.AddInventoryAsync(inventory);

                var toast = Toast.Make("Inventory added successfully!", ToastDuration.Short);
                await toast.Show();
            }

            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to save inventory: {ex.Message}", "OK");
        }
    }

    void OnCancelClicked(object? sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
}
