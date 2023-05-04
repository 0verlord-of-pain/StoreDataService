using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreDataService.Application.CQRS.Transactions.Commands.Create;
using StoreDataService.Application.CQRS.Transactions.Commands.Delete;
using StoreDataService.Application.CQRS.Transactions.Queries.Views;
using StoreDataService.Domain.Entities;
using StoreDataService.Storage.Persistence;

namespace StoreDataService.Application.CQRS.Transactions.Commands;

public class TransactionCommandHandler :
    IRequestHandler<CreateTransactionCommand, TransactionView>,
    IRequestHandler<DeleteTransactionCommand, Unit>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public TransactionCommandHandler(IMapper mapper, DataContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<TransactionView> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(i => i.Id == request.UserId, cancellationToken);

        var products = await _context.Products
            .Where(i => request.Products.ContainsKey(i.Id))
            .ToListAsync(cancellationToken);

        var totalAmount = 0m;

        var paymentProducts = new Dictionary<Guid, int>();
        foreach (var product in products)
            if (request.Products.TryGetValue(product.Id, out var quantity))
            {
                totalAmount += product.Price * quantity;
                paymentProducts.Add(product.Id, quantity);
            }

        var transaction = Transaction.Create(totalAmount, user, paymentProducts);

        await _context.Transactions.AddAsync(transaction, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        var result = _mapper.Map<TransactionView>(transaction);

        return result;
    }

    public async Task<Unit> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _context.Transactions
            .FirstOrDefaultAsync(i => i.Id == request.TransactionId, cancellationToken);

        transaction?.SoftDelete();

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}