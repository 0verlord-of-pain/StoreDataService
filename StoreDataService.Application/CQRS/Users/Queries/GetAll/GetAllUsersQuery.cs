using MediatR;
using StoreDataService.Application.CQRS.Users.Queries.Views;

namespace StoreDataService.Application.CQRS.Users.Queries.GetAll;

public class GetAllUsersQuery : IRequest<IEnumerable<UserView>>
{
    public GetAllUsersQuery(int page)
    {
        Page = page;
    }

    public int Page { get; init; }
}