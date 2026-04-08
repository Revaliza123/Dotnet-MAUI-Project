
using DotnetMauiProject.Services;

namespace ProjectMaui;

public partial class MainPage : ContentPage
{
	int count = 0;
	private readonly Services.ProductServices _productService;
	public MainPage(Services.ProductServices productServices)
	{
		InitializeComponent();
		_productService = productServices;

		CheckDatabase();
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
	private async void CheckDatabase()
	{
		if (_productService != null)
		{
			var data = await _productService.GetProductsAsync();
			System.Diagnostics.Debug.WriteLine($"{data} ini adalah data products");
		}
	}
}
