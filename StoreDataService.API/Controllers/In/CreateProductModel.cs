namespace StoreDataService.API.Controllers.In;

public class CreateProductModel
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Article { get; set; } = string.Empty;
    public decimal Price { get; set; }
}