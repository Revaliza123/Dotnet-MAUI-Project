
using ProjectMaui.Domain.Infrasturcture;
using ProjectMaui.Domain.Services;

namespace ProjectMaui.Client.Views;

public partial class MainPage : ContentPage
{
	int count = 0;
	private readonly Domain.Services.ProductServices _productService;
	private readonly Domain.Services.TableServices _tableService;
	private readonly Domain.Services.OrderService _orderService;
	public MainPage(Domain.Services.ProductServices productServices)
	{
		InitializeComponent();
		_productService = productServices;

		var database = new DatabaseService();

		_productService = new ProductServices(database);
		_tableService = new TableServices(database);
		_orderService = new OrderService(database);
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