using MediatR;

namespace StoreDataService.Application.CQRS.Users.Commands.RestoreUser;

public class RestoreUserCommand : IRequest<Guid>
{
    public RestoreUserCommand(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; init; }
}