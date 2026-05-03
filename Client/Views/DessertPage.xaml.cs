using ProjectMaui.Domain.Models;
using ProjectMaui.Domain.Services;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace ProjectMaui.Client.Views;

public partial class DessertPage : ContentPage
{
    private readonly ProductServices _productService;
    private readonly DataSeedService _seeder;
    private readonly CartService _cartService;

    public DessertPage(ProductServices productService, DataSeedService seeder, CartService cartService)
    {
        InitializeComponent();
        _productService = productService;
        _seeder = seeder;
        _cartService = cartService;
        _cartService.OnCartChanged += UpdateCartBadge;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _cartService.OnCartChanged -= UpdateCartBadge;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadProducts();
    }

    private async Task LoadProducts()
    {
        try
        {
            await _seeder.SeedAllData();
            var products = await _productService.GetAllProducts();
            if (products != null)
            {
                var dessertProducts = products.Where(p => p.Type == Product.ProductTypes.Dessert).ToList();
                MenuCollection.ItemsSource = dessertProducts;
                EmptyLabel.IsVisible = !dessertProducts.Any();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void OnCategoryClicked(object sender, EventArgs e)
    {
        if (sender is Button btn)
        {
            if (btn.Text == "All Menu")
            {
                await Navigation.PopAsync();
            }
            else if (btn.Text == "Food")
            {
                await Navigation.PushAsync(new FoodPage(_productService, _seeder, _cartService));
            }
            else if (btn.Text == "Drinks")
            {
                await Navigation.PushAsync(new DrinksPage(_productService, _seeder, _cartService));
            }
        }
    }

    private async void OnAddToCartClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is Product product)
        {
            _cartService.AddItem(product);
            var toast = Toast.Make($"{product.Name} ditambahkan ke keranjang", ToastDuration.Short, 14);
            await toast.Show();
        }
    }

    private async void OnCartIconTapped(object sender, TappedEventArgs e)
    {
        if (_cartService.IsEmpty)
        {
            var toast = Toast.Make("Keranjang masih kosong", ToastDuration.Short, 14);
            await toast.Show();
            return;
        }
        await Navigation.PushAsync(new OrderDetailPage(_cartService));
    }

    private async void OnCartBottomBarTapped(object sender, TappedEventArgs e)
    {
        if (_cartService.IsEmpty)
        {
            var toast = Toast.Make("Keranjang masih kosong", ToastDuration.Short, 14);
            await toast.Show();
            return;
        }
        await Navigation.PushAsync(new OrderDetailPage(_cartService));
    }

    private void UpdateCartBadge()
    {
        Device.InvokeOnMainThreadAsync(() =>
        {
            int qty = _cartService.TotalQuantity;
            bool isEmpty = _cartService.IsEmpty;

            CartBadge.IsVisible = !isEmpty;
            CartBadge.Text = qty.ToString();

            CartBottomBar.IsVisible = !isEmpty;
            CartItemCountLabel.Text = $"{qty} item{(qty > 1 ? "s" : "")}";
            CartTotalPriceLabel.Text = $"Rp {_cartService.TotalPrice:N0}";
        });
    }
}
