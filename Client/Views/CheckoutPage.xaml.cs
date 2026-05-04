using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Controls;
using ProjectMaui.Domain.Models;
using ProjectMaui.Domain.Services;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace ProjectMaui.Client.Views;

public partial class CheckoutPage : ContentPage
{
    private readonly CartService _cartService;
    private readonly OrderService _orderService;
    private readonly TableServices _tableService;
    private readonly UserServices _userService;

    private Table? _selectedTable;
    private Border? _selectedBorder;

    public CheckoutPage(CartService cartService, OrderService orderService,
        TableServices tableService, UserServices userService)
    {
        InitializeComponent();
        _cartService = cartService;
        _orderService = orderService;
        _tableService = tableService;
        _userService = userService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            await LoadAvailableTables();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Debug Error", ex.ToString(), "OK");
        }
    }

    async Task LoadAvailableTables()
    {
        TablesContainer.Children.Clear();
        _selectedTable = null;
        _selectedBorder = null;
        ConfirmButton.IsEnabled = false;

        OrderTotalLabel.Text = $"Total: Rp {_cartService.TotalPrice:N0}";

        var tables = await _tableService.GetAvailableTablesAsync();

        if (tables.Count == 0)
        {
            var noTablesLabel = new Label
            {
                Text = "Tidak ada meja tersedia saat ini",
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.Gray,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(0, 40)
            };
            TablesContainer.Children.Add(noTablesLabel);
            return;
        }

        foreach (var table in tables)
        {
            TablesContainer.Children.Add(CreateTableCard(table));
        }
    }

    private Border CreateTableCard(Table table)
    {
        var border = new Border
        {
            StrokeShape = new RoundRectangle { CornerRadius = 16 },
            BackgroundColor = Colors.White,
            StrokeThickness = 0
        };

        var grid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Auto }
            },
            ColumnSpacing = 10,
            Padding = 16
        };

        var infoStack = new VerticalStackLayout { Spacing = 4 };

        var titleLabel = new Label
        {
            Text = $"Meja {table.TableNumber}",
            FontSize = 16,
            FontAttributes = FontAttributes.Bold,
            TextColor = (Color)Color.Parse("#1E3A5F")
        };
        infoStack.Children.Add(titleLabel);

        var areaLabel = new Label
        {
            Text = table.Area,
            FontSize = 13,
            TextColor = Colors.Gray
        };
        infoStack.Children.Add(areaLabel);

        var capacityLabel = new Label
        {
            Text = $"Kapasitas: {table.Capacity} orang",
            FontSize = 12,
            TextColor = Colors.Gray
        };
        infoStack.Children.Add(capacityLabel);

        var statusBadge = new Border
        {
            BackgroundColor = Colors.Green,
            StrokeShape = new RoundRectangle { CornerRadius = 12 },
            Padding = new Thickness(10, 4)
        };
        statusBadge.Content = new Label
        {
            Text = "Tersedia",
            FontSize = 11,
            FontAttributes = FontAttributes.Bold,
            TextColor = Colors.White,
            LineHeight = 1
        };
        infoStack.Children.Add(statusBadge);

        var selectBtn = new Button
        {
            Text = "Pilih",
            WidthRequest = 80,
            HeightRequest = 36,
            BackgroundColor = (Color)Color.Parse("#E8F5E9"),
            TextColor = (Color)Color.Parse("#1E3A5F"),
            CornerRadius = 8,
            FontSize = 13,
            FontAttributes = FontAttributes.Bold,
            VerticalOptions = LayoutOptions.Center
        };

        var tableRef = table;
        var borderRef = border;
        selectBtn.Clicked += (s, e) => SelectTable(tableRef, borderRef, selectBtn);

        grid.Add(infoStack, 0, 0);
        grid.Add(selectBtn, 1, 0);
        border.Content = grid;
        return border;
    }

    private void SelectTable(Table table, Border border, Button btn)
    {
        // Deselect previous
        if (_selectedBorder != null && _selectedBorder != border)
        {
            var prevGrid = _selectedBorder.Content as Grid;
            if (prevGrid != null && prevGrid.Children.Count > 1)
            {
                var prevBtn = prevGrid.Children[1] as Button;
                if (prevBtn != null)
                {
                    prevBtn.Text = "Pilih";
                    prevBtn.BackgroundColor = (Color)Color.Parse("#E8F5E9");
                    prevBtn.TextColor = (Color)Color.Parse("#1E3A5F");
                }
            }
        }

        // Select new
        _selectedTable = table;
        _selectedBorder = border;
        btn.Text = "✓";
        btn.BackgroundColor = (Color)Color.Parse("#1E3A5F");
        btn.TextColor = Colors.White;
        ConfirmButton.IsEnabled = true;
    }

    private async void OnConfirmClicked(object? sender, EventArgs e)
    {
        if (_selectedTable == null) return;

        try
        {
            var items = _cartService.Items;
            if (items.Count == 0)
            {
                await DisplayAlert("Error", "Keranjang masih kosong!", "OK");
                return;
            }

            // Get current customer
            var customers = await _userService.GetAllCustomers();
            var customer = customers.FirstOrDefault();

            if (customer == null)
            {
                await DisplayAlert("Error", "User tidak ditemukan. Silakan login terlebih dahulu.", "OK");
                return;
            }

            // Update table status to Occupied
            _selectedTable.SetStatus(TableStatus.Occupied);
            await _tableService.UpdateTable(_selectedTable);

            // Create order
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = customer.Id,
                OrderDate = DateTime.Now,
                TableId = _selectedTable.Id,
                PaymentStatus = PaymentStatus.Pending,
                OrderStatus = OrderStatus.Placed
            };

            // Create order items with customization
            foreach (var cartItem in items)
            {
                var noteStr = new List<string>();
                if (!string.IsNullOrWhiteSpace(cartItem.Note))
                    noteStr.Add(cartItem.Note);
                noteStr.Add($"Gula: {cartItem.SugarLevel}");
                noteStr.Add($"Suhu: {cartItem.ServingTemp}");

                var orderItem = new OrderItem
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    ProductId = cartItem.Product.Id,
                    Quantity = cartItem.Quantity,
                    UnitPrice = cartItem.UnitPrice,
                    Note = string.Join(" | ", noteStr),
                    ItemStatus = ItemStatus.Pending
                };
                order.OrderItems.Add(orderItem);
            }

            // Save to database
            await _orderService.AddOrderAsync(order);

            // Clear cart
            _cartService.Clear();

            var toast = Toast.Make("Pesanan berhasil dipesan!", ToastDuration.Short);
            await toast.Show();

            // Navigate to customer orders page
            await Navigation.PushAsync(new CustomerOrdersPage(_orderService));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Gagal memproses pesanan: {ex.Message}", "OK");
        }
    }
}
