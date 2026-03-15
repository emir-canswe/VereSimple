using Microsoft.EntityFrameworkCore;
using VereSimple.Models;

namespace VereSimple.Data;

public class AppDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string dbPath = System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "VereSimple",
            "veresimple.db"
        );

        System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(dbPath)!);
        optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.HasIndex(e => e.FullName);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Type).IsRequired().HasMaxLength(10);
            entity.HasOne(e => e.Customer)
                  .WithMany(c => c.Transactions)
                  .HasForeignKey(e => e.CustomerId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(e => e.CustomerId);
            entity.HasIndex(e => e.Date);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.Username).IsUnique();
        });
    }
}