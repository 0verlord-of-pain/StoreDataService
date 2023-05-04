using MediatR;

namespace StoreDataService.Application.CQRS.Products.Commands.Delete;

public class DeleteProductCommand : IRequest<Unit>
{
    public DeleteProductCommand(Guid userId, Guid productId)
    {
        UserId = userId;
        ProductId = productId;
    }

    public Guid UserId { get; init; }
    public Guid ProductId { get; init; }
}