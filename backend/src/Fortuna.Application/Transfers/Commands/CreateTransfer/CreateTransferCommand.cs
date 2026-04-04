using Fortuna.Application.Abstractions.Messaging;

namespace Fortuna.Application.Transfers.Commands.CreateTransfer;

public sealed record CreateTransferCommand(
    Guid SourceAccountId,
    Guid TargetAccountId,
    decimal Amount,
    string Currency,
    string Title) : ICommand<Guid>;
