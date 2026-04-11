using Fortuna.Application.Abstractions.Messaging;
using Fortuna.Application.Abstractions.Persistence;
using Fortuna.Application.Abstractions.Security;
using Fortuna.Application.Common.Exceptions;
using Fortuna.Domain.Accounts;
using Fortuna.Domain.Cards;
using Fortuna.Domain.Customers;
using Fortuna.Domain.Products;
using Fortuna.Domain.Customers.Repositories;
using Fortuna.Domain.Products.Repositories;

namespace Fortuna.Application.Customers.Commands.RegisterCustomer;

public sealed class RegisterCustomerCommandHandler : ICommandHandler<RegisterCustomerCommand, Guid>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IProductRepository _productRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IProductRepository productRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _productRepository = productRepository;
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

        var customerType = ParseCustomerType(command.CustomerType);
        var customer = new Customer(
            CustomerId.New(),
            new FullName(command.FirstName, command.LastName),
            email,
            _passwordHasher.Hash(command.Password),
            customerType);

        await _customerRepository.AddAsync(customer, cancellationToken);

        foreach (var product in CreateDefaultProducts(customer.Id, customerType))
        {
            await _productRepository.AddAsync(product, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return customer.Id.Value;
    }

    private static CustomerType ParseCustomerType(string customerType)
    {
        if (string.Equals(customerType, "Norma", StringComparison.OrdinalIgnoreCase))
            return CustomerType.Normal;

        if (Enum.TryParse<CustomerType>(customerType, true, out var parsedCustomerType))
            return parsedCustomerType;

        throw new ValidationException("Customer type must be 'Normal' or 'Prestige'.");
    }

    private static IEnumerable<Product> CreateDefaultProducts(CustomerId customerId, CustomerType customerType)
    {
        yield return BankAccount.Open(
            customerId,
            new AccountNumber(GenerateNumber("PL", 26)),
            customerType == CustomerType.Prestige ? "Prestige Account" : "Standard Account",
            "PLN",
            customerType == CustomerType.Prestige ? BankAccountType.Prestige : BankAccountType.Standard);

        yield return Card.Create(
            customerId,
            "Debit Card",
            GenerateNumber("CARD", 24),
            "PLN",
            CardType.Debit);

        if (customerType == CustomerType.Prestige)
        {
            yield return Card.Create(
                customerId,
                "Credit Card",
                GenerateNumber("CARD", 24),
                "PLN",
                CardType.Credit,
                10000m);
        }
    }

    private static string GenerateNumber(string prefix, int guidCharacters)
        => $"{prefix}{Guid.NewGuid():N}"[..(prefix.Length + guidCharacters)];
}
