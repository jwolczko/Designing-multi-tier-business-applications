using Fortuna.Domain.Accounts;
using Fortuna.Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fortuna.Infrastructure.Persistence.Write.Configurations;

public sealed class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
{
    public void Configure(EntityTypeBuilder<BankAccount> builder)
    {
        builder.ToTable("BankAccounts", "dbo");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, value => new BankAccountId(value));

        builder.Property(x => x.CustomerId)
            .HasConversion(x => x.Value, value => new CustomerId(value));

        builder.OwnsOne(x => x.AccountNumber, owned =>
        {
            owned.Property(p => p.Value).HasColumnName("AccountNumber").HasMaxLength(34).IsRequired();
        });

        builder.OwnsOne(x => x.Balance, owned =>
        {
            owned.Property(p => p.Amount).HasColumnName("Balance").HasColumnType("decimal(18,2)").IsRequired();
            owned.Property(p => p.Currency).HasColumnName("Currency").HasMaxLength(3).IsRequired();
        });

        builder.Property(x => x.AccountName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Currency).HasMaxLength(3).IsRequired();
        builder.Property(x => x.Status).IsRequired();
        builder.Property(x => x.CreatedAtUtc).IsRequired();

        builder.HasMany(x => x.Transactions)
            .WithOne()
            .HasForeignKey(x => x.BankAccountId);

        builder.Metadata.FindNavigation(nameof(BankAccount.Transactions))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
