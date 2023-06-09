﻿using MediatR;
using StoreDataService.Application.CQRS.Transactions.Queries.Views;

namespace StoreDataService.Application.CQRS.Transactions.Queries.GetTransactionsByUserId;

public class GetTransactionsByUserIdQuery : IRequest<IEnumerable<TransactionView>>
{
    public GetTransactionsByUserIdQuery(Guid userId, int page)
    {
        UserId = userId;
        Page = page;
    }

    public Guid UserId { get; init; }
    public int Page { get; init; }
}