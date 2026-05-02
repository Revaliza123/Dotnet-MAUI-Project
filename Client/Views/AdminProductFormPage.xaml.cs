using Microsoft.Maui.Controls;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Microsoft.Maui.Media;
using ProjectMaui.Domain.Models;
using ProjectMaui.Domain.Services;

namespace ProjectMaui.Client.Views;

public partial class AdminProductFormPage : ContentPage
{
    private readonly ProductServices _productService;
    private Product? _editingProduct;
    private FileResult? _selectedImage;

    public AdminProductFormPage(IServiceProvider serviceProvider, Product? product = null)
    {
        _productService = serviceProvider.GetRequiredService<ProductServices>();
        _editingProduct = product;
        InitializeComponent();

        if (_editingProduct != null)
        {
            PopulateForm(_editingProduct);
        }
    }

    void PopulateForm(Product product)
    {
        var typeName = product.GetType().Name;
        var typeIndex = typeName.ToLower() switch
        {
            "food" => 0,
            "dessert" => 1,
            "drink" => 2,
            _ => 0
        };
        TypePicker.SelectedIndex = typeIndex;

        NameEntry.Text = product.Name;
        DescriptionEditor.Text = product.Description;
        PriceEntry.Text = product.Price.ToString("F2");
        StockEntry.Text = product.StockQuantity.ToString();
        IngredientsEntry.Text = product.Ingredients;
        PrepTimeEntry.Text = ((int)product.PreparationTime.TotalMinutes).ToString();

        StatusPicker.SelectedIndex = product.Status switch
        {
            ProductStatus.Available => 0,
            ProductStatus.OutOfStock => 1,
            ProductStatus.Discontinued => 2,
            _ => 0
        };

        if (!string.IsNullOrEmpty(product.Image))
        {
            ImagePathEntry.Text = product.Image;
            PreviewImage.Source = product.Image;
        }

        SaveButton.Text = "Update";
    }

    async void OnPickImageClicked(object? sender, EventArgs e)
    {
        try
        {
            var result = await MediaPicker.PickPhotoAsync();
            if (result != null)
            {
                _selectedImage = result;
                ImagePathEntry.Text = result.FileName;
                PreviewImage.Source = result.FullPath;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to pick image: {ex.Message}", "OK");
        }
    }

    // PERBAIKAN: Menggunakan EventArgs, bukan SelectedIndexChangedEventArgs
    void OnTypeSelected(object? sender, EventArgs e)
    {
        // Jika butuh mengambil index:
        // var picker = (Picker)sender!;
        // int selectedIndex = picker.SelectedIndex;
    }

    async void OnCancelClicked(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    async void OnSaveClicked(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameEntry.Text))
        {
            await DisplayAlert("Validation Error", "Product name is required.", "OK");
            return;
        }

        if (!decimal.TryParse(PriceEntry.Text, out decimal price) || price <= 0)
        {
            await DisplayAlert("Validation Error", "Valid price is required.", "OK");
            return;
        }

        if (!int.TryParse(StockEntry.Text, out int stock) || stock < 0)
        {
            await DisplayAlert("Validation Error", "Valid stock quantity is required.", "OK");
            return;
        }

        if (!int.TryParse(PrepTimeEntry.Text, out int prepMinutes))
        {
            await DisplayAlert("Validation Error", "Valid preparation time is required.", "OK");
            return;
        }

        var status = StatusPicker.SelectedIndex switch
        {
            1 => ProductStatus.OutOfStock,
            2 => ProductStatus.Discontinued,
            _ => ProductStatus.Available
        };

        try
        {
            if (_editingProduct == null)
            {
                var type = TypePicker.SelectedIndex switch
                {
                    1 => Product.ProductTypes.Dessert,
                    2 => Product.ProductTypes.Drink,
                    _ => Product.ProductTypes.Food
                };

                Product product = type switch
                {
                    Product.ProductTypes.Dessert => new Dessert(
                        NameEntry.Text!,
                        DescriptionEditor.Text ?? "",
                        price,
                        ImagePathEntry.Text ?? "",
                        IngredientsEntry.Text ?? "",
                        stock,
                        TimeSpan.FromMinutes(prepMinutes),
                        status,
                        "Sweet",              // taste
                        "N/A",                // nutritionInfo
                        0,                    // sweetness
                        ServingTemp.Cold      // temp
                    ),
                    Product.ProductTypes.Drink => new Drink(
                        NameEntry.Text!,
                        DescriptionEditor.Text ?? "",
                        price,
                        ImagePathEntry.Text ?? "",
                        IngredientsEntry.Text ?? "",
                        stock,
                        TimeSpan.FromMinutes(prepMinutes),
                        status,
                        SugarLevel.Normal,    // sugar
                        false                 // caffeinated
                    ),
                    _ => new Food(
                        NameEntry.Text!,
                        DescriptionEditor.Text ?? "",
                        price,
                        ImagePathEntry.Text ?? "",
                        IngredientsEntry.Text ?? "",
                        stock,
                        TimeSpan.FromMinutes(prepMinutes),
                        status,
                        "Savory",             // taste
                        "Standard"            // nutritionInfo
                    )
                };

                await _productService.AddProduct(product, type, _selectedImage);
                var toast = Toast.Make("Product added successfully!", ToastDuration.Short);
                await toast.Show();
            }
            else
            {
                _editingProduct.Name = NameEntry.Text!;
                _editingProduct.Description = DescriptionEditor.Text ?? "";
                _editingProduct.Price = price;
                _editingProduct.StockQuantity = stock;
                _editingProduct.Ingredients = IngredientsEntry.Text ?? "";
                _editingProduct.PreparationTime = TimeSpan.FromMinutes(prepMinutes);
                _editingProduct.Status = status;

                var type = _editingProduct.GetType().Name.ToLower() switch
                {
                    "food" => Product.ProductTypes.Food,
                    "dessert" => Product.ProductTypes.Dessert,
                    "drink" => Product.ProductTypes.Drink,
                    _ => Product.ProductTypes.Food
                };

                await _productService.UpdateProduct(_editingProduct, type, _selectedImage);
                var toast = Toast.Make("Product updated successfully!", ToastDuration.Short);
                await toast.Show();
            }

            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to save product: {ex.Message}", "OK");
        }
    }
}