using ProjectMaui.Domain.Models;
using ProjectMaui.Domain.Services;

namespace ProjectMaui.Client.Views;

public partial class CustomerOrdersPage : ContentPage
{
    private readonly OrderService _orderService;

    public CustomerOrdersPage(OrderService orderService)
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

    private async void OnOrderTapped(object? sender, TappedEventArgs e)
    {
        if (e.Parameter is Order order)
        {
            await Navigation.PushAsync(new OrderDetailViewPage(order, _orderService));
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadOrders();
    }
}
