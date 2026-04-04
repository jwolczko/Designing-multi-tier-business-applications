using Fortuna.Domain.Abstractions;

namespace Fortuna.Domain.Accounts;

public sealed class TransactionEntry : Entity<TransactionId>
{
    private TransactionEntry()
    {
    }

    private TransactionEntry(
        TransactionId id,
        BankAccountId bankAccountId,
        TransactionType type,
        Money amount,
        string title,
        DateTime bookedAtUtc,
        Guid? transferId) : base(id)
    {
        BankAccountId = bankAccountId;
        Type = type;
        Amount = amount;
        Title = title;
        BookedAtUtc = bookedAtUtc;
        TransferId = transferId;
    }

    public BankAccountId BankAccountId { get; private set; } = default!;
    public TransactionType Type { get; private set; }
    public Money Amount { get; private set; } = default!;
    public string Title { get; private set; } = default!;
    public DateTime BookedAtUtc { get; private set; }
    public Guid? TransferId { get; private set; }

    public static TransactionEntry CreateCredit(BankAccountId bankAccountId, Money amount, string title, Guid? transferId = null)
        => new(TransactionId.New(), bankAccountId, TransactionType.Credit, amount, title, DateTime.UtcNow, transferId);

    public static TransactionEntry CreateDebit(BankAccountId bankAccountId, Money amount, string title, Guid? transferId = null)
        => new(TransactionId.New(), bankAccountId, TransactionType.Debit, amount, title, DateTime.UtcNow, transferId);
}
