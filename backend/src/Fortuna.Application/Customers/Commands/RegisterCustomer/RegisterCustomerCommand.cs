using Fortuna.Application.Abstractions.Messaging;

namespace Fortuna.Application.Customers.Commands.RegisterCustomer;

public sealed record RegisterCustomerCommand(
    string FirstName,
    string LastName,
    string Email) : ICommand<Guid>;
