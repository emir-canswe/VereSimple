using System.Windows;
using VereSimple.Data;
using VereSimple.Repositories;
using VereSimple.Services;

namespace VereSimple.Views;

public partial class AddTransactionWindow : Window
{
    private readonly int _customerId;
    private readonly string _type;
    private readonly TransactionService _transactionService;

    public AddTransactionWindow(int customerId, string type)
    {
        InitializeComponent();
        _customerId = customerId;
        _type = type;

        var db = new AppDbContext();
        var transactionRepo = new TransactionRepository(db);
        _transactionService = new TransactionService(transactionRepo);

        TxtTitle.Text = type == "DEBT" ? "Borç Ekle" : "Ödeme Al";
        BtnSave.Background = type == "DEBT"
            ? System.Windows.Media.Brushes.Crimson
            : System.Windows.Media.Brushes.SeaGreen;
    }

    private async void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        if (!decimal.TryParse(TxtAmount.Text.Trim().Replace(",", "."),
            System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.InvariantCulture,
            out decimal amount) || amount <= 0)
        {
            TxtError.Text = "Geçerli bir tutar girin.";
            TxtError.Visibility = Visibility.Visible;
            return;
        }

        try
        {
            if (_type == "DEBT")
                await _transactionService.AddDebtAsync(_customerId, amount, TxtDescription.Text.Trim());
            else
                await _transactionService.AddPaymentAsync(_customerId, amount, TxtDescription.Text.Trim());

            this.Close();
        }
        catch (Exception ex)
        {
            TxtError.Text = $"Hata: {ex.Message}";
            TxtError.Visibility = Visibility.Visible;
        }
    }

    private void BtnCancel_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}