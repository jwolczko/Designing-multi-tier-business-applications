using Fortuna.Domain.Abstractions;
using Fortuna.Domain.Accounts;
using Fortuna.Domain.Transfers.Events;

namespace Fortuna.Domain.Transfers;

public sealed class Transfer : Entity<TransferId>, IAggregateRoot
{
    private Transfer()
    {
    }

    private Transfer(
        TransferId id,
        BankAccountId sourceAccountId,
        BankAccountId targetAccountId,
        Money amount,
        string title) : base(id)
    {
        SourceAccountId = sourceAccountId;
        TargetAccountId = targetAccountId;
        Amount = amount;
        Title = title;
        Status = TransferStatus.Pending;
        CreatedAtUtc = DateTime.UtcNow;

        AddDomainEvent(new TransferCreatedDomainEvent(
            id.Value,
            sourceAccountId.Value,
            targetAccountId.Value,
            amount.Amount,
            amount.Currency,
            title));
    }

    public BankAccountId SourceAccountId { get; private set; } = default!;
    public BankAccountId TargetAccountId { get; private set; } = default!;
    public Money Amount { get; private set; } = default!;
    public string Title { get; private set; } = default!;
    public TransferStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? CompletedAtUtc { get; private set; }

    public static Transfer Create(BankAccountId sourceAccountId, BankAccountId targetAccountId, Money amount, string title)
        => new(TransferId.New(), sourceAccountId, targetAccountId, amount, title);

    public void MarkCompleted()
    {
        Status = TransferStatus.Completed;
        CompletedAtUtc = DateTime.UtcNow;

        AddDomainEvent(new TransferCompletedDomainEvent(Id.Value));
    }
}
