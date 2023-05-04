using MediatR;

namespace StoreDataService.Application.CQRS.Users.Queries.GetUserAndCategory;

public class GetUserAndCategoryQuery : IRequest<Dictionary<string, int>>
{
    public GetUserAndCategoryQuery(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; init; }
}