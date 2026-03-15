namespace VereSimple.Models;

public class Transaction
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; } = string.Empty; // "DEBT" veya "PAYMENT"
    public string? Description { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public Customer Customer { get; set; } = null!;
}