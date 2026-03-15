using VereSimple.Models;
using VereSimple.Repositories;

namespace VereSimple.Services;

public class CustomerService
{
    private readonly ICustomerRepository _customerRepo;
    private readonly ITransactionRepository _transactionRepo;

    public CustomerService(ICustomerRepository customerRepo, ITransactionRepository transactionRepo)
    {
        _customerRepo = customerRepo;
        _transactionRepo = transactionRepo;
    }

    public async Task<List<Customer>> GetAllCustomersAsync()
    {
        return await _customerRepo.GetAllAsync();
    }

    public async Task<Customer?> GetCustomerByIdAsync(int id)
    {
        return await _customerRepo.GetByIdAsync(id);
    }

    public async Task<Customer> AddCustomerAsync(string fullName, string? phone, string? address, string? notes)
    {
        var customer = new Customer
        {
            FullName = fullName.Trim(),
            Phone = phone?.Trim(),
            Address = address?.Trim(),
            Notes = notes?.Trim(),
            CreatedAt = DateTime.Now
        };
        return await _customerRepo.AddAsync(customer);
    }

    public async Task UpdateCustomerAsync(Customer customer)
    {
        await _customerRepo.UpdateAsync(customer);
    }

    public async Task DeleteCustomerAsync(int id)
    {
        await _customerRepo.DeleteAsync(id);
    }

    public async Task<List<Customer>> SearchCustomersAsync(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return await _customerRepo.GetAllAsync();

        return await _customerRepo.SearchAsync(keyword);
    }

    public async Task<decimal> GetRemainingDebtAsync(int customerId)
    {
        var totalDebt = await _transactionRepo.GetTotalDebtByCustomerAsync(customerId);
        var totalPayment = await _transactionRepo.GetTotalPaymentByCustomerAsync(customerId);
        return totalDebt - totalPayment;
    }
}