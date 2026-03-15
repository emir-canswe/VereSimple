using Microsoft.EntityFrameworkCore;
using VereSimple.Data;
using VereSimple.Models;

namespace VereSimple.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _context;

    public TransactionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Transaction>> GetByCustomerIdAsync(int customerId)
    {
        return await _context.Transactions
            .Where(t => t.CustomerId == customerId)
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }

    public async Task<Transaction> AddAsync(Transaction transaction)
    {
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
        return transaction;
    }

    public async Task DeleteAsync(int id)
    {
        var transaction = await _context.Transactions.FindAsync(id);
        if (transaction != null)
        {
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<decimal> GetTotalDebtByCustomerAsync(int customerId)
    {
        return await _context.Transactions
            .Where(t => t.CustomerId == customerId && t.Type == "DEBT")
            .SumAsync(t => t.Amount);
    }

    public async Task<decimal> GetTotalPaymentByCustomerAsync(int customerId)
    {
        return await _context.Transactions
            .Where(t => t.CustomerId == customerId && t.Type == "PAYMENT")
            .SumAsync(t => t.Amount);
    }

    public async Task<List<Transaction>> GetByDateRangeAsync(DateTime start, DateTime end)
    {
        return await _context.Transactions
            .Include(t => t.Customer)
            .Where(t => t.Date >= start && t.Date <= end)
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }
}