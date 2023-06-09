﻿namespace StoreDataService.API.Controllers.In;

public class CreateUserModel
{
    public string Email { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string FatherName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime Birthday { get; set; }
}