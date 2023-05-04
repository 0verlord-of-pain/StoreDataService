using MediatR;
using StoreDataService.Application.CQRS.Products.Queries.Views;

namespace StoreDataService.Application.CQRS.Products.Queries.GetProductByCategory;

public class GetProductByCategoryQuery : IRequest<IEnumerable<ProductView>>
{
    public GetProductByCategoryQuery(string category, int page)
    {
        Category = category;
        Page = page;
    }

    public string Category { get; init; }
    public int Page { get; init; }
}