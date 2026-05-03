using Microsoft.Maui.Controls.Shapes;
using ProjectMaui.Domain.Models;
using ProjectMaui.Domain.Services;

namespace ProjectMaui.Client.Views;

public partial class OrderDetailViewPage : ContentPage
{
    private readonly Order _order;

    public OrderDetailViewPage(Order order, OrderService orderService)
    {
        InitializeComponent();
        _order = order;
        LoadOrderDetails();
    }

    private void LoadOrderDetails()
    {
        OrderDateLabel.Text = $"Pesanan #{_order.OrderDate:dd MMMM yyyy HH:mm} | Status: {_order.OrderStatus}";

        foreach (var item in _order.OrderItems)
            ItemsContainer.Children.Add(CreateItemCard(item));

        var total = _order.OrderItems.Sum(i => i.SubTotal);
        TotalLabel.Text = $"Rp {total:N0}";
    }

    private Border CreateItemCard(OrderItem item)
    {
        var border = new Border
        {
            StrokeShape = new RoundRectangle { CornerRadius = 16 },
            BackgroundColor = Colors.White,
            StrokeThickness = 0,
            Margin = new Thickness(0, 0, 0, 8)
        };

        var container = new VerticalStackLayout
        {
            Padding = 16,
            Spacing = 6
        };

        container.Children.Add(new Label
        {
            Text = $"Produk: {item.ProductId}",
            FontSize = 14,
            FontAttributes = FontAttributes.Bold,
            TextColor = (Color)Color.Parse("#1E3A5F")
        });

        container.Children.Add(new Label
        {
            Text = $"Jumlah: {item.Quantity} x Rp {item.UnitPrice:N0}",
            FontSize = 13,
            TextColor = Colors.Gray
        });

        if (!string.IsNullOrWhiteSpace(item.Note))
        {
            container.Children.Add(new Label
            {
                Text = $"Catatan: {item.Note}",
                FontSize = 12,
                TextColor = (Color)Color.Parse("#555555"),
                FontAttributes = FontAttributes.Italic
            });
        }

        container.Children.Add(new Label
        {
            Text = $"Subtotal: Rp {item.SubTotal:N0}",
            FontSize = 14,
            FontAttributes = FontAttributes.Bold,
            TextColor = (Color)Color.Parse("#FF6B35")
        });

        border.Content = container;
        return border;
    }
}
