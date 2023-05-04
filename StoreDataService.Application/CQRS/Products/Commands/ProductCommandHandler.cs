using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreDataService.Application.CQRS.Products.Commands.Create;
using StoreDataService.Application.CQRS.Products.Commands.Delete;
using StoreDataService.Application.CQRS.Products.Queries.Views;
using StoreDataService.Domain.Entities;
using StoreDataService.Storage.Persistence;

namespace StoreDataService.Application.CQRS.Products.Commands;

public class ProductCommandHandler :
    IRequestHandler<CreateProductCommand, ProductView>,
    IRequestHandler<DeleteProductCommand, Unit>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public ProductCommandHandler(IMapper mapper, DataContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<ProductView> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = Product.Create(request.Name, request.Category, request.Article, request.Price);

        await _context.Products.AddAsync(product, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        var result = _mapper.Map<ProductView>(product);

        return result;
    }

    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _context.Products
            .FirstOrDefaultAsync(i => i.Id == request.ProductId, cancellationToken);

        transaction?.SoftDelete();

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}