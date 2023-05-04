namespace StoreDataService.Application.CQRS.Products.Queries.Views;

public class ProductView
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Article { get; set; } = string.Empty;
    public decimal Price { get; set; }
}