using Microsoft.Extensions.Logging;
using ProjectMaui.Client;
using ProjectMaui.Client.Views;
using ProjectMaui.Domain.Infrasturcture;
using ProjectMaui.Domain.Services;
using CommunityToolkit.Maui;

namespace ProjectMaui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        // Register services
        builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddSingleton<DataSeedService>();
        builder.Services.AddSingleton<UserServices>();
        builder.Services.AddSingleton<IProductService, ProductServices>();
        builder.Services.AddSingleton<ProductServices>(sp => (ProductServices)sp.GetRequiredService<IProductService>());
        builder.Services.AddSingleton<CartService>();
        builder.Services.AddSingleton<CategoryServices>();
        builder.Services.AddSingleton<IInventoryManager, InventoryServices>();
        builder.Services.AddSingleton<OrderService>();
        builder.Services.AddSingleton<IPaymentProcessor, PaymentProcessor>();
        builder.Services.AddSingleton<TableServices>();

        // Register pages
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<AdminPage>();
        builder.Services.AddSingleton<OrderDetailPage>();
        builder.Services.AddTransient<CustomerMenuPage>();
        builder.Services.AddTransient<CustomerOrdersPage>();
        builder.Services.AddTransient<AdminProductsPage>();
        builder.Services.AddTransient<AdminProductFormPage>();
        builder.Services.AddTransient<AdminCategoriesPage>();
        builder.Services.AddTransient<AdminCategoryFormPage>();
        builder.Services.AddTransient<AdminOrdersPage>();
        builder.Services.AddTransient<AdminInventoryPage>();
        builder.Services.AddTransient<AdminInventoryFormPage>();
        builder.Services.AddTransient<AdminTablesPage>();
        builder.Services.AddTransient<CheckoutPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();

        // Seed data
        var seeder = app.Services.GetRequiredService<DataSeedService>();
        seeder.SeedAllData();

        return app;
    }
}
