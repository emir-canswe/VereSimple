using System.Windows.Controls;
using VereSimple.Data;

namespace VereSimple.Views;

public partial class DashboardPage : Page
{
    public DashboardPage()
    {
        InitializeComponent();
        LoadStats();
    }

    private void LoadStats()
    {
        try
        {
            using var db = new AppDbContext();

            var debtList = db.Transactions
                .Where(t => t.Type == "DEBT")
                .Select(t => t.Amount)
                .ToList();

            var paymentList = db.Transactions
                .Where(t => t.Type == "PAYMENT")
                .Select(t => t.Amount)
                .ToList();

            decimal totalDebt = debtList.Any() ? debtList.Sum() : 0;
            decimal totalPayment = paymentList.Any() ? paymentList.Sum() : 0;
            int totalCustomers = db.Customers.Count(c => c.IsActive);

            TxtTotalDebt.Text = $"{totalDebt:N2} ₺";
            TxtTotalPayment.Text = $"{totalPayment:N2} ₺";
            TxtRemainingDebt.Text = $"{totalDebt - totalPayment:N2} ₺";
            TxtTotalCustomers.Text = totalCustomers.ToString();
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show($"Dashboard hatası: {ex.Message}");
        }
    }
}