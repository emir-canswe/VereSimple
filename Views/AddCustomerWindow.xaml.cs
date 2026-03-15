using System.Windows;
using VereSimple.Data;
using VereSimple.Repositories;
using VereSimple.Services;

namespace VereSimple.Views;

public partial class AddCustomerWindow : Window
{
    private readonly CustomerService _customerService;
    private readonly int? _editId;

    public AddCustomerWindow(int? editId = null)
    {
        InitializeComponent();
        var db = new AppDbContext();
        var customerRepo = new CustomerRepository(db);
        var transactionRepo = new TransactionRepository(db);
        _customerService = new CustomerService(customerRepo, transactionRepo);
        _editId = editId;

        if (editId.HasValue)
        {
            TxtTitle.Text = "Müşteri Düzenle";
            LoadCustomer(editId.Value);
        }
    }

    private async void LoadCustomer(int id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id);
        if (customer != null)
        {
            TxtFullName.Text = customer.FullName;
            TxtPhone.Text = customer.Phone;
            TxtAddress.Text = customer.Address;
            TxtNotes.Text = customer.Notes;
        }
    }

    private async void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        string fullName = TxtFullName.Text.Trim();

        if (string.IsNullOrEmpty(fullName))
        {
            TxtError.Text = "Ad Soyad alanı zorunludur.";
            TxtError.Visibility = Visibility.Visible;
            return;
        }

        try
        {
            if (_editId.HasValue)
            {
                var customer = await _customerService.GetCustomerByIdAsync(_editId.Value);
                if (customer != null)
                {
                    customer.FullName = fullName;
                    customer.Phone = TxtPhone.Text.Trim();
                    customer.Address = TxtAddress.Text.Trim();
                    customer.Notes = TxtNotes.Text.Trim();
                    await _customerService.UpdateCustomerAsync(customer);
                }
            }
            else
            {
                await _customerService.AddCustomerAsync(
                    fullName,
                    TxtPhone.Text.Trim(),
                    TxtAddress.Text.Trim(),
                    TxtNotes.Text.Trim());
            }

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