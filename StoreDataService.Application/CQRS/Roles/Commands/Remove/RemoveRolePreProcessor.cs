using MediatR.Pipeline;
using Microsoft.AspNetCore.Identity;
using StoreDataService.Core.Exceptions;
using StoreDataService.Domain.Entities;

namespace StoreDataService.Application.CQRS.Roles.Commands.Remove;

public sealed class RemoveRolePreProcessor : IRequestPreProcessor<RemoveRoleCommand>
{
    private readonly UserManager<User> _userManager;

    public RemoveRolePreProcessor(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task Process(RemoveRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null) throw new NotFoundException("User was not found");
        var userPolicy = await _userManager.GetRolesAsync(user);
        if (!userPolicy.Contains(request.Role.ToString()))
            throw new NotFoundException($"User was not have {request.Role} role");
    }
}