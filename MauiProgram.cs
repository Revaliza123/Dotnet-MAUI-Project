using Microsoft.Extensions.Logging;
using ProjectMaui.Domain.Infrasturcture;
using ProjectMaui.Domain.Services;
using ProjectMaui.Client.Views;
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
						fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
					});

		builder.Services.AddSingleton<DatabaseService>();
		builder.Services.AddSingleton<ProductServices>();
		builder.Services.AddSingleton<OrderService>();
		builder.Services.AddSingleton<TableServices>();
		builder.Services.AddSingleton<UserServices>();
		builder.Services.AddSingleton<InventoryServices>();
		builder.Services.AddSingleton<CategoryServices>();
		builder.Services.AddSingleton<DataSeedService>();

		builder.Services.AddTransient<CustomerMenuPage>();
		builder.Services.AddTransient<FoodPage>();
		builder.Services.AddTransient<DrinksPage>();
		builder.Services.AddTransient<DessertPage>();
		builder.Services.AddTransient<MainPage>();

		builder.Services.AddTransient<AdminPage>();
		builder.Services.AddTransient<AdminProductsPage>();
		builder.Services.AddTransient<AdminProductFormPage>();
		builder.Services.AddTransient<AdminCategoriesPage>();
		builder.Services.AddTransient<AdminCategoryFormPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
