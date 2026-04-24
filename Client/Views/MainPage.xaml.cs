
using ProjectMaui.Domain.Infrasturcture;
using ProjectMaui.Domain.Services;

namespace ProjectMaui.Client.Views;

public partial class MainPage : ContentPage
{
	int count = 0;
	private readonly ProductServices _productService;
	private readonly TableServices _tableService;
	private readonly OrderService _orderService;
	private readonly DataSeedService _seeder;

	public MainPage(ProductServices productServices, TableServices tableServices, OrderService orderService, DataSeedService seeder)
	{
		InitializeComponent();

		_productService = productServices;
		_tableService = tableServices;
		_orderService = orderService;
		_seeder = seeder;

	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		try
		{
			await _seeder.SeedAllData();
			Console.WriteLine("✅ Seeding selesai.");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"❌ SEED ERROR: {ex.GetType().Name} — {ex.Message}");
			Console.WriteLine(ex.StackTrace);
			await DisplayAlert("Seed Error", ex.Message, "OK");
		}
	}

	private void OnCounterClicked(object? sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}
}