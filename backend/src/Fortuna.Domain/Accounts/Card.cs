using Fortuna.Domain.Abstractions;
using Fortuna.Domain.Customers;
using Fortuna.Domain.Products;

namespace Fortuna.Domain.Cards;

public sealed class Card : Product, IAggregateRoot
{
    private Card()
    {
    }

    private Card(
        Guid id,
        CustomerId customerId,
        string productName,
        string productNumber,
        string currency,
        CardType cardType,
        decimal? creditLimit) : base(
        id,
        customerId,
        productName,
        productNumber,
        currency,
        ProductCategory.Card,
        ProductStatus.Active)
    {
        if (cardType == CardType.Credit && (!creditLimit.HasValue || creditLimit.Value <= 0))
            throw new DomainException("Credit card limit must be greater than zero.");

        if (cardType == CardType.Debit && creditLimit.HasValue)
            throw new DomainException("Debit card cannot have a credit limit.");

        CardType = cardType;
        CreditLimit = creditLimit;
        RaiseCreatedDomainEvent(cardType.ToString());
    }

    public CardType CardType { get; private set; }
    public decimal? CreditLimit { get; private set; }

    public static Card Create(
        CustomerId customerId,
        string productName,
        string productNumber,
        string currency,
        CardType cardType,
        decimal? creditLimit = null)
        => new(Guid.NewGuid(), customerId, productName, productNumber, currency, cardType, creditLimit);
}
