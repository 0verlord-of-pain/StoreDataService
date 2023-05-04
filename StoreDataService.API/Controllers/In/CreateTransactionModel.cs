namespace StoreDataService.API.Controllers.In;

public class CreateTransactionModel
{
    public Guid UserId { get; set; }
    public Dictionary<Guid, int> Products { get; set; }
}