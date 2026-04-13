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
            if (btn.Text == "Food")
            {
                await Navigation.PushAsync(new FoodPage());
            }
            else if (btn.Text == "Drinks")
            {
                await Navigation.PushAsync(new DrinksPage());
            }
            else if (btn.Text == "Dessert")
            {
                await Navigation.PushAsync(new DessertPage());
            }
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