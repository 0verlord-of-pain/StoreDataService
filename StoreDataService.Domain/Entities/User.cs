using Microsoft.AspNetCore.Identity;

namespace StoreDataService.Domain.Entities;

public class User : IdentityUser<Guid>, IBaseEntity
{
    public string Surname { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string FatherName { get; set; } = string.Empty;
    public DateTime Birthday { get; set; }
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? UpdatedOnUtc { get; set; }
    public bool IsArchived { get; set; }

    public void SoftDelete()
    {
        IsArchived = true;
    }

    public void Restore()
    {
        IsArchived = false;
    }

    public static User Create(string email, string surname, string name,
        string fatherName, DateTime birthday)
    {
        return new User
        {
            UserName = email,
            Email = email,
            Surname = surname,
            Name = name,
            FatherName = fatherName,
            Birthday = birthday
        };
    }
}