using MediatR;
using StoreDataService.Application.CQRS.Products.Queries.Views;

namespace StoreDataService.Application.CQRS.Products.Commands.Create;

public class CreateProductCommand : IRequest<ProductView>
{
    public CreateProductCommand(Guid userId, string name,
        string category, string article, decimal price)
    {
        UserId = userId;
        Name = name;
        Category = category;
        Article = article;
        Price = price;
    }

    public Guid UserId { get; init; }
    public string Name { get; init; }
    public string Category { get; init; }
    public string Article { get; init; }
    public decimal Price { get; init; }
}