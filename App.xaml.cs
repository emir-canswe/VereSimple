using System.Windows;
using VereSimple.Data;
using VereSimple.Models;

namespace VereSimple;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        SeedDatabase();
    }

    private void SeedDatabase()
    {
        using var db = new AppDbContext();
        db.Database.EnsureCreated();

        // Admin kullanıcısı yoksa oluştur
        if (!db.Users.Any())
        {
            db.Users.Add(new User
            {
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Role = "Admin",
                IsActive = true,
                CreatedAt = DateTime.Now
            });
            db.SaveChanges();
        }
    }
}