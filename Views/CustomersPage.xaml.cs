using System.Windows;
using System.Windows.Controls;
using VereSimple.Data;
using VereSimple.Models;
using VereSimple.Services;
using VereSimple.Repositories;

namespace VereSimple.Views;

public class CustomerViewModel
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal RemainingDebt { get; set; }
}

public partial class CustomersPage : Page
{
    private readonly CustomerService _customerService;

    public CustomersPage()
    {
        InitializeComponent();
        var db = new AppDbContext();
        var customerRepo = new CustomerRepository(db);
        var transactionRepo = new TransactionRepository(db);
        _customerService = new CustomerService(customerRepo, transactionRepo);
        LoadCustomers();
    }

    private async void LoadCustomers(string search = "")
    {
        var customers = string.IsNullOrEmpty(search)
            ? await _customerService.GetAllCustomersAsync()
            : await _customerService.SearchCustomersAsync(search);

        var list = new List<CustomerViewModel>();
        foreach (var c in customers)
        {
            var debt = await _customerService.GetRemainingDebtAsync(c.Id);
            list.Add(new CustomerViewModel
            {
                Id = c.Id,
                FullName = c.FullName,
                Phone = c.Phone,
                CreatedAt = c.CreatedAt,
                RemainingDebt = debt
            });
        }

        CustomersGrid.ItemsSource = list;
    }

    private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
    {
        LoadCustomers(TxtSearch.Text.Trim());
    }

    private void BtnAddCustomer_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new AddCustomerWindow();
        dialog.ShowDialog();
        LoadCustomers();
    }

    private void BtnDetail_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is int id)
        {
            var detail = new CustomerDetailWindow(id);
            detail.ShowDialog();
            LoadCustomers();
        }
    }

    private void BtnEdit_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is int id)
        {
            var edit = new AddCustomerWindow(id);
            edit.ShowDialog();
            LoadCustomers();
        }
    }

    private async void BtnDelete_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is int id)
        {
            var result = MessageBox.Show(
                "Bu müşteriyi silmek istediğinize emin misiniz?",
                "Silme Onayı",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                await _customerService.DeleteCustomerAsync(id);
                LoadCustomers();
            }
        }
    }
}