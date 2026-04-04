using Fortuna.Application.Customers.Commands.RegisterCustomer;
using Fortuna.Contracts.Customers;
using Microsoft.AspNetCore.Mvc;

namespace Fortuna.Api.Controllers;

[ApiController]
[Route("api/customers")]
public sealed class CustomersController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<RegisterCustomerResponse>> Register(
        [FromBody] RegisterCustomerRequest request,
        [FromServices] RegisterCustomerCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var customerId = await handler.Handle(
            new RegisterCustomerCommand(request.FirstName, request.LastName, request.Email),
            cancellationToken);

        return CreatedAtAction(nameof(Register), new RegisterCustomerResponse(customerId));
    }
}
