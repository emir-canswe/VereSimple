using Microsoft.EntityFrameworkCore;
using VereSimple.Data;
using VereSimple.Models;

namespace VereSimple.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Customer>> GetAllAsync()
    {
        return await _context.Customers
            .Where(c => c.IsActive)
            .OrderBy(c => c.FullName)
            .ToListAsync();
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        return await _context.Customers
            .Include(c => c.Transactions)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Customer> AddAsync(Customer customer)
    {
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task UpdateAsync(Customer customer)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer != null)
        {
            customer.IsActive = false;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Customer>> SearchAsync(string keyword)
    {
        return await _context.Customers
            .Where(c => c.IsActive && 
                   (c.FullName.Contains(keyword) || 
                    (c.Phone != null && c.Phone.Contains(keyword))))
            .ToListAsync();
    }
}