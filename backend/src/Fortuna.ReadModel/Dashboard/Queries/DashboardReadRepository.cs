using Dapper;
using Fortuna.Application.Abstractions.Persistence;
using Fortuna.Application.Dashboard.Queries.GetDashboard;

namespace Fortuna.ReadModel.Dashboard.Queries;

public sealed class DashboardReadRepository : IDashboardReadRepository
{
    private readonly IReadDbConnectionFactory _connectionFactory;

    public DashboardReadRepository(IReadDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<DashboardDto> GetDashboardAsync(Guid customerId, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();

        var products = (await connection.QueryAsync<ProductTileDto>(
            @"SELECT AccountId, AccountName, AccountNumber, Balance, Currency
              FROM [read].[ProductTile]
              WHERE CustomerId = @customerId
              ORDER BY AccountName;",
            new { customerId }))
            .AsList();

        var events = (await connection.QueryAsync<TimelineEventDto>(
            @"SELECT TOP 20 Id, EventDateUtc, EventType, Title, Amount, Currency, IsPositive
              FROM [read].[TimelineEvent]
              WHERE CustomerId = @customerId
              ORDER BY EventDateUtc DESC;",
            new { customerId }))
            .AsList();

        var totalBalance = products.Sum(x => x.Balance);
        var currency = products.FirstOrDefault()?.Currency ?? "PLN";

        return new DashboardDto(customerId, totalBalance, currency, products, events);
    }
}
