using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using StoreDataService.Core.Exceptions;
using StoreDataService.Storage.Persistence;

namespace StoreDataService.Application.CQRS.Users.Queries.GetUserAndCategory;

public sealed class GetUserAndCategoryPreProcessor : IRequestPreProcessor<GetUserAndCategoryQuery>
{
    private readonly DataContext _context;

    public GetUserAndCategoryPreProcessor(DataContext context)
    {
        _context = context;
    }

    public async Task Process(GetUserAndCategoryQuery request, CancellationToken cancellationToken)
    {
        var product = await _context.Users.FirstOrDefaultAsync(i => i.Id == request.UserId, cancellationToken);
        if (product is null) throw new NotFoundException("User was not found");
    }
}