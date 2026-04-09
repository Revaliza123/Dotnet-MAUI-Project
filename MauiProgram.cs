using Microsoft.Extensions.Logging;
using DotnetMauiProject.Services;
using ProjectMaui.Services;

namespace ProjectMaui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<DatabaseService>();
		builder.Services.AddSingleton<ProductServices>();
		builder.Services.AddSingleton<OrderService>();
		builder.Services.AddSingleton<TableServices>();
		builder.Services.AddTransient<MainPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
