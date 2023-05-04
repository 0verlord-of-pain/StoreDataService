using MediatR;
using StoreDataService.Application.CQRS.Users.Queries.Views;

namespace StoreDataService.Application.CQRS.Users.Queries.GetBirthdayPeople;

public class GetBirthdayPeopleQuery : IRequest<IEnumerable<UserView>>
{
    public GetBirthdayPeopleQuery(DateTime birthday)
    {
        Birthday = birthday;
    }

    public DateTime Birthday { get; init; }
}