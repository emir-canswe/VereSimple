using VereSimple.Models;

namespace VereSimple.Repositories;

public interface ICustomerRepository
{
    Task<List<Customer>> GetAllAsync();
    Task<Customer?> GetByIdAsync(int id);
    Task<Customer> AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(int id);
    Task<List<Customer>> SearchAsync(string keyword);
}