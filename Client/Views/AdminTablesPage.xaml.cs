using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Microsoft.Maui.Controls;
using ProjectMaui.Domain.Models;
using ProjectMaui.Domain.Services;

namespace ProjectMaui.Client.Views;

public partial class AdminTablesPage : ContentPage
{
    private readonly TableServices _tableService;

    public AdminTablesPage(TableServices tableService)
    {
        InitializeComponent();
        _tableService = tableService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadTables();
    }

    async Task LoadTables()
    {
        try
        {
            var tables = await _tableService.GetTableData();
            var displayList = tables.Select(t => new
            {
                t.Id,
                t.TableNumber,
                t.Area,
                t.Capacity,
                t.Status,
                TableNumberText = $"Meja {t.TableNumber}",
                AreaText = t.Area,
                CapacityText = $"Kapasitas: {t.Capacity} orang",
                StatusText = t.Status.ToString(),
                StatusColor = t.Status switch
                {
                    TableStatus.Available => Colors.Green,
                    TableStatus.Occupied => Colors.Red,
                    TableStatus.Reserved => Colors.Orange,
                    TableStatus.Cleaning => Colors.Gray,
                    _ => Colors.Gray
                },
                CanChangeStatus = t.Status != TableStatus.Occupied,
                AvailableBtnColor = t.Status == TableStatus.Available ? Colors.Gray : Colors.Green,
                ReservedBtnColor = t.Status == TableStatus.Reserved ? Colors.Gray : Colors.Orange,
                CleaningBtnColor = t.Status == TableStatus.Cleaning ? Colors.Gray : Colors.Blue
            }).ToList();

            TablesCollectionView.ItemsSource = displayList;
            emptyState.IsVisible = displayList.Count == 0;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Gagal memuat data meja: {ex.Message}", "OK");
        }
    }

    async void OnSetStatusClicked(object? sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is Guid id)
        {
            string? statusText = btn.Text;
            TableStatus newStatus = statusText switch
            {
                "Available" => TableStatus.Available,
                "Reserved" => TableStatus.Reserved,
                "Cleaning" => TableStatus.Cleaning,
                _ => TableStatus.Available
            };

            try
            {
                var table = await _tableService.GetTableByIdAsync(id);
                table.SetStatus(newStatus);
                await _tableService.UpdateTable(table);

                var toast = Toast.Make($"Status meja diubah ke {newStatus}!", ToastDuration.Short);
                await toast.Show();
                await LoadTables();
            }
            catch (InvalidOperationException ex)
            {
                await DisplayAlert("Gagal", ex.Message, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Gagal mengubah status: {ex.Message}", "OK");
            }
        }
    }

    async void OnDeleteClicked(object? sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is Guid id)
        {
            var confirmed = await DisplayAlert(
                "Konfirmasi Hapus",
                "Apakah Anda yakin ingin menghapus meja ini?",
                "Ya", "Batal");

            if (confirmed)
            {
                try
                {
                    await _tableService.DeleteTableAsync(id);
                    var toast = Toast.Make("Meja berhasil dihapus!", ToastDuration.Short);
                    await toast.Show();
                    await LoadTables();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Gagal menghapus meja: {ex.Message}", "OK");
                }
            }
        }
    }
}
