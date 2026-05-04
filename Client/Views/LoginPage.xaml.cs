using ProjectMaui.Domain.Models;
using ProjectMaui.Domain.Services;

namespace ProjectMaui.Client.Views;

public partial class LoginPage : ContentPage
{
    private readonly UserServices _userServices;

    public LoginPage(UserServices userServices)
    {
        InitializeComponent();
        _userServices = userServices;
    }

    async void OnLoginClicked(object? sender, EventArgs e)
    {
        var username = UsernameEntry.Text?.Trim();
        var password = PasswordEntry.Text?.Trim();

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            ShowError("Username dan password tidak boleh kosong.");
            return;
        }

        SetLoading(true);

        try
        {
            var user = await _userServices.Authenticate(username, password);

            if (user == null)
            {
                ShowError("Username atau password salah.");
                return;
            }

            Page nextPage = user switch
            {
                Employee emp => ResolveAdminPage(),
                Customer cust => ResolveCustomerPage(),
                _ => throw new InvalidOperationException("Unknown user type")
            };

            Application.Current!.MainPage = new NavigationPage(nextPage);
        }
        catch (Exception ex)
        {
            ShowError($"Terjadi kesalahan: {ex.Message}");
        }
        finally
        {
            SetLoading(false);
        }
    }

    private Page ResolveAdminPage()
    {
        return IPlatformApplication.Current!.Services.GetRequiredService<AdminPage>();
    }

    private Page ResolveCustomerPage()
    {
        return IPlatformApplication.Current!.Services.GetRequiredService<CustomerMenuPage>();
    }

    private void ShowError(string message)
    {
        ErrorLabel.Text = message;
        ErrorLabel.IsVisible = true;
    }

    private void SetLoading(bool isLoading)
    {
        LoadingIndicator.IsRunning = isLoading;
        LoadingIndicator.IsVisible = isLoading;
        LoginButton.IsEnabled = !isLoading;
        LoginButton.Text = isLoading ? "Memproses..." : "Masuk";
    }
}