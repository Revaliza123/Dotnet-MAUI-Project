using Microsoft.Extensions.DependencyInjection;
using ProjectMaui.Domain.Services;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls.Shapes;

namespace ProjectMaui.Client.Views;

public class AdminPage : ContentPage
{
    private readonly IServiceProvider _serviceProvider;

    public AdminPage(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        
        Background = new LinearGradientBrush
        {
            StartPoint = new Point(0, 0.5),
            EndPoint = new Point(1, 0.5),
            GradientStops =
            {
                new GradientStop { Offset = 0.0f, Color = Color.FromArgb("#2D0000") },
                new GradientStop { Offset = 1.0f, Color = Color.FromArgb("#5c1a01") }
            }
        };
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Title = "Admin Dashboard";

        var grid = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Star },
            },
            Margin = new Thickness(16, 40, 16, 16),
        };

        var titleLabel = new Label
        {
            Text = "⚙️  Admin Dashboard",
            FontSize = 20,
            FontAttributes = FontAttributes.Bold,
            TextColor = Colors.White,
            VerticalOptions = LayoutOptions.Center,
            Margin = new Thickness(0, 0, 0, 16),
        };
        grid.Add(titleLabel, 0, 0);

        var scroll = new ScrollView();
        var itemsLayout = new VerticalStackLayout
        {
            Spacing = 16,
            Padding = new Thickness(0, 0, 0, 16),
        };
        scroll.Content = itemsLayout;
        grid.Add(scroll, 0, 1);

        itemsLayout.Add(CreateMenuCard("📦", "Products", "Manage all products", async () =>
        {
            var productPage = new AdminProductsPage(
                _serviceProvider.GetRequiredService<ProductServices>(),
                _serviceProvider);
            await Navigation.PushAsync(productPage);
        }));

        itemsLayout.Add(CreateMenuCard("📂", "Categories", "Manage categories", async () =>
        {
            var categoryPage = new AdminCategoriesPage(
                _serviceProvider.GetRequiredService<CategoryServices>());
            await Navigation.PushAsync(categoryPage);
        }));

        itemsLayout.Add(CreateMenuCard("📊", "Inventory", "Manage inventory", () =>
            DisplayAlert("Coming Soon", "Inventory management will be available soon.", "OK")));

        itemsLayout.Add(CreateMenuCard("👥", "Users", "Manage users", () =>
            DisplayAlert("Coming Soon", "User management will be available soon.", "OK")));

        Content = grid;
    }

    IView CreateMenuCard(string icon, string title, string description, Action action)
    {
        var border = new Border
        {
            StrokeShape = new RoundRectangle { CornerRadius = 15 },
            Background = Color.FromArgb("#4a0e05"),
            Stroke = Colors.Transparent,
            Padding = new Thickness(16),
        };

        var grid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = 60 },
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = 50 },
            }
        };

        var iconLabel = new Label
        {
            Text = icon,
            FontSize = 32,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
        };

        var textStack = new VerticalStackLayout { Spacing = 4 };
        textStack.Add(new Label
        {
            Text = title,
            FontSize = 18,
            FontAttributes = FontAttributes.Bold,
            TextColor = Colors.White,
        });
        textStack.Add(new Label
        {
            Text = description,
            FontSize = 12,
            TextColor = Color.FromArgb("#BDBDBD"),
        });

        var arrowLabel = new Label
        {
            Text = "➡️",
            FontSize = 20,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
        };

        grid.Add(iconLabel, 0, 0);
        grid.Add(textStack, 1, 0);
        grid.Add(arrowLabel, 2, 0);

        border.Content = grid;

        var tap = new TapGestureRecognizer
        {
            Command = new Command(() => action.Invoke())
        };
        border.GestureRecognizers.Add(tap);

        return border;
    }
}