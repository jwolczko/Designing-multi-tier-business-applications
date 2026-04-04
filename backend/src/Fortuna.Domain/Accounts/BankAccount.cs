using Fortuna.Domain.Abstractions;
using Fortuna.Domain.Accounts.Events;
using Fortuna.Domain.Customers;

namespace Fortuna.Domain.Accounts;

public sealed class BankAccount : Entity<BankAccountId>, IAggregateRoot
{
    private readonly List<TransactionEntry> _transactions = [];

    private BankAccount()
    {
    }

    private BankAccount(
        BankAccountId id,
        CustomerId customerId,
        AccountNumber accountNumber,
        string accountName,
        string currency) : base(id)
    {
        CustomerId = customerId;
        AccountNumber = accountNumber;
        AccountName = accountName;
        Currency = currency;
        Balance = Money.Zero(currency);
        Status = AccountStatus.Active;
        CreatedAtUtc = DateTime.UtcNow;

        AddDomainEvent(new BankAccountOpenedDomainEvent(
            id.Value,
            customerId.Value,
            accountNumber.Value,
            Balance.Amount,
            currency));
    }

    public CustomerId CustomerId { get; private set; } = default!;
    public AccountNumber AccountNumber { get; private set; } = default!;
    public string AccountName { get; private set; } = default!;
    public string Currency { get; private set; } = default!;
    public Money Balance { get; private set; } = default!;
    public AccountStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public IReadOnlyCollection<TransactionEntry> Transactions => _transactions;

    public static BankAccount Open(CustomerId customerId, AccountNumber accountNumber, string accountName, string currency)
        => new(BankAccountId.New(), customerId, accountNumber, accountName, currency);

    public void Deposit(Money amount, string title, Guid? transferId = null)
    {
        EnsureActive();
        EnsurePositive(amount);

        Balance = Balance.Add(amount);
        _transactions.Add(TransactionEntry.CreateCredit(Id, amount, title, transferId));

        AddDomainEvent(new MoneyDepositedDomainEvent(
            Id.Value,
            CustomerId.Value,
            amount.Amount,
            amount.Currency,
            title));
    }

    public void Withdraw(Money amount, string title, Guid? transferId = null)
    {
        EnsureActive();
        EnsurePositive(amount);

        if (Balance.Amount < amount.Amount)
            throw new DomainException("Insufficient funds.");

        Balance = Balance.Subtract(amount);
        _transactions.Add(TransactionEntry.CreateDebit(Id, amount, title, transferId));

        AddDomainEvent(new MoneyWithdrawnDomainEvent(
            Id.Value,
            CustomerId.Value,
            amount.Amount,
            amount.Currency,
            title));
    }

    private void EnsureActive()
    {
        if (Status != AccountStatus.Active)
            throw new DomainException("Account is not active.");
    }

    private static void EnsurePositive(Money amount)
    {
        if (amount.Amount <= 0)
            throw new DomainException("Amount must be greater than zero.");
    }
}
