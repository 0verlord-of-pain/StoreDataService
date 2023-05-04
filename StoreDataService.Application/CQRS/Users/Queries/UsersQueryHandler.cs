using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreDataService.Application.CQRS.Users.Queries.GetAll;
using StoreDataService.Application.CQRS.Users.Queries.GetBirthdayPeople;
using StoreDataService.Application.CQRS.Users.Queries.GetUser;
using StoreDataService.Application.CQRS.Users.Queries.GetUserAndCategory;
using StoreDataService.Application.CQRS.Users.Queries.GetUserByLastPayment;
using StoreDataService.Application.CQRS.Users.Queries.Views;
using StoreDataService.Storage.Persistence;

namespace StoreDataService.Application.CQRS.Users.Queries;

public sealed class UsersQueryHandler :
    IRequestHandler<GetAllUsersQuery, IEnumerable<UserView>>,
    IRequestHandler<GetUserQuery, UserView>,
    IRequestHandler<GetBirthdayPeopleQuery, IEnumerable<UserView>>,
    IRequestHandler<GetUserByLastPaymentQuery, IEnumerable<UserViewAndLastPaymentDate>>,
    IRequestHandler<GetUserAndCategoryQuery, Dictionary<string, int>>

{
    private const int limit = 10;
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public UsersQueryHandler(IMapper mapper, DataContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<IEnumerable<UserView>> Handle(
        GetAllUsersQuery request,
        CancellationToken cancellationToken)
    {
        if (request.Page <= 0) throw new ValidationException("Page is too small");

        var users = await _context.Users
            .Skip(limit * (request.Page - 1))
            .Take(limit)
            .ToListAsync(cancellationToken);

        var usersViews = _mapper.Map<IEnumerable<UserView>>(users);
        return usersViews;
    }

    public async Task<IEnumerable<UserView>> Handle(GetBirthdayPeopleQuery request, CancellationToken cancellationToken)
    {
        var users = await _context.Users
            .Where(i => i.Birthday == request.Birthday)
            .ToArrayAsync(cancellationToken);

        var usersViews = _mapper.Map<IEnumerable<UserView>>(users);
        return usersViews;
    }

    public async Task<Dictionary<string, int>> Handle(GetUserAndCategoryQuery request,
        CancellationToken cancellationToken)
    {
        var transactions = await _context.Transactions
            .Where(i => i.UserId == request.UserId)
            .ToArrayAsync(cancellationToken);

        var dictionary = new Dictionary<Guid, int>();

        foreach (var transaction in transactions)
        foreach (var product in transaction.Products)
            if (!dictionary.TryAdd(product.Key, product.Value))
                dictionary[product.Key] += product.Value;

        var categories = await _context.Products
            .Where(i => dictionary.ContainsKey(i.Id))
            .Select(i => new { i.Category, Count = dictionary[i.Id] })
            .ToArrayAsync(cancellationToken);

        var result = new Dictionary<string, int>();

        foreach (var category in categories)
            if (!result.TryAdd(category.Category, category.Count))
                result[category.Category] += category.Count;

        return result;
    }

    public async Task<IEnumerable<UserViewAndLastPaymentDate>> Handle(GetUserByLastPaymentQuery request,
        CancellationToken cancellationToken)
    {
        var dateTimeUtcNow = DateTime.UtcNow;
        var date = new DateTime(dateTimeUtcNow.Year, dateTimeUtcNow.Month, dateTimeUtcNow.Day - request.LastPayment);
        var users = await _context.Users
            .Where(i => i.Transactions.LastOrDefault()!.CreatedOnUtc >= date)
            .ToArrayAsync(cancellationToken);

        var usersViews = _mapper.Map<IEnumerable<UserViewAndLastPaymentDate>>(users);
        return usersViews;
    }

    public async Task<UserView> Handle(
        GetUserQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(i => i.Id == request.UserId, cancellationToken);

        var view = _mapper.Map<UserView>(user);

        return view;
    }
}