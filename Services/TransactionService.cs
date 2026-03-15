using VereSimple.Models;
using VereSimple.Repositories;

namespace VereSimple.Services;

public class TransactionService
{
    private readonly ITransactionRepository _transactionRepo;

    public TransactionService(ITransactionRepository transactionRepo)
    {
        _transactionRepo = transactionRepo;
    }

    public async Task AddDebtAsync(int customerId, decimal amount, string? description)
    {
        var transaction = new Transaction
        {
            CustomerId = customerId,
            Amount = amount,
            Type = "DEBT",
            Description = description,
            Date = DateTime.Now
        };
        await _transactionRepo.AddAsync(transaction);
    }

    public async Task AddPaymentAsync(int customerId, decimal amount, string? description)
    {
        var transaction = new Transaction
        {
            CustomerId = customerId,
            Amount = amount,
            Type = "PAYMENT",
            Description = description,
            Date = DateTime.Now
        };
        await _transactionRepo.AddAsync(transaction);
    }

    public async Task<List<Transaction>> GetCustomerTransactionsAsync(int customerId)
    {
        return await _transactionRepo.GetByCustomerIdAsync(customerId);
    }

    public async Task<List<Transaction>> GetTransactionsByDateRangeAsync(DateTime start, DateTime end)
    {
        return await _transactionRepo.GetByDateRangeAsync(start, end);
    }

    public async Task DeleteTransactionAsync(int id)
    {
        await _transactionRepo.DeleteAsync(id);
    }
}