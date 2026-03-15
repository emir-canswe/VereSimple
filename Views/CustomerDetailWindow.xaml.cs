using System.Windows;
using VereSimple.Data;
using VereSimple.Repositories;
using VereSimple.Services;

namespace VereSimple.Views;

public class TransactionViewModel
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Type { get; set; } = string.Empty;
    public string TypeDisplay => Type == "DEBT" ? "Borç" : "Ödeme";
    public decimal Amount { get; set; }
    public string? Description { get; set; }
}

public partial class CustomerDetailWindow : Window
{
    private readonly int _customerId;
    private readonly CustomerService _customerService;
    private readonly TransactionService _transactionService;

    public CustomerDetailWindow(int customerId)
    {
        InitializeComponent();
        _customerId = customerId;

        var db = new AppDbContext();
        var customerRepo = new CustomerRepository(db);
        var transactionRepo = new TransactionRepository(db);
        _customerService = new CustomerService(customerRepo, transactionRepo);
        _transactionService = new TransactionService(transactionRepo);

        Loaded += async (s, e) => await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        var customer = await _customerService.GetCustomerByIdAsync(_customerId);
        if (customer == null) return;

        TxtCustomerName.Text = customer.FullName;

        var transactions = await _transactionService.GetCustomerTransactionsAsync(_customerId);

        decimal totalDebt = transactions.Where(t => t.Type == "DEBT").Sum(t => t.Amount);
        decimal totalPayment = transactions.Where(t => t.Type == "PAYMENT").Sum(t => t.Amount);

        TxtTotalDebt.Text = $"{totalDebt:N2} ₺";
        TxtTotalPayment.Text = $"{totalPayment:N2} ₺";
        TxtRemaining.Text = $"{totalDebt - totalPayment:N2} ₺";

        TransactionsGrid.ItemsSource = transactions.Select(t => new TransactionViewModel
        {
            Id = t.Id,
            Date = t.Date,
            Type = t.Type,
            Amount = t.Amount,
            Description = t.Description
        }).ToList();
    }

    private void BtnAddDebt_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new AddTransactionWindow(_customerId, "DEBT");
        dialog.Owner = this;
        dialog.ShowDialog();
        Loaded += async (s, ev) => await LoadDataAsync();
        _ = LoadDataAsync();
    }

    private void BtnAddPayment_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new AddTransactionWindow(_customerId, "PAYMENT");
        dialog.Owner = this;
        dialog.ShowDialog();
        _ = LoadDataAsync();
    }
}