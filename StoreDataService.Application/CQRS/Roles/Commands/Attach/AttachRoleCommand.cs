using MediatR;

namespace StoreDataService.Application.CQRS.Roles.Commands.Attach;

public class AttachRoleCommand : IRequest<Unit>
{
    public AttachRoleCommand(Guid userId, Core.Enums.Roles role)
    {
        Role = role;
        UserId = userId;
    }

    public Core.Enums.Roles Role { get; init; }
    public Guid UserId { get; init; }
}