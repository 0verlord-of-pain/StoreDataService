using MediatR;
using StoreDataService.Application.CQRS.Transactions.Queries.Views;

namespace StoreDataService.Application.CQRS.Transactions.Queries.GetTransactions;

public class GetUserTransactionsQuery : IRequest<IEnumerable<TransactionView>>
{
    public GetUserTransactionsQuery(Guid userId, int page)
    {
        UserId = userId;
        Page = page;
    }

    public Guid UserId { get; init; }
    public int Page { get; init; }
}