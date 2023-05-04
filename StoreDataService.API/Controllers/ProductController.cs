using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreDataService.API.Controllers.In;
using StoreDataService.API.Persistence;
using StoreDataService.Application.CQRS.Products.Commands.Create;
using StoreDataService.Application.CQRS.Products.Commands.Delete;
using StoreDataService.Application.CQRS.Products.Queries.GetProductByCategory;
using StoreDataService.Application.CQRS.Products.Queries.GetProductById;
using StoreDataService.Application.CQRS.Products.Queries.Views;

namespace StoreDataService.API.Controllers;

public class ProductController : BaseController
{
    [HttpGet("/{category}")]
    [Authorize(Policy = Policies.User)]
    [ProducesResponseType(typeof(IEnumerable<ProductView>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductByCategory([FromQuery] string category, [FromQuery] int? page = 1)
    {
        var result = await _mediator.Send(new GetProductByCategoryQuery(category, page.Value));
        return Ok(result);
    }

    [HttpGet("{productId}")]
    [Authorize(Policy = Policies.User)]
    [ProducesResponseType(typeof(ProductView), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTransaction([FromRoute] Guid productId)
    {
        var result = await _mediator.Send(new GetProductByIdQuery(productId));
        return Ok(result);
    }

    [HttpPost("create")]
    [Authorize(Policy = Policies.User)]
    [ProducesResponseType(typeof(ProductView), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create([FromBody] CreateProductModel model)
    {
        var command = new CreateProductCommand(UserId, model.Name, model.Category, model.Article, model.Price);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{productId}")]
    [Authorize(Policy = Policies.AdminOrManager)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete([FromRoute] Guid productId)
    {
        await _mediator.Send(new DeleteProductCommand(UserId, productId));
        return NoContent();
    }
}