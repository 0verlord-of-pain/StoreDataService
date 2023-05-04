using AutoMapper;
using StoreDataService.Application.CQRS.Products.Queries.Views;
using StoreDataService.Application.CQRS.Users.Queries.Views;
using StoreDataService.Domain.Entities;

namespace StoreDataService.Application.Mapper;

public sealed class RegisterViews : Profile
{
    public RegisterViews()
    {
        CreateMap<User, UserView>();
        CreateMap<User, UserViewAndLastPaymentDate>()
            .ForMember(dest => dest.LastPaymentDate,
                dest => dest
                    .MapFrom(src => src.Transactions.Last().CreatedOnUtc));

        CreateMap<Transaction, ProductView>();

        CreateMap<Product, ProductView>();
    }
}