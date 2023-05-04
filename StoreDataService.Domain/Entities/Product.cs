namespace StoreDataService.Domain.Entities;

public class Product : IBaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Article { get; set; } = string.Empty;
    public decimal Price { get; set; }
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

    public static Product Create(string name, string category,
        string article, decimal price)
    {
        return new Product
        {
            Name = name,
            Category = category,
            Article = article,
            Price = price
        };
    }
}