using MediatR.Pipeline;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StoreDataService.Core.Exceptions;
using StoreDataService.Domain.Entities;
using StoreDataService.Storage.Persistence;

namespace StoreDataService.Application.CQRS.Products.Commands.Delete;

public sealed class DeleteProductPreProcessor : IRequestPreProcessor<DeleteProductCommand>
{
    private readonly DataContext _context;
    private readonly UserManager<User> _userManager;

    public DeleteProductPreProcessor(UserManager<User> userManager, DataContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task Process(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null) throw new NotFoundException("User was not found");

        var transaction =
            await _context.Products.FirstOrDefaultAsync(i => i.Id == request.ProductId, cancellationToken);
        if (transaction is null) throw new NotFoundException("Product was not found");

        var userPolicy = await _userManager.GetRolesAsync(user);
        if (!userPolicy.Contains(Core.Enums.Roles.Admin.ToString())
            && !userPolicy.Contains(Core.Enums.Roles.Manager.ToString()))
            throw new ForbidException("You do not have permission to do this");
    }
}