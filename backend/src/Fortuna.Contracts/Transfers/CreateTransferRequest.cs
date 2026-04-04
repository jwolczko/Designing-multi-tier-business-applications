namespace Fortuna.Contracts.Transfers;

public sealed record CreateTransferRequest(
    Guid SourceAccountId,
    Guid TargetAccountId,
    decimal Amount,
    string Currency,
    string Title);
