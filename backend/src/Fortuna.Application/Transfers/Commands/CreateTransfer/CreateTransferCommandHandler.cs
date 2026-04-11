using Fortuna.Application.Abstractions.Messaging;
using Fortuna.Application.Abstractions.Persistence;
using Fortuna.Application.Common.Exceptions;
using Fortuna.Domain.Accounts;
using Fortuna.Domain.Accounts.Repositories;
using Fortuna.Domain.Transfers;
using Fortuna.Domain.Transfers.Repositories;

namespace Fortuna.Application.Transfers.Commands.CreateTransfer;

public sealed class CreateTransferCommandHandler : ICommandHandler<CreateTransferCommand, Guid>
{
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly ITransferRepository _transferRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTransferCommandHandler(
        IBankAccountRepository bankAccountRepository,
        ITransferRepository transferRepository,
        IUnitOfWork unitOfWork)
    {
        _bankAccountRepository = bankAccountRepository;
        _transferRepository = transferRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateTransferCommand command, CancellationToken cancellationToken)
    {
        if (command.SourceAccountId == command.TargetAccountId)
            throw new ValidationException("Source and target account must be different.");

        var source = await _bankAccountRepository.GetByIdAsync(command.SourceAccountId, cancellationToken);
        var target = await _bankAccountRepository.GetByIdAsync(command.TargetAccountId, cancellationToken);

        if (source is null || target is null || source.CustomerId.Value != command.CustomerId)
            throw new NotFoundException("Source or target account not found.");

        var amount = new Money(command.Amount, command.Currency);
        var transfer = Transfer.Create(source.Id, target.Id, amount, command.Title);

        source.Withdraw(amount, command.Title, transfer.Id.Value);
        target.Deposit(amount, command.Title, transfer.Id.Value);
        transfer.MarkCompleted();

        await _transferRepository.AddAsync(transfer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return transfer.Id.Value;
    }
}
