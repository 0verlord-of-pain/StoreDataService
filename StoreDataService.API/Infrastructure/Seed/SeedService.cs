using Microsoft.AspNetCore.Identity;
using StoreDataService.Core.Enums;
using StoreDataService.Domain.Entities;
using StoreDataService.Storage.Persistence;

namespace StoreDataService.API.Infrastructure.Seed;

internal sealed class SeedService : ISeedService
{
    private readonly DataContext _context;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly UserManager<User> _userManager;

    public SeedService(
        RoleManager<IdentityRole<Guid>> roleManager,
        UserManager<User> userManager,
        DataContext context)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _context = context;
    }

    public async Task SeedRolesAsync()
    {
        await _roleManager.CreateAsync(new IdentityRole<Guid>(Roles.Admin.ToString()));
        await _roleManager.CreateAsync(new IdentityRole<Guid>(Roles.Manager.ToString()));
        await _roleManager.CreateAsync(new IdentityRole<Guid>(Roles.User.ToString()));
    }

    public async Task SeedAdminAndManagerAsync()
    {
        var defaultAdmin = new User
        {
            UserName = "admin",
            Email = "admin@gmail.com",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };

        if (_userManager.Users.All(u => u.Email != defaultAdmin.Email))
        {
            var user = await _userManager.FindByEmailAsync(defaultAdmin.Email);
            if (user is null)
            {
                await _userManager.CreateAsync(defaultAdmin, "admin1");
                await _userManager.AddToRoleAsync(defaultAdmin, Roles.Admin.ToString());
                await _userManager.AddToRoleAsync(defaultAdmin, Roles.Manager.ToString());
                await _userManager.AddToRoleAsync(defaultAdmin, Roles.User.ToString());
            }
        }

        var defaultManager = new User
        {
            UserName = "manager",
            Email = "manager@gmail.com",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };

        if (_userManager.Users.All(u => u.Email != defaultManager.Email))
        {
            var user = await _userManager.FindByEmailAsync(defaultManager.Email);
            if (user is null)
            {
                await _userManager.CreateAsync(defaultManager, "manager1");
                await _userManager.AddToRoleAsync(defaultManager, Roles.Manager.ToString());
                await _userManager.AddToRoleAsync(defaultManager, Roles.User.ToString());
            }
        }
    }

    public async Task SeedDefaultUserAsync()
    {
        var defaultUser = new User
        {
            UserName = "Test",
            Email = "test@gmail.com",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };

        if (_userManager.Users.All(u => u.Email != defaultUser.Email))
        {
            var user = await _userManager.FindByEmailAsync(defaultUser.Email);
            if (user is null)
            {
                await _userManager.CreateAsync(defaultUser, "test12");
                await _userManager.AddToRoleAsync(defaultUser, Roles.User.ToString());
            }
        }
    }
}