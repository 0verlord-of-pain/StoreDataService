using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreDataService.Domain.Entities;

namespace StoreDataService.Storage.Mapping;

public sealed class TransactionMap : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transaction");
        builder.HasKey(item => item.Id);

        builder
            .HasIndex(item => item.Id)
            .IsUnique();

        builder
            .HasOne(item => item.User)
            .WithMany(item => item.Transactions)
            .HasForeignKey(item => item.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}