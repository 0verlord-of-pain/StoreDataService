namespace StoreDataService.Domain.Entities;

public class Transaction : IBaseEntity
{
    public Dictionary<Guid, int> Products = new();
    public decimal Amount { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = new();
    public Guid Id { get; set; }
    public bool IsArchived { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? UpdatedOnUtc { get; set; }

    public void SoftDelete()
    {
        IsArchived = true;
    }

    public void Restore()
    {
        IsArchived = false;
    }

    public static Transaction Create(decimal amount, User user, Dictionary<Guid, int> products)
    {
        return new Transaction
        {
            Amount = amount,
            User = user,
            UserId = user.Id,
            Products = products
        };
    }
}