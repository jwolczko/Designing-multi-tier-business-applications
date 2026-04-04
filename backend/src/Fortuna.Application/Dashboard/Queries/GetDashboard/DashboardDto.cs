namespace Fortuna.Application.Dashboard.Queries.GetDashboard;

public sealed record ProductTileDto(
    Guid AccountId,
    string AccountName,
    string AccountNumber,
    decimal Balance,
    string Currency);

public sealed record TimelineEventDto(
    Guid Id,
    DateTime EventDateUtc,
    string EventType,
    string Title,
    decimal Amount,
    string Currency,
    bool IsPositive);

public sealed record DashboardDto(
    Guid CustomerId,
    decimal TotalBalance,
    string Currency,
    IReadOnlyCollection<ProductTileDto> Products,
    IReadOnlyCollection<TimelineEventDto> Events);
