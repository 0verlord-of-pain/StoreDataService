﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreDataService.API.Controllers.In;
using StoreDataService.API.Infrastructure.Validators;
using StoreDataService.API.Persistence;
using StoreDataService.Application.CQRS.Users.Commands.DeleteUser;
using StoreDataService.Application.CQRS.Users.Commands.RestoreUser;
using StoreDataService.Core.Exceptions;
using StoreDataService.Core.Extensions;
using StoreDataService.Domain.Entities;

namespace StoreDataService.API.Controllers;

public class AccountController : BaseController
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("signup")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> SignUp([FromBody] CreateUserModel model)
    {
        if (!EmailValidator.IsValid(model.Email)) throw new ValidationException("Email is not valid");

        if (!NameValidator.IsValid(model.Name)) throw new ValidationException("Name is not valid");
        if (!NameValidator.IsValid(model.Surname)) throw new ValidationException("Surname is not valid");
        if (!NameValidator.IsValid(model.FatherName)) throw new ValidationException("FatherName is not valid");

        if (!await new PasswordValidator(_userManager).IsValidAsync(model.Password))
            throw new ValidationException("Password is not valid");

        var user = await _userManager.Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Email.Equals(model.Email));

        if (user != null)
        {
            if (user.IsArchived)
                throw new UserDeleteException(
                    "The user with this email has been deleted. If you would like to install it please contact support.");
            throw new ValidationException("User already exist");
        }

        user = Domain.Entities.User.Create(model.Email, model.Surname, model.Name, model.FatherName, model.Birthday);

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.TryGetErrors(out _)) throw new IdentityUserException("Failed to add user");

        var addToRoleResult = await _userManager.AddToRoleAsync(user, "User");

        if (addToRoleResult.TryGetErrors(out _)) throw new IdentityUserException("Failed to add role to user");

        return Created("", "");
    }

    [HttpPut("signin")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SignIn(
        [FromQuery] string email,
        [FromQuery] string password)
    {
        if (!EmailValidator.IsValid(email)) throw new ValidationException("Email is not valid");

        if (!await new PasswordValidator(_userManager).IsValidAsync(password))
            throw new ValidationException("Password is not valid");

        var user = await _userManager.FindByEmailAsync(email);
        if (user is null) throw new IdentityUserException("User with this email not found");

        if (user.IsArchived)
            throw new UserDeleteException(
                "The user with this email has been deleted. If you would like to install it please contact support.");
        var userName = user.UserName;

        var result = await _signInManager.PasswordSignInAsync(userName, password, false, false);

        if (!result.Succeeded) return BadRequest();

        return Ok();
    }

    [HttpPut("logout")]
    [Authorize(Policy = Policies.User)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }


    [HttpDelete("{userId}")]
    [Authorize(Policy = Policies.AdminOrManager)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete([FromRoute] Guid userId)
    {
        await _mediator.Send(new DeleteUserCommand(userId));
        return NoContent();
    }

    [HttpPut("{userId}/restore")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [Authorize(Policy = Policies.AdminOrManager)]
    public async Task<IActionResult> Restore([FromRoute] Guid userId)
    {
        var result = await _mediator.Send(new RestoreUserCommand(userId));
        return Ok(result);
    }
}