using MediatR;
using StoreDataService.Application.CQRS.Products.Queries.Views;

namespace StoreDataService.Application.CQRS.Products.Queries.GetProductById;

public class GetProductByIdQuery : IRequest<ProductView>
{
    public GetProductByIdQuery(Guid productId)
    {
        ProductId = productId;
    }

    public Guid ProductId { get; init; }
}