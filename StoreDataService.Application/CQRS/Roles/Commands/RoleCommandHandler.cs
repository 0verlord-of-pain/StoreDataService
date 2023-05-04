using MediatR;
using Microsoft.AspNetCore.Identity;
using StoreDataService.Application.CQRS.Roles.Commands.Attach;
using StoreDataService.Application.CQRS.Roles.Commands.Remove;
using StoreDataService.Core.Exceptions;
using StoreDataService.Core.Extensions;
using StoreDataService.Domain.Entities;

namespace StoreDataService.Application.CQRS.Roles.Commands;

public class RoleCommandHandler :
    IRequestHandler<AttachRoleCommand, Unit>,
    IRequestHandler<RemoveRoleCommand, Unit>
{
    private readonly UserManager<User> _userManager;

    public RoleCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Unit> Handle(AttachRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        var addToRoleResult = await _userManager.AddToRoleAsync(user, request.Role.ToString());
        if (addToRoleResult.TryGetErrors(out var addToRoleErrors)) throw new IdentityUserException(addToRoleErrors);
        return Unit.Value;
    }

    public async Task<Unit> Handle(RemoveRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        var addToRoleResult = await _userManager.RemoveFromRoleAsync(user, request.Role.ToString());
        if (addToRoleResult.TryGetErrors(out var addToRoleErrors)) throw new IdentityUserException(addToRoleErrors);
        return Unit.Value;
    }
}