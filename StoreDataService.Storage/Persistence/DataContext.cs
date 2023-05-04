using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StoreDataService.Domain.Entities;
using StoreDataService.Storage.Extensions;

namespace StoreDataService.Storage.Persistence;

public class DataContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public DataContext(
        DbContextOptions<DataContext> options)
        : base(options)
    {
        Database.Migrate();
    }

    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Product> Products { get; set; }

    public override async Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        this.UpdateSystemDates();
        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasQueryFilter(i => !i.IsArchived);
        modelBuilder.Entity<Transaction>().HasQueryFilter(i => !i.IsArchived);
        modelBuilder.Entity<User>().HasQueryFilter(i => !i.IsArchived);

        base.OnModelCreating(modelBuilder);
    }
}