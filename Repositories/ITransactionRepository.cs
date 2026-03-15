using VereSimple.Models;

namespace VereSimple.Repositories;

public interface ITransactionRepository
{
    Task<List<Transaction>> GetByCustomerIdAsync(int customerId);
    Task<Transaction> AddAsync(Transaction transaction);
    Task DeleteAsync(int id);
    Task<decimal> GetTotalDebtByCustomerAsync(int customerId);
    Task<decimal> GetTotalPaymentByCustomerAsync(int customerId);
    Task<List<Transaction>> GetByDateRangeAsync(DateTime start, DateTime end);
}