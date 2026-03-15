using System.Windows;
using VereSimple.Data;

namespace VereSimple.Views;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
        TxtUsername.Focus();
    }

    private void BtnLogin_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            string username = TxtUsername.Text.Trim();
            string password = TxtPassword.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                TxtError.Text = "Kullanıcı adı ve şifre boş olamaz.";
                TxtError.Visibility = Visibility.Visible;
                return;
            }

            using var db = new AppDbContext();

            // Önce kullanıcı var mı kontrol et
            var allUsers = db.Users.ToList();
            if (!allUsers.Any())
            {
                TxtError.Text = "Veritabanında hiç kullanıcı yok!";
                TxtError.Visibility = Visibility.Visible;
                return;
            }

            var user = allUsers.FirstOrDefault(u => u.Username == username && u.IsActive);

            if (user == null)
            {
                TxtError.Text = $"'{username}' kullanıcısı bulunamadı.";
                TxtError.Visibility = Visibility.Visible;
                return;
            }

            bool passwordOk = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (!passwordOk)
            {
                TxtError.Text = "Şifre hatalı.";
                TxtError.Visibility = Visibility.Visible;
                return;
            }

            var dashboard = new DashboardWindow(user);
            dashboard.Show();
            this.Close();
        }
        catch (Exception ex)
        {
            TxtError.Text = $"Hata: {ex.Message}";
            TxtError.Visibility = Visibility.Visible;
        }
    }
}