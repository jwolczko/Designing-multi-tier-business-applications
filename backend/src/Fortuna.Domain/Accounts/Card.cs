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
        CardType cardType) : base(
        id,
        customerId,
        productName,
        productNumber,
        currency,
        ProductCategory.Card,
        ProductStatus.Active)
    {
        CardType = cardType;
        RaiseCreatedDomainEvent(cardType.ToString());
    }

    public CardType CardType { get; private set; }

    public static Card Create(
        CustomerId customerId,
        string productName,
        string productNumber,
        string currency,
        CardType cardType)
        => new(Guid.NewGuid(), customerId, productName, productNumber, currency, cardType);
}
