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
            _serviceProvider.GetRequiredService<IProductService>(),
            _serviceProvider);
        await Navigation.PushAsync(productsPage);
    }

    async void OnCategoriesTapped(object? sender, TappedEventArgs e)
    {
        var categoriesPage = new AdminCategoriesPage(
            _serviceProvider.GetRequiredService<CategoryServices>());
        await Navigation.PushAsync(categoriesPage);
    }

    async void OnInventoryTapped(object? sender, TappedEventArgs e)
    {
        var inventoryPage = new AdminInventoryPage(
            _serviceProvider.GetRequiredService<IInventoryManager>(),
            _serviceProvider.GetRequiredService<IProductService>(),
            _serviceProvider);
        await Navigation.PushAsync(inventoryPage);
    }

    async void OnTablesTapped(object? sender, TappedEventArgs e)
    {
        var tablesPage = _serviceProvider.GetRequiredService<AdminTablesPage>();
        await Navigation.PushAsync(tablesPage);
    }
}
