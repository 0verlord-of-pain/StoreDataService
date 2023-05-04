using MediatR.Pipeline;
using StoreDataService.Core.Exceptions;

namespace StoreDataService.Application.CQRS.Users.Queries.GetUserByLastPayment;

public sealed class GetUserByLastPaymentPreProcessor : IRequestPreProcessor<GetUserByLastPaymentQuery>
{
    public Task Process(GetUserByLastPaymentQuery request, CancellationToken cancellationToken)
    {
        if (request.LastPayment < 0) throw new NotFoundException("Last payment day cannot be less than 0");

        return Task.CompletedTask;
    }
}