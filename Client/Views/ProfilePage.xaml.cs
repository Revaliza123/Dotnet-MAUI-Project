namespace ProjectMaui.Client.Views;

public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();
    }

    private async void OnUpdateClicked(object sender, EventArgs e)
    {
        // Simulasi panggil logic yang kita buat di UserServices tadi
        bool isConfirmed = await DisplayAlert("Konfirmasi", "Apakah anda ingin menyimpan perubahan?", "Ya", "Tidak");
        
        if (isConfirmed)
        {
            // Di sini nanti kita panggil method UpdateProfile dari UserServices
            await DisplayAlert("Sukses", "Profil berhasil diperbarui di Database SQLite!", "OK");
        }
    }
}
