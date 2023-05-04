using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using StoreDataService.Core.Exceptions;
using StoreDataService.Storage.Persistence;

namespace StoreDataService.Application.CQRS.Products.Queries.GetProductById;

public sealed class GetProductByIdPreProcessor : IRequestPreProcessor<GetProductByIdQuery>
{
    private readonly DataContext _context;

    public GetProductByIdPreProcessor(DataContext context)
    {
        _context = context;
    }

    public async Task Process(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FirstOrDefaultAsync(i => i.Id == request.ProductId, cancellationToken);
        if (product is null) throw new NotFoundException("Product was not found");
    }
}