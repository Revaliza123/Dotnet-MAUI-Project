using ProjectMaui.Domain.Services;
using ProjectMaui.Domain.Models;
using System.Linq;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace ProjectMaui.Client.Views;

public partial class CustomerMenuPage : ContentPage
{
    private readonly ProductServices _productService;
    private readonly DataSeedService _seeder;
    private readonly CartService _cartService;
    private readonly OrderDetailPage _orderDetailPage;

    private List<Product> _allProducts = new List<Product>();

    public CustomerMenuPage(ProductServices productService, DataSeedService seeder, CartService cartService, OrderDetailPage orderDetailPage)
    {
        InitializeComponent();
        _productService = productService;
        _seeder = seeder;
        _cartService = cartService;
        _cartService.OnCartChanged += UpdateCartBadge;
        _orderDetailPage = orderDetailPage;
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
                _allProducts = products.ToList();
            }

            if (!_allProducts.Any())
            {
                await DisplayAlert("Info", "Database masih kosong setelah seed.", "OK");
                return;
            }

            MenuCollection.ItemsSource = _allProducts;
            CategoryTitle.Text = "Semua Menu 🍽️";
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void OnCategoryClicked(object sender, EventArgs e)
    {
        if (sender is Button btn)
        {
            var layout = btn.Parent as HorizontalStackLayout;
            if (layout != null)
            {
                foreach (var child in layout.Children)
                {
                    if (child is Button button)
                    {
                        button.BackgroundColor = Colors.Transparent;
                        button.TextColor = Color.FromArgb("#1E3A5F");
                    }
                }
            }

            btn.BackgroundColor = Color.FromArgb("#1E3A5F");
            btn.TextColor = Colors.White;

            string category = btn.Text;
            string categoryName = btn.Text;

            CategoryTitle.Text = categoryName switch
            {
                "All Menu" => "Semua Menu 🍽️",
                "Food" => "Makanan 🍛",
                "Drinks" => "Minuman 🍹",
                "Dessert" => "Pencuci Mulut 🍰",
                _ => $"{categoryName} 🍽️"
            };

            if (categoryName == "All Menu")
            {
                MenuCollection.ItemsSource = _allProducts;
            }
            else if (categoryName == "Drinks")
            {
                MenuCollection.ItemsSource = _allProducts.Where(p => p.Type == Product.ProductTypes.Drink).ToList();
            }
            else if (Enum.TryParse(categoryName, out Product.ProductTypes selectedType))
            {
                MenuCollection.ItemsSource = _allProducts.Where(p => p.Type == selectedType).ToList();
            }
        }
    }

    private async void OnCartBottomBarTapped(object sender, TappedEventArgs e)
    {
        if (_cartService.IsEmpty) return;
        await Navigation.PushAsync(_orderDetailPage);
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
        await Navigation.PushAsync(_orderDetailPage);
    }

    private void UpdateCartBadge()
    {
        Device.InvokeOnMainThreadAsync(() =>
        {
            var isEmpty = _cartService.IsEmpty;

            CartBadge.IsVisible = !isEmpty;
            CartBadge.Text = _cartService.TotalQuantity.ToString();
            CartBottomBar.IsVisible = !isEmpty;
            CartItemCountLabel.Text = $"{_cartService.TotalQuantity} item";
            CartTotalPriceLabel.Text = $"Rp {_cartService.TotalPrice:N0}";
        });
    }
}