using Microsoft.Maui.Controls.Shapes;
using ProjectMaui.Domain.Models;
using ProjectMaui.Domain.Services;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace ProjectMaui.Client.Views;

public partial class AdminOrderEditPage : ContentPage
{
    private readonly Order _order;
    private readonly OrderService _orderService;

    public AdminOrderEditPage(Order order, OrderService orderService)
    {
        InitializeComponent();
        _order = order;
        _orderService = orderService;
        LoadOrderData();
    }

    private void LoadOrderData()
    {
        OrderIdLabel.Text = $"ID: {_order.Id:N0} | Tanggal: {_order.OrderDate:dd MMMM yyyy}";

        StatusPicker.ItemsSource = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToList();
        StatusPicker.SelectedItem = _order.OrderStatus;

        PaymentStatusPicker.ItemsSource = Enum.GetValues(typeof(PaymentStatus)).Cast<PaymentStatus>().ToList();
        PaymentStatusPicker.SelectedItem = _order.PaymentStatus;

        foreach (var item in _order.OrderItems)
            ItemsContainer.Children.Add(CreateItemCard(item));

        var total = _order.OrderItems.Sum(i => i.SubTotal);
        TotalLabel.Text = $"Rp {total:N0}";
    }

   private Border CreateItemCard(OrderItem item)
    {
        var border = new Border
        {
            StrokeShape = new RoundRectangle { CornerRadius = 12 },
            BackgroundColor = (Color)Color.Parse("#F9F9F9"),
            StrokeThickness = 0
        };
        // ... sisa sama
        var container = new VerticalStackLayout { Padding = 12, Spacing = 4 };

        container.Children.Add(new Label
        {
            Text = $"Item: {item.ProductId}",
            FontSize = 13,
            FontAttributes = FontAttributes.Bold,
            TextColor = (Color)Color.Parse("#1E3A5F")
        });
        container.Children.Add(new Label
        {
            Text = $"Qty: {item.Quantity} x Rp {item.UnitPrice:N0}",
            FontSize = 12,
            TextColor = Colors.Gray
        });
        container.Children.Add(new Label
        {
            Text = $"Status: {item.ItemStatus}",
            FontSize = 12,
            TextColor = (Color)Color.Parse("#FF6B35")
        });
        if (!string.IsNullOrWhiteSpace(item.Note))
        {
            container.Children.Add(new Label
            {
                Text = $"Catatan: {item.Note}",
                FontSize = 11,
                TextColor = (Color)Color.Parse("#555555"),
                FontAttributes = FontAttributes.Italic
            });
        }

        border.Content = container;
        return border;
    }

    private async void OnSaveClicked(object? sender, EventArgs e)
    {
        try
        {
            _order.OrderStatus = (OrderStatus)StatusPicker.SelectedItem!;
            _order.PaymentStatus = (PaymentStatus)PaymentStatusPicker.SelectedItem!;

            await _orderService.UpdateOrderStatusAsync(_order.Id, _order.OrderStatus);

            var toast = Toast.Make("Pesanan berhasil diperbarui!", ToastDuration.Short);
            await toast.Show();

            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Gagal menyimpan: {ex.Message}", "OK");
        }
    }
}
