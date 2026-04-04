using Fortuna.Application.Abstractions.Messaging;
using Fortuna.Application.Abstractions.Persistence;
using Fortuna.Domain.Customers;
using Fortuna.Domain.Customers.Repositories;

namespace Fortuna.Application.Customers.Commands.RegisterCustomer;

public sealed class RegisterCustomerCommandHandler : ICommandHandler<RegisterCustomerCommand, Guid>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(RegisterCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = new Customer(
            CustomerId.New(),
            new FullName(command.FirstName, command.LastName),
            new Email(command.Email));

        await _customerRepository.AddAsync(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return customer.Id.Value;
    }
}
