using Microsoft.Maui.Controls.Shapes;
using ProjectMaui.Domain.DTOs;
using ProjectMaui.Domain.Models;
using ProjectMaui.Domain.Services;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace ProjectMaui.Client.Views;

public partial class OrderDetailPage : ContentPage
{
    private readonly CartService _cartService;
    private readonly OrderService _orderService;
    private readonly UserServices _userService;

    public OrderDetailPage(CartService cartService, OrderService orderService, UserServices userService)
    {
        InitializeComponent();
        _cartService = cartService;
        _orderService = orderService;
        _userService = userService;
        LoadCartItems();
    }

    private void LoadCartItems()
    {
        ItemsContainer.Children.Clear();
        var items = _cartService.Items;

        if (items.Count == 0)
        {
            var emptyStack = new VerticalStackLayout
            {
                Spacing = 10,
                HorizontalOptions = LayoutOptions.Center,
                Padding = new Thickness(0, 40)
            };

            emptyStack.Children.Add(new Label
            {
                Text = "🛒",
                FontSize = 48,
                HorizontalOptions = LayoutOptions.Center
            });
            emptyStack.Children.Add(new Label
            {
                Text = "Keranjang masih kosong",
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.Gray,
                HorizontalOptions = LayoutOptions.Center
            });
            emptyStack.Children.Add(new Label
            {
                Text = "Tambahkan produk dari menu",
                FontSize = 13,
                TextColor = Colors.Gray,
                HorizontalOptions = LayoutOptions.Center
            });

            ItemsContainer.Children.Add(emptyStack);
        }
        else
        {
            foreach (var cartItem in items)
            {
                ItemsContainer.Children.Add(CreateItemCard(cartItem));
            }
        }

        UpdateSummary();
    }

    private Border CreateItemCard(CartItem cartItem)
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
            Spacing = 10
        };

        // Product info row
        var infoGrid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Auto }
            },
            ColumnSpacing = 10
        };

        var infoStack = new VerticalStackLayout { Spacing = 2 };
        infoStack.Children.Add(new Label
        {
            Text = cartItem.Product.Name,
            FontSize = 16,
            FontAttributes = FontAttributes.Bold,
            TextColor = (Color)Color.Parse("#1E3A5F")
        });
        infoStack.Children.Add(new Label
        {
            Text = cartItem.Product.Description,
            FontSize = 12,
            TextColor = Colors.Gray,
            MaxLines = 2,
            LineBreakMode = LineBreakMode.TailTruncation
        });
        infoStack.Children.Add(new Label
        {
            Text = $"Rp {cartItem.UnitPrice:N0}",
            FontSize = 14,
            FontAttributes = FontAttributes.Bold,
            TextColor = (Color)Color.Parse("#FF6B35")
        });
        infoGrid.Add(infoStack, 0, 0);

        // Quantity controls
        var qtyStack = new HorizontalStackLayout
        {
            Spacing = 8,
            VerticalOptions = LayoutOptions.Center
        };

        var minusBtn = new Button
        {
            Text = "-",
            WidthRequest = 28,
            HeightRequest = 28,
            BackgroundColor = (Color)Color.Parse("#F0F0F0"),
            TextColor = (Color)Color.Parse("#333333"),
            FontSize = 14,
            CornerRadius = 14
        };
        minusBtn.Clicked += (s, e) =>
        {
            cartItem.Quantity--;
            if (cartItem.Quantity <= 0)
            {
                _cartService.RemoveItem(cartItem.Product.Id);
                LoadCartItems();
            }
            else
            {
                LoadCartItems();
            }
        };

        var qtyLabel = new Label
        {
            Text = cartItem.Quantity.ToString(),
            FontSize = 16,
            FontAttributes = FontAttributes.Bold,
            TextColor = (Color)Color.Parse("#1E3A5F"),
            VerticalOptions = LayoutOptions.Center
        };

        var plusBtn = new Button
        {
            Text = "+",
            WidthRequest = 28,
            HeightRequest = 28,
            BackgroundColor = (Color)Color.Parse("#1E3A5F"),
            TextColor = Colors.White,
            FontSize = 14,
            CornerRadius = 14
        };
        plusBtn.Clicked += (s, e) =>
        {
            cartItem.Quantity++;
            LoadCartItems();
        };

        qtyStack.Children.Add(minusBtn);
        qtyStack.Children.Add(qtyLabel);
        qtyStack.Children.Add(plusBtn);
        infoGrid.Add(qtyStack, 1, 0);
        container.Children.Add(infoGrid);

        // Customization section
        var customStack = new VerticalStackLayout { Spacing = 8 };

        // Sugar Level
        var sugarGrid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Auto },
                new ColumnDefinition { Width = GridLength.Star }
            },
            ColumnSpacing = 10
        };
        sugarGrid.Add(new Label
        {
            Text = "Gula:",
            FontSize = 13,
            TextColor = (Color)Color.Parse("#555555"),
            VerticalOptions = LayoutOptions.Center
        }, 0, 0);

        var sugarPicker = new Picker
        {
            Title = "Pilih tingkat gula",
            FontSize = 13,
            BackgroundColor = (Color)Color.Parse("#F9F9F9")
        };
        sugarPicker.ItemsSource = new[] { "Tidak", "Sedikit", "Normal", "Banyak" };
        sugarPicker.SelectedItem = cartItem.SugarLevel.ToString();
        sugarPicker.SelectedIndexChanged += (s, e) =>
        {
            if (Enum.TryParse<SugarLevel>(sugarPicker.SelectedItem?.ToString(), out var level))
                cartItem.SugarLevel = level;
        };
        sugarGrid.Add(sugarPicker, 1, 0);
        customStack.Children.Add(sugarGrid);

        // Serving Temp
        var tempGrid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Auto },
                new ColumnDefinition { Width = GridLength.Star }
            },
            ColumnSpacing = 10
        };
        tempGrid.Add(new Label
        {
            Text = "Suhu:",
            FontSize = 13,
            TextColor = (Color)Color.Parse("#555555"),
            VerticalOptions = LayoutOptions.Center
        }, 0, 0);

        var tempPicker = new Picker
        {
            Title = "Pilih suhu",
            FontSize = 13,
            BackgroundColor = (Color)Color.Parse("#F9F9F9")
        };
        tempPicker.ItemsSource = new[] { "Dingin", "Hangat", "Panas" };
        tempPicker.SelectedItem = cartItem.ServingTemp.ToString();
        tempPicker.SelectedIndexChanged += (s, e) =>
        {
            if (Enum.TryParse<ServingTemp>(tempPicker.SelectedItem?.ToString(), out var temp))
                cartItem.ServingTemp = temp;
        };
        tempGrid.Add(tempPicker, 1, 0);
        customStack.Children.Add(tempGrid);

        // Notes entry
        var noteEntry = new Entry
        {
            Placeholder = "Catatan tambahan (opsional)",
            FontSize = 13,
            BackgroundColor = (Color)Color.Parse("#F9F9F9")
        };
        noteEntry.Text = cartItem.Note;
        noteEntry.TextChanged += (s, e) =>
        {
            cartItem.Note = noteEntry.Text;
        };
        customStack.Children.Add(noteEntry);

        container.Children.Add(customStack);

        // Remove button and subtotal
        var bottomGrid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Auto }
            }
        };

        var removeBtn = new Button
        {
            Text = "Hapus Item",
            BackgroundColor = Colors.Transparent,
            TextColor = (Color)Color.Parse("#E74C3C"),
            FontSize = 13,
            HeightRequest = 30
        };
        removeBtn.Clicked += (s, e) =>
        {
            _cartService.RemoveItem(cartItem.Product.Id);
            LoadCartItems();
        };
        bottomGrid.Add(removeBtn, 0, 0);

        bottomGrid.Add(new Label
        {
            Text = $"Subtotal: Rp {cartItem.SubTotal:N0}",
            FontSize = 14,
            FontAttributes = FontAttributes.Bold,
            TextColor = (Color)Color.Parse("#1E3A5F"),
            VerticalOptions = LayoutOptions.Center
        }, 1, 0);

        container.Children.Add(bottomGrid);
        border.Content = container;
        return border;
    }

    private void UpdateSummary()
    {
        var items = _cartService.Items;
        ItemCountLabel.Text = items.Count.ToString();
        TotalPriceLabel.Text = $"Rp {_cartService.TotalPrice:N0}";
        OrderTotalLabel.Text = $"Rp {_cartService.TotalPrice:N0}";
        OrderButton.IsEnabled = items.Count > 0;
    }

    private async void OnPlaceOrderClicked(object? sender, EventArgs e)
    {
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

            // Create order
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = customer.Id,
                OrderDate = DateTime.Now,
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
