using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Microsoft.Extensions.DependencyInjection;
using ProjectMaui.Domain.Models;
using ProjectMaui.Domain.Services;

namespace ProjectMaui.Client.Views;

public partial class AdminProductsPage : ContentPage
{
    private readonly ProductServices _productService;
    private readonly IServiceProvider _serviceProvider;

    public AdminProductsPage(ProductServices productService, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _productService = productService;
        _serviceProvider = serviceProvider;
        productsItemsView.BindingContext = this;
    }

    public Microsoft.Maui.Controls.BindingBase GoBackCommand =>
        new Microsoft.Maui.Controls.Binding
        {
            Source = new Microsoft.Maui.Controls.Command(async () => await Shell.Current.GoToAsync(".."))
        };

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadProducts();
    }

    async Task LoadProducts()
    {
        try
        {
            var products = await _productService.GetAllProducts();
            productsItemsView.ItemsSource = products;
            emptyState.IsVisible = products.Count == 0;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load products: {ex.Message}", "OK");
        }
    }

    async void OnAddProductClicked(object? sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new AdminProductFormPage(_serviceProvider));
    }

    async void OnEditClicked(object? sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is Guid id)
        {
            var product = await _productService.GetAllProducts()
                .ContinueWith(t => t.Result.FirstOrDefault(p => p.Id == id));
            if (product != null)
            {
                await Navigation.PushAsync(new AdminProductFormPage(_serviceProvider, product));
            }
        }
    }

    async void OnDeleteClicked(object? sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is Guid id)
        {
            var confirm = await DisplayAlert(
                "Confirm Delete",
                "Are you sure you want to delete this product?",
                "Yes",
                "No");

            if (!confirm) return;

            try
            {
                var allProducts = await _productService.GetAllProducts();
                var product = allProducts.FirstOrDefault(p => p.Id == id);
                if (product == null)
                {
                    await DisplayAlert("Error", "Product not found.", "OK");
                    return;
                }

                var type = product.GetType().Name.ToLower() switch
                {
                    "food" => Product.ProductTypes.Food,
                    "dessert" => Product.ProductTypes.Dessert,
                    "drink" => Product.ProductTypes.Drink,
                    _ => throw new InvalidOperationException("Unknown product type")
                };

                await _productService.DeleteProduct(id, type);

                var toast = Toast.Make("Product deleted successfully", CommunityToolkit.Maui.Core.ToastDuration.Short);
                await toast.Show();

                await LoadProducts();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to delete product: {ex.Message}", "OK");
            }
        }
    }
}
