using MediatR;
using StoreDataService.Application.CQRS.Users.Queries.Views;

namespace StoreDataService.Application.CQRS.Users.Queries.GetUserByLastPayment;

public class GetUserByLastPaymentQuery : IRequest<IEnumerable<UserViewAndLastPaymentDate>>
{
    public GetUserByLastPaymentQuery(int lastPayment)
    {
        LastPayment = lastPayment;
    }

    public int LastPayment { get; init; }
}