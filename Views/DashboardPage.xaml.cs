using System.Windows.Controls;
using VereSimple.Data;
using VereSimple.Repositories;
using VereSimple.Services;

namespace VereSimple.Views;

public partial class DashboardPage : Page
{
    public DashboardPage()
    {
        InitializeComponent();
        Loaded += async (s, e) => await LoadStatsAsync();
    }

    private async Task LoadStatsAsync()
    {
        try
        {
            var db = new AppDbContext();
            var customerRepo = new CustomerRepository(db);
            var transactionRepo = new TransactionRepository(db);
            var customerService = new CustomerService(customerRepo, transactionRepo);

            var debtList = await transactionRepo.GetByDateRangeAsync(
                DateTime.MinValue, DateTime.MaxValue);

            decimal totalDebt = debtList
                .Where(t => t.Type == "DEBT").Sum(t => t.Amount);
            decimal totalPayment = debtList
                .Where(t => t.Type == "PAYMENT").Sum(t => t.Amount);

            var customers = await customerRepo.GetAllAsync();
            int totalCustomers = customers.Count;

            TxtTotalDebt.Text = $"{totalDebt:N2} ₺";
            TxtTotalPayment.Text = $"{totalPayment:N2} ₺";
            TxtRemainingDebt.Text = $"{totalDebt - totalPayment:N2} ₺";
            TxtTotalCustomers.Text = totalCustomers.ToString();

            // En borçlu 5 müşteri
            var topList = new List<TopDebtorViewModel>();
            foreach (var c in customers)
            {
                var debt = await customerService.GetRemainingDebtAsync(c.Id);
                if (debt > 0)
                    topList.Add(new TopDebtorViewModel
                    {
                        FullName = c.FullName,
                        Phone = c.Phone ?? "-",
                        RemainingDebt = debt
                    });
            }

            TopDebtorsGrid.ItemsSource = topList
                .OrderByDescending(x => x.RemainingDebt)
                .Take(5)
                .ToList();
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show($"Dashboard hatası: {ex.Message}");
        }
    }
}

public class TopDebtorViewModel
{
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public decimal RemainingDebt { get; set; }
}