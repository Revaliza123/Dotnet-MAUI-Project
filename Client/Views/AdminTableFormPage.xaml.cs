using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using ProjectMaui.Domain.Models;
using ProjectMaui.Domain.Services;

namespace ProjectMaui.Client.Views;

public partial class AdminTableFormPage : ContentPage
{
    private readonly TableServices _tableService;

    private readonly List<TableStatus> _statusOptions = new()
    {
        TableStatus.Available,
        TableStatus.Reserved,
        TableStatus.Cleaning
    };

    public AdminTableFormPage(TableServices tableService)
    {
        InitializeComponent();
        _tableService = tableService;

        StatusPicker.ItemsSource = _statusOptions.Select(s => s.ToString()).ToList();
        StatusPicker.SelectedIndex = 0; // Default: Available
    }

    async void OnSaveClicked(object? sender, EventArgs e)
    {
        if (!int.TryParse(TableNumberEntry.Text, out int tableNumber) || tableNumber < 1)
        {
            await DisplayAlert("Error", "Nomor meja harus berupa angka dan minimal 1.", "OK");
            return;
        }

        var area = AreaEntry.Text?.Trim();
        if (string.IsNullOrWhiteSpace(area))
        {
            await DisplayAlert("Error", "Area / lokasi tidak boleh kosong.", "OK");
            return;
        }

        if (!int.TryParse(CapacityEntry.Text, out int capacity) || capacity < 2)
        {
            await DisplayAlert("Error", "Kapasitas harus berupa angka dan minimal 2 orang.", "OK");
            return;
        }

        if (StatusPicker.SelectedIndex < 0)
        {
            await DisplayAlert("Error", "Pilih status awal meja.", "OK");
            return;
        }

        var selectedStatus = _statusOptions[StatusPicker.SelectedIndex];

        try
        {
            var table = new Table(tableNumber, area, capacity, selectedStatus);
            await _tableService.AddTable(table);

            var toast = Toast.Make("Meja berhasil ditambahkan!", ToastDuration.Short);
            await toast.Show();

            await Navigation.PopAsync();
        }
        catch (ArgumentException ex)
        {
            await DisplayAlert("Validasi Gagal", ex.Message, "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Gagal menyimpan meja: {ex.Message}", "OK");
        }
    }

    void OnCancelClicked(object? sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
}