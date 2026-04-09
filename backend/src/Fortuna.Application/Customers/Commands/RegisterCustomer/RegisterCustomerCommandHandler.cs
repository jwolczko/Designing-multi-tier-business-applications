using Fortuna.Application.Abstractions.Messaging;
using Fortuna.Application.Abstractions.Persistence;
using Fortuna.Application.Abstractions.Security;
using Fortuna.Application.Common.Exceptions;
using Fortuna.Domain.Customers;
using Fortuna.Domain.Customers.Repositories;

namespace Fortuna.Application.Customers.Commands.RegisterCustomer;

public sealed class RegisterCustomerCommandHandler : ICommandHandler<RegisterCustomerCommand, Guid>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(RegisterCustomerCommand command, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.Password) || command.Password.Length < 8)
            throw new ValidationException("Password must be at least 8 characters long.");

        var email = new Email(command.Email);
        var existingCustomer = await _customerRepository.GetByEmailAsync(email, cancellationToken);

        if (existingCustomer is not null)
            throw new ValidationException("Customer with this email already exists.");

        var customer = new Customer(
            CustomerId.New(),
            new FullName(command.FirstName, command.LastName),
            email,
            _passwordHasher.Hash(command.Password));

        await _customerRepository.AddAsync(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return customer.Id.Value;
    }
}
