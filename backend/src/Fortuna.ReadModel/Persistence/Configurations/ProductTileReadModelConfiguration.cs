using Fortuna.ReadModel.Dashboard.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fortuna.ReadModel.Persistence.Configurations;

public sealed class ProductTileReadModelConfiguration : IEntityTypeConfiguration<ProductTileReadModel>
{
    public void Configure(EntityTypeBuilder<ProductTileReadModel> builder)
    {
        builder.ToTable("ProductTile", "read");
        builder.HasKey(x => x.AccountId);

        builder.Property(x => x.AccountId).ValueGeneratedNever();
        builder.Property(x => x.CustomerId).IsRequired();
        builder.Property(x => x.AccountName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.AccountNumber).HasMaxLength(34).IsRequired();
        builder.Property(x => x.Balance).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(x => x.Currency).HasMaxLength(3).IsRequired();

        builder.HasIndex(x => x.CustomerId);
    }
}
