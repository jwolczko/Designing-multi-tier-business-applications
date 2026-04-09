using Fortuna.Domain.Abstractions;
using Fortuna.Domain.Customers;

namespace Fortuna.Domain.Accounts.Events;

public sealed record BankAccountOpenedDomainEvent(
    Guid AccountId,
    Guid CustomerId,
    string AccountNumber,
    string AccountName,
    decimal Balance,
    string Currency) : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}
