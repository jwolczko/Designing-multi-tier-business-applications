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
        Guid sourceAccountId,
        Guid targetAccountId,
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
            sourceAccountId,
            targetAccountId,
            amount.Amount,
            amount.Currency,
            title));
    }

    public Guid SourceAccountId { get; private set; }
    public Guid TargetAccountId { get; private set; }
    public Money Amount { get; private set; } = default!;
    public string Title { get; private set; } = default!;
    public TransferStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? CompletedAtUtc { get; private set; }

    public static Transfer Create(Guid sourceAccountId, Guid targetAccountId, Money amount, string title)
        => new(TransferId.New(), sourceAccountId, targetAccountId, amount, title);

    public void MarkCompleted()
    {
        Status = TransferStatus.Completed;
        CompletedAtUtc = DateTime.UtcNow;

        AddDomainEvent(new TransferCompletedDomainEvent(Id.Value));
    }
}
