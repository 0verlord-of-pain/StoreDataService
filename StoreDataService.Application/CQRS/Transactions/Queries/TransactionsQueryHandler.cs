﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreDataService.Application.CQRS.Transactions.Queries.GetTransactionById;
using StoreDataService.Application.CQRS.Transactions.Queries.GetTransactions;
using StoreDataService.Application.CQRS.Transactions.Queries.GetTransactionsByUserId;
using StoreDataService.Application.CQRS.Transactions.Queries.Views;
using StoreDataService.Storage.Persistence;

namespace StoreDataService.Application.CQRS.Transactions.Queries;

public sealed class TransactionsQueryHandler :
    IRequestHandler<GetUserTransactionsQuery, IEnumerable<TransactionView>>,
    IRequestHandler<GetTransactionsByUserIdQuery, IEnumerable<TransactionView>>,
    IRequestHandler<GetTransactionByIdQuery, TransactionView>
{
    private const int Limit = 10;
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public TransactionsQueryHandler(IMapper mapper, DataContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<TransactionView> Handle(
        GetTransactionByIdQuery request,
        CancellationToken cancellationToken)
    {
        var transactions = await _context.Transactions
            .FirstOrDefaultAsync(i => i.Id == request.TransactionId, cancellationToken);

        var result = _mapper.Map<TransactionView>(transactions);

        return result;
    }

    public async Task<IEnumerable<TransactionView>> Handle(
        GetTransactionsByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var transactions = await _context.Transactions
            .Where(i => i.UserId == request.UserId)
            .Skip(Limit * (request.Page - 1))
            .Take(Limit)
            .ToListAsync(cancellationToken);

        var result = _mapper.Map<IEnumerable<TransactionView>>(transactions);

        return result;
    }

    public async Task<IEnumerable<TransactionView>> Handle(
        GetUserTransactionsQuery request,
        CancellationToken cancellationToken)
    {
        var transactions = await _context.Transactions
            .Where(i => i.UserId == request.UserId)
            .Skip(Limit * (request.Page - 1))
            .Take(Limit)
            .ToListAsync(cancellationToken);

        var result = _mapper.Map<IEnumerable<TransactionView>>(transactions);

        return result;
    }
}