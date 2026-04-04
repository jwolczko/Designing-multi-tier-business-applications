using Fortuna.Domain.Abstractions;

namespace Fortuna.Domain.Customers;

public sealed class Customer : Entity<CustomerId>, IAggregateRoot
{
    private Customer()
    {
    }

    public Customer(CustomerId id, FullName fullName, Email email) : base(id)
    {
        FullName = fullName;
        Email = email;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public FullName FullName { get; private set; } = default!;
    public Email Email { get; private set; } = default!;
    public DateTime CreatedAtUtc { get; private set; }
}
