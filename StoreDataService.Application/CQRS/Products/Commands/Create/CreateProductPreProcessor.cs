using MediatR.Pipeline;
using Microsoft.AspNetCore.Identity;
using StoreDataService.Core.Exceptions;
using StoreDataService.Domain.Entities;

namespace StoreDataService.Application.CQRS.Products.Commands.Create;

public sealed class CreateProductPreProcessor : IRequestPreProcessor<CreateProductCommand>
{
    private readonly UserManager<User> _userManager;

    public CreateProductPreProcessor(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task Process(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null) throw new NotFoundException("User was not found");

        var userPolicy = await _userManager.GetRolesAsync(user);
        if (!userPolicy.Contains(Core.Enums.Roles.Admin.ToString())
            && !userPolicy.Contains(Core.Enums.Roles.Manager.ToString()))
            throw new ForbidException("You do not have permission to do this");

        if (string.IsNullOrEmpty(request.Name)) throw new ValidationException("The Name field cannot be empty");
        if (string.IsNullOrEmpty(request.Article)) throw new ValidationException("The Article field cannot be empty");
        if (string.IsNullOrEmpty(request.Category)) throw new ValidationException("The Category field cannot be empty");
        if (request.Price <= 0m) throw new ValidationException("Price too low");
    }
}