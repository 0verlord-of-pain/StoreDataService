using StoreDataService.Domain.Entities;

namespace StoreDataService.Application.CQRS.Transactions.Queries.Views;

public class TransactionView
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = new();
    public List<Product> Products { get; set; } = new();
    public bool IsArchived { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? UpdatedOnUtc { get; set; }
}