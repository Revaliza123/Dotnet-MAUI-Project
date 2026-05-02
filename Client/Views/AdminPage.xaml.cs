using Microsoft.Extensions.DependencyInjection;
using ProjectMaui.Domain.Services;

namespace ProjectMaui.Client.Views;

public partial class AdminPage : ContentPage
{
    private readonly IServiceProvider _serviceProvider;

    public AdminPage(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
        BackgroundColor = Colors.WhiteSmoke;
    }

    async void OnProductsTapped(object? sender, TappedEventArgs e)
    {
        var productsPage = new AdminProductsPage(
            _serviceProvider.GetRequiredService<ProductServices>(),
            _serviceProvider);
        await Navigation.PushAsync(productsPage);
    }

    async void OnCategoriesTapped(object? sender, TappedEventArgs e)
    {
        var categoriesPage = new AdminCategoriesPage(
            _serviceProvider.GetRequiredService<CategoryServices>());
        await Navigation.PushAsync(categoriesPage);
    }
}
