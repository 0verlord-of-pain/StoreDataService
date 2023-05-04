using MediatR.Pipeline;
using StoreDataService.Core.Exceptions;

namespace StoreDataService.Application.CQRS.Products.Queries.GetProductByCategory;

public sealed class GetProductByCategoryPreProcessor : IRequestPreProcessor<GetProductByCategoryQuery>
{
    public Task Process(GetProductByCategoryQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Category)) throw new ValidationException("The Category field cannot be empty");

        if (request.Page < 1) throw new ValidationException("Page number cannot be less than 1");
        return Task.CompletedTask;
    }
}