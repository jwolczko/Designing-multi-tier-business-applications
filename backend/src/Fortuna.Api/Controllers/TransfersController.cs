using Fortuna.Application.Transfers.Commands.CreateTransfer;
using Fortuna.Contracts.Transfers;
using Microsoft.AspNetCore.Mvc;

namespace Fortuna.Api.Controllers;

[ApiController]
[Route("api/transfers")]
public sealed class TransfersController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromBody] CreateTransferRequest request,
        [FromServices] CreateTransferCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var transferId = await handler.Handle(
            new CreateTransferCommand(
                request.SourceAccountId,
                request.TargetAccountId,
                request.Amount,
                request.Currency,
                request.Title),
            cancellationToken);

        return Ok(transferId);
    }
}
