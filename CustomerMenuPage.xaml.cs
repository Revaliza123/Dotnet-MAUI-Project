namespace ProjectMaui;

public partial class CustomerMenuPage : ContentPage
{
    public CustomerMenuPage()
    {
        InitializeComponent();
    }

    private async void OnCategoryClicked(object sender, EventArgs e)
    {
        if (sender is Button btn)
        {
            await DisplayAlert("Kategori", $"Kamu memilih kategori: {btn.Text}", "OK");
        }
    }

    private async void OnAddToCartClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Keranjang", "Menu berhasil ditambahkan ke keranjang!", "OK");
    }

    private async void OnCartIconTapped(object sender, TappedEventArgs e)
    {
        await DisplayAlert("Keranjang Pesanan", "Membuka halaman keranjang pesananmu...", "OK");
    }
}