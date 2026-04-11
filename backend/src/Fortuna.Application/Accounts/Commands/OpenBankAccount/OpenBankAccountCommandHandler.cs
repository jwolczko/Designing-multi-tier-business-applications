using Fortuna.Application.Abstractions.Messaging;
using Fortuna.Application.Abstractions.Persistence;
using Fortuna.Application.Common.Exceptions;
using Fortuna.Domain.Accounts;
using Fortuna.Domain.Accounts.Repositories;
using Fortuna.Domain.Customers;
using Fortuna.Domain.Customers.Repositories;

namespace Fortuna.Application.Accounts.Commands.OpenBankAccount;

public sealed class OpenBankAccountCommandHandler : ICommandHandler<OpenBankAccountCommand, Guid>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OpenBankAccountCommandHandler(
        ICustomerRepository customerRepository,
        IBankAccountRepository bankAccountRepository,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _bankAccountRepository = bankAccountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(OpenBankAccountCommand command, CancellationToken cancellationToken)
    {
        var customerId = new CustomerId(command.CustomerId);
        var customer = await _customerRepository.GetByIdAsync(customerId, cancellationToken);
        if (customer is null)
            throw new NotFoundException("Customer not found.");

        var bankAccount = BankAccount.Open(
            customerId,
            new AccountNumber(command.AccountNumber),
            command.AccountName,
            command.Currency,
            (BankAccountType)command.AccountType);

        await _bankAccountRepository.AddAsync(bankAccount, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return bankAccount.Id;
    }
}
