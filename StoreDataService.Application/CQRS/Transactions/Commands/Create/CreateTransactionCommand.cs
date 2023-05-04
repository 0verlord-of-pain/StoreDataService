using MediatR;
using StoreDataService.Application.CQRS.Transactions.Queries.Views;

namespace StoreDataService.Application.CQRS.Transactions.Commands.Create;

public class CreateTransactionCommand : IRequest<TransactionView>
{
    public CreateTransactionCommand(Guid userId, Dictionary<Guid, int> products)
    {
        UserId = userId;
        Products = products;
    }

    public Guid UserId { get; init; }
    public Dictionary<Guid, int> Products { get; init; }
}