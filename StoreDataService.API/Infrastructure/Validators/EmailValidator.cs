using System.ComponentModel.DataAnnotations;

namespace StoreDataService.API.Infrastructure.Validators;

public struct EmailValidator
{
    public static bool IsValid(string email)
    {
        return new EmailAddressAttribute().IsValid(email);
    }
}