﻿using MediatR;

namespace StoreDataService.Application.CQRS.Users.Commands.DeleteUser;

public class DeleteUserCommand : IRequest<Unit>
{
    public DeleteUserCommand(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; }
}