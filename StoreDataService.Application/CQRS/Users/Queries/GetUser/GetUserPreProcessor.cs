using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using StoreDataService.Core.Exceptions;
using StoreDataService.Storage.Persistence;

namespace StoreDataService.Application.CQRS.Users.Queries.GetUser;

public sealed class GetUserPreProcessor : IRequestPreProcessor<GetUserQuery>
{
    private readonly DataContext _context;

    public GetUserPreProcessor(DataContext context)
    {
        _context = context;
    }

    public async Task Process(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(i => i.Id == request.UserId, cancellationToken);

        if (user is null) throw new NotFoundException("User was not found");
    }
}