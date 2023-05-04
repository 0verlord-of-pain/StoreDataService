using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreDataService.API.Controllers.In;
using StoreDataService.API.Persistence;
using StoreDataService.Application.CQRS.Transactions.Commands.Create;
using StoreDataService.Application.CQRS.Transactions.Commands.Delete;
using StoreDataService.Application.CQRS.Transactions.Queries.GetTransactionById;
using StoreDataService.Application.CQRS.Transactions.Queries.GetTransactions;
using StoreDataService.Application.CQRS.Transactions.Queries.GetTransactionsByUserId;
using StoreDataService.Application.CQRS.Transactions.Queries.Views;

namespace StoreDataService.API.Controllers;

public class TransactionController : BaseController
{
    [HttpGet]
    [Authorize(Policy = Policies.User)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserTransactions([FromQuery] int? page = 1)
    {
        var result = await _mediator.Send(new GetUserTransactionsQuery(UserId, page.Value));
        return Ok(result);
    }

    [HttpGet("{transactionId}")]
    [Authorize(Policy = Policies.User)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTransaction([FromRoute] Guid transactionId)
    {
        var result = await _mediator.Send(new GetTransactionByIdQuery(UserId, transactionId));
        return Ok(result);
    }

    [HttpGet("users/{userId}")]
    [Authorize(Policy = Policies.AdminOrManager)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTransactionsByUserId([FromRoute] Guid userId, [FromQuery] int? page = 1)
    {
        var result = await _mediator.Send(new GetTransactionsByUserIdQuery(userId, page.Value));
        return Ok(result);
    }

    [HttpPost("create")]
    [Authorize(Policy = Policies.User)]
    [ProducesResponseType(typeof(TransactionView), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create([FromBody] CreateTransactionModel model)
    {
        var command = new CreateTransactionCommand(model.UserId, model.Products);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{transactionId}")]
    [Authorize(Policy = Policies.AdminOrManager)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete([FromRoute] Guid transactionId)
    {
        await _mediator.Send(new DeleteTransactionCommand(UserId, transactionId));
        return NoContent();
    }
}