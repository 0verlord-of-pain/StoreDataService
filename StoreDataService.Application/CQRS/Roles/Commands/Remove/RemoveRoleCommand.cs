using MediatR;

namespace StoreDataService.Application.CQRS.Roles.Commands.Remove;

public class RemoveRoleCommand : IRequest<Unit>
{
    public RemoveRoleCommand(Guid userId, Core.Enums.Roles role)
    {
        Role = role;
        UserId = userId;
    }

    public Core.Enums.Roles Role { get; init; }
    public Guid UserId { get; init; }
}