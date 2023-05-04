using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreDataService.API.Persistence;
using StoreDataService.Application.CQRS.Users.Queries.GetAll;
using StoreDataService.Application.CQRS.Users.Queries.GetBirthdayPeople;
using StoreDataService.Application.CQRS.Users.Queries.GetUser;
using StoreDataService.Application.CQRS.Users.Queries.GetUserAndCategory;
using StoreDataService.Application.CQRS.Users.Queries.GetUserByLastPayment;
using StoreDataService.Application.CQRS.Users.Queries.Views;

namespace StoreDataService.API.Controllers;

public class UserInfoController : BaseController
{
    [HttpGet]
    [Authorize(Policy = Policies.AdminOrManager)]
    [ProducesResponseType(typeof(IEnumerable<UserView>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsers([FromQuery] int? page = 1)
    {
        var result = await _mediator.Send(new GetAllUsersQuery(page!.Value));
        return Ok(result);
    }

    [HttpGet("{userId}")]
    [Authorize(Policy = Policies.AdminOrManager)]
    [ProducesResponseType(typeof(UserView), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromRoute] Guid userId)
    {
        var result = await _mediator.Send(new GetUserQuery().Create(userId));
        return Ok(result);
    }

    [HttpGet("/birthday")]
    [Authorize(Policy = Policies.AdminOrManager)]
    [ProducesResponseType(typeof(IEnumerable<UserView>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBirthdayPeople([FromQuery] DateTime birthday)
    {
        var result = await _mediator.Send(new GetBirthdayPeopleQuery(birthday));
        return Ok(result);
    }

    [HttpGet("/lastPayment")]
    [Authorize(Policy = Policies.AdminOrManager)]
    [ProducesResponseType(typeof(IEnumerable<UserViewAndLastPaymentDate>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserByLastPaymentDate([FromQuery] int? lastPayment = 1)
    {
        var result = await _mediator.Send(new GetUserByLastPaymentQuery(lastPayment.Value));
        return Ok(result);
    }

    [HttpGet("/{userId}/categories")]
    [Authorize(Policy = Policies.AdminOrManager)]
    [ProducesResponseType(typeof(Dictionary<string, int>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserAndCategoryInfo([FromRoute] Guid userId)
    {
        var result = await _mediator.Send(new GetUserAndCategoryQuery(userId));
        return Ok(result);
    }
}