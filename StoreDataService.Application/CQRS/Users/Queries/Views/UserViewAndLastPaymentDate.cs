namespace StoreDataService.Application.CQRS.Users.Queries.Views;

public class UserViewAndLastPaymentDate : UserView
{
    public DateTime LastPaymentDate { get; set; }
}