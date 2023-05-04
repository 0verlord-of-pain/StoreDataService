using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreDataService.Domain.Entities;

namespace StoreDataService.Storage.Mapping;

public sealed class ProductMap : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Product");
        builder.HasKey(item => item.Id);

        builder
            .HasIndex(item => item.Id)
            .IsUnique();
    }
}