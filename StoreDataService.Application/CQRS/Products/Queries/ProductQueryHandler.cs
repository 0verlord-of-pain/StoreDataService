using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreDataService.Application.CQRS.Products.Queries.GetProductByCategory;
using StoreDataService.Application.CQRS.Products.Queries.GetProductById;
using StoreDataService.Application.CQRS.Products.Queries.Views;
using StoreDataService.Storage.Persistence;

namespace StoreDataService.Application.CQRS.Products.Queries;

public sealed class ProductQueryHandler :
    IRequestHandler<GetProductByCategoryQuery, IEnumerable<ProductView>>,
    IRequestHandler<GetProductByIdQuery, ProductView>
{
    private const int Limit = 10;
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public ProductQueryHandler(IMapper mapper, DataContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<IEnumerable<ProductView>> Handle(GetProductByCategoryQuery request,
        CancellationToken cancellationToken)
    {
        var transactions = await _context.Products
            .Where(i => i.Category == request.Category)
            .Skip(Limit * (request.Page - 1))
            .Take(Limit)
            .ToListAsync(cancellationToken);

        var result = _mapper.Map<IEnumerable<ProductView>>(transactions);

        return result;
    }

    public async Task<ProductView> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var transactions = await _context.Products
            .FirstOrDefaultAsync(i => i.Id == request.ProductId, cancellationToken);

        var result = _mapper.Map<ProductView>(transactions);

        return result;
    }
}