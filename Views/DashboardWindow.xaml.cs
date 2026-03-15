using System.Windows;
using VereSimple.Models;

namespace VereSimple.Views;

public partial class DashboardWindow : Window
{
    private readonly User _currentUser;

    public DashboardWindow(User user)
    {
        InitializeComponent();
        _currentUser = user;
        MainFrame.Navigate(new DashboardPage());
    }

    private void BtnLogout_Click(object sender, RoutedEventArgs e)
    {
        var login = new LoginWindow();
        login.Show();
        this.Close();
    }

    private void BtnDashboard_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new DashboardPage());
    }

    private void BtnCustomers_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new CustomersPage());
    }
}