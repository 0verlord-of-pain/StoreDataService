using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using StoreDataService.Core.Exceptions;
using StoreDataService.Storage.Persistence;

namespace StoreDataService.Application.CQRS.Transactions.Commands.Create;

public sealed class CreateTransactionPreProcessor : IRequestPreProcessor<CreateTransactionCommand>
{
    private readonly DataContext _context;

    public CreateTransactionPreProcessor(DataContext context)
    {
        _context = context;
    }

    public async Task Process(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(i => i.Id == request.UserId, cancellationToken);
        if (user is null) throw new NotFoundException("User was not found");

        if (request.Products is null) throw new NotFoundException("Shopping list cannot be empty");
    }
}