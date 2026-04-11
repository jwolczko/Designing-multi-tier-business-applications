using Fortuna.Application.Abstractions.Messaging;
using Fortuna.Application.Abstractions.Persistence;
using Fortuna.Application.Common.Exceptions;
using Fortuna.Domain.Accounts;
using Fortuna.Domain.Accounts.Repositories;

namespace Fortuna.Application.Accounts.Commands.DepositMoney;

public sealed class DepositMoneyCommandHandler : ICommandHandler<DepositMoneyCommand, Guid>
{
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DepositMoneyCommandHandler(IBankAccountRepository bankAccountRepository, IUnitOfWork unitOfWork)
    {
        _bankAccountRepository = bankAccountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(DepositMoneyCommand command, CancellationToken cancellationToken)
    {
        var account = await _bankAccountRepository.GetByIdAsync(command.AccountId, cancellationToken);
        if (account is null || account.CustomerId.Value != command.CustomerId)
            throw new NotFoundException("Bank account not found.");

        account.Deposit(new Money(command.Amount, command.Currency), command.Title);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return account.Id;
    }
}
