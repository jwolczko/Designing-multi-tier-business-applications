using System.Text.Json;
using FluentAssertions;
using Fortuna.Domain.Accounts.Events;
using Fortuna.Infrastructure.Persistence.Write;
using Fortuna.ReadModel.Projections;
using Fortuna.ReadModel.Persistence;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Fortuna.UnitTests.ReadModel;

public sealed class ProjectionDispatcherTests
{
    [Fact]
    public async Task ProjectAsync_Should_Project_Opened_Account_Only_Once_For_Duplicate_Message()
    {
        await using var dbContext = CreateDbContext();
        var sut = new ProjectionDispatcher(dbContext);
        var accountId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var domainEvent = new BankAccountOpenedDomainEvent(accountId, customerId, "PL001234", "Main account", 0m, "PLN");
        var message = CreateOutboxMessage(Guid.NewGuid(), domainEvent);

        await sut.ProjectAsync(message, CancellationToken.None);
        await sut.ProjectAsync(message, CancellationToken.None);

        dbContext.ProductTiles.Should().ContainSingle();
        dbContext.ProcessedOutboxMessages.Should().ContainSingle();

        var productTile = await dbContext.ProductTiles.SingleAsync();
        productTile.AccountName.Should().Be("Main account");
        productTile.AccountNumber.Should().Be("PL001234");
    }

    [Fact]
    public async Task ProjectAsync_Should_Update_Balance_And_Create_Timeline_Entries()
    {
        await using var dbContext = CreateDbContext();
        var sut = new ProjectionDispatcher(dbContext);
        var accountId = Guid.NewGuid();
        var customerId = Guid.NewGuid();

        await sut.ProjectAsync(
            CreateOutboxMessage(
                Guid.NewGuid(),
                new BankAccountOpenedDomainEvent(accountId, customerId, "PL009999", "Savings", 100m, "PLN")),
            CancellationToken.None);

        await sut.ProjectAsync(
            CreateOutboxMessage(
                Guid.NewGuid(),
                new MoneyDepositedDomainEvent(accountId, customerId, 25m, "PLN", "Salary")),
            CancellationToken.None);

        await sut.ProjectAsync(
            CreateOutboxMessage(
                Guid.NewGuid(),
                new MoneyWithdrawnDomainEvent(accountId, customerId, 10m, "PLN", "Groceries")),
            CancellationToken.None);

        var productTile = await dbContext.ProductTiles.SingleAsync();
        productTile.Balance.Should().Be(115m);

        var timelineEvents = await dbContext.TimelineEvents
            .OrderBy(x => x.EventDateUtc)
            .ToListAsync();

        timelineEvents.Should().HaveCount(2);
        timelineEvents[0].EventType.Should().Be("deposit");
        timelineEvents[0].IsPositive.Should().BeTrue();
        timelineEvents[1].EventType.Should().Be("withdrawal");
        timelineEvents[1].IsPositive.Should().BeFalse();
    }

    private static ReadDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ReadDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ReadDbContext(options);
    }

    private static OutboxMessage CreateOutboxMessage<TEvent>(Guid messageId, TEvent domainEvent)
        where TEvent : class
        => new()
        {
            Id = messageId,
            Type = typeof(TEvent).FullName ?? typeof(TEvent).Name,
            Payload = JsonSerializer.Serialize(domainEvent),
            OccurredOnUtc = DateTime.UtcNow
        };
}
