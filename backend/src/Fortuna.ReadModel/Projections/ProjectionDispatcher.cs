using System.Text.Json;
using Fortuna.Domain.Accounts.Events;
using Fortuna.Domain.Transfers.Events;
using Fortuna.Infrastructure.Persistence.Write;
using Fortuna.Infrastructure.Projections.Interfaces;
using Fortuna.ReadModel.Dashboard.ReadModels;
using Fortuna.ReadModel.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fortuna.ReadModel.Projections;

public sealed class ProjectionDispatcher : IReadModelProjector
{
    private readonly ReadDbContext _dbContext;

    public ProjectionDispatcher(ReadDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ProjectAsync(OutboxMessage message, CancellationToken cancellationToken)
    {
        var alreadyProcessed = await _dbContext.ProcessedOutboxMessages
            .AsNoTracking()
            .AnyAsync(x => x.OutboxMessageId == message.Id, cancellationToken);

        if (alreadyProcessed)
        {
            return;
        }

        if (IsEventType<BankAccountOpenedDomainEvent>(message.Type))
        {
            await ProjectBankAccountOpenedAsync(message, cancellationToken);
        }
        else if (IsEventType<MoneyDepositedDomainEvent>(message.Type))
        {
            await ProjectMoneyDepositedAsync(message, cancellationToken);
        }
        else if (IsEventType<MoneyWithdrawnDomainEvent>(message.Type))
        {
            await ProjectMoneyWithdrawnAsync(message, cancellationToken);
        }

        _dbContext.ProcessedOutboxMessages.Add(new ProcessedOutboxMessageReadModel
        {
            OutboxMessageId = message.Id,
            ProcessedAtUtc = DateTime.UtcNow
        });

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task ProjectBankAccountOpenedAsync(OutboxMessage message, CancellationToken cancellationToken)
    {
        var domainEvent = Deserialize<BankAccountOpenedDomainEvent>(message.Payload);

        var productTile = await _dbContext.ProductTiles.FindAsync([domainEvent.AccountId], cancellationToken);
        if (productTile is null)
        {
            productTile = new ProductTileReadModel
            {
                AccountId = domainEvent.AccountId
            };

            _dbContext.ProductTiles.Add(productTile);
        }

        productTile.CustomerId = domainEvent.CustomerId;
        productTile.AccountName = domainEvent.AccountName;
        productTile.AccountNumber = domainEvent.AccountNumber;
        productTile.Balance = domainEvent.Balance;
        productTile.Currency = domainEvent.Currency;
    }

    private async Task ProjectMoneyDepositedAsync(OutboxMessage message, CancellationToken cancellationToken)
    {
        var domainEvent = Deserialize<MoneyDepositedDomainEvent>(message.Payload);
        var productTile = await GetRequiredProductTileAsync(domainEvent.AccountId, cancellationToken);

        productTile.Balance += domainEvent.Amount;

        _dbContext.TimelineEvents.Add(new TimelineEventReadModel
        {
            Id = message.Id,
            CustomerId = domainEvent.CustomerId,
            AccountId = domainEvent.AccountId,
            EventDateUtc = domainEvent.OccurredOnUtc,
            EventType = "deposit",
            Title = domainEvent.Title,
            Amount = domainEvent.Amount,
            Currency = domainEvent.Currency,
            IsPositive = true
        });
    }

    private async Task ProjectMoneyWithdrawnAsync(OutboxMessage message, CancellationToken cancellationToken)
    {
        var domainEvent = Deserialize<MoneyWithdrawnDomainEvent>(message.Payload);
        var productTile = await GetRequiredProductTileAsync(domainEvent.AccountId, cancellationToken);

        productTile.Balance -= domainEvent.Amount;

        _dbContext.TimelineEvents.Add(new TimelineEventReadModel
        {
            Id = message.Id,
            CustomerId = domainEvent.CustomerId,
            AccountId = domainEvent.AccountId,
            EventDateUtc = domainEvent.OccurredOnUtc,
            EventType = "withdrawal",
            Title = domainEvent.Title,
            Amount = domainEvent.Amount,
            Currency = domainEvent.Currency,
            IsPositive = false
        });
    }

    private async Task<ProductTileReadModel> GetRequiredProductTileAsync(Guid accountId, CancellationToken cancellationToken)
    {
        var productTile = await _dbContext.ProductTiles.FindAsync([accountId], cancellationToken);

        return productTile ?? throw new InvalidOperationException($"Product tile for account '{accountId}' was not found.");
    }

    private static bool IsEventType<TEvent>(string typeName)
        => string.Equals(typeName, typeof(TEvent).FullName, StringComparison.Ordinal)
            || string.Equals(typeName, typeof(TEvent).Name, StringComparison.Ordinal);

    private static TEvent Deserialize<TEvent>(string payload)
        => JsonSerializer.Deserialize<TEvent>(payload)
            ?? throw new InvalidOperationException($"Unable to deserialize event '{typeof(TEvent).Name}'.");
}
