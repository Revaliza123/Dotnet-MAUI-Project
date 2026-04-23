namespace ProjectMaui.Client.Views;

using ProjectMaui.Domain.Services;
using ProjectMaui.Domain.Models;

public partial class CustomerMenuPage : ContentPage
{
    private readonly ProductServices _productService;

    public CustomerMenuPage(ProductServices productService)
    {
        InitializeComponent();
        _productService = productService;
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
            var products = await _productService.GetAllProducts();
            Console.WriteLine($"DEBUG: Jumlah produk ditemukan: {products?.Count ?? 0}"); // Cek di Output Window

            if (products == null || products.Count == 0)
            {
                await DisplayAlert("Info", "Database kosong, pastikan Seeder jalan.", "OK");
            }

            MenuCollection.ItemsSource = products;
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
            await DisplayAlert("Filter", $"Mencari kategori: {btn.Text}", "OK");
        }
    }

    private async void OnAddToCartClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is Product product)
        {
            await DisplayAlert("Keranjang", $"{product.Name} berhasil ditambahkan!", "OK");
        }
    }

    private async void OnCartIconTapped(object sender, TappedEventArgs e)
    {
        await DisplayAlert("Keranjang Pesanan", "Membuka halaman keranjang...", "OK");
    }
}