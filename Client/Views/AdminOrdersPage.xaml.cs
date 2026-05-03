using ProjectMaui.Domain.Models;
using ProjectMaui.Domain.Services;

namespace ProjectMaui.Client.Views;

public partial class AdminOrdersPage : ContentPage
{
    private readonly OrderService _orderService;

    public AdminOrdersPage(OrderService orderService)
    {
        InitializeComponent();
        _orderService = orderService;
        BindingContext = this;
        LoadOrders();
    }

    private async void LoadOrders()
    {
        try
        {
            var orders = await _orderService.GetOrdersWithItemsAsync();
            OrdersCollection.ItemsSource = orders;
            EmptyState.IsVisible = orders.Count == 0;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Gagal memuat pesanan: {ex.Message}", "OK");
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadOrders();
    }

    private async void OnViewOrderClicked(object? sender, EventArgs e)
    {
        if (sender is Button btn && btn.BindingContext is Order order)
        {
            await Navigation.PushAsync(new OrderDetailViewPage(order, _orderService));
        }
    }

    private async void OnEditOrderClicked(object? sender, EventArgs e)
    {
        if (sender is Button btn && btn.BindingContext is Order order)
        {
            await Navigation.PushAsync(new AdminOrderEditPage(order, _orderService));
        }
    }

private async void OnDeleteOrderClicked(object? sender, EventArgs e)
{
    var confirm = await DisplayActionSheet("Hapus pesanan ini?", "Batal", null, "Hapus");

    if (confirm == "Hapus" && sender is Button btn && btn.BindingContext is Order order)
    {
        await _orderService.DeleteOrderAsync(order);
        await DisplayAlert("Berhasil", "Pesanan berhasil dihapus.", "OK");
        LoadOrders();
    }
}
}
