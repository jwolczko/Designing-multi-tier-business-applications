using Fortuna.Application.Transfers.Commands.CreateTransfer;
using Fortuna.Application.Accounts.Commands.DepositMoney;
using Fortuna.Api.Security;
using Fortuna.Contracts.Transfers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fortuna.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/transfers")]
public sealed class TransfersController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromBody] CreateTransferRequest request,
        [FromServices] CreateTransferCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var customerId = User.GetRequiredCustomerId();

        var transferId = await handler.Handle(
            new CreateTransferCommand(
                request.TransferType,
                customerId,
                request.SourceAccountId,
                request.TargetAccountId,
                request.TargetAccountNumber,
                request.RecipientName,
                request.Amount,
                request.Currency,
                request.Title),
            cancellationToken);

        return Ok(transferId);
    }

    [HttpPost("incoming")]
    public async Task<ActionResult<Guid>> SimulateIncoming(
        [FromBody] SimulateIncomingTransferRequest request,
        [FromServices] DepositMoneyCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var customerId = User.GetRequiredCustomerId();

        var result = await handler.Handle(
            new DepositMoneyCommand(
                customerId,
                request.TargetAccountId,
                request.Amount,
                request.Currency,
                request.Title),
            cancellationToken);

        return Ok(result);
    }
}
