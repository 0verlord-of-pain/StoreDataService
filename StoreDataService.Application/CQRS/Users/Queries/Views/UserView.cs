namespace StoreDataService.Application.CQRS.Users.Queries.Views;

public class UserView
{
    public Guid Id { get; set; }
    public string Surname { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string FatherName { get; set; } = string.Empty;
}