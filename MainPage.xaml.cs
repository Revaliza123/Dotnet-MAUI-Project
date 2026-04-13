
using ProjectMaui.Services;

namespace ProjectMaui;

public partial class MainPage : ContentPage
{
	int count = 0;
	private readonly Services.ProductServices _productService;
	private readonly Services.TableServices _tableService;
	private readonly Services.OrderService _orderService;
	public MainPage(Services.ProductServices productServices)
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
