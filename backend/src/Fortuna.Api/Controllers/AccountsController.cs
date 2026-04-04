using Fortuna.Application.Accounts.Commands.DepositMoney;
using Fortuna.Application.Accounts.Commands.OpenBankAccount;
using Fortuna.Application.Accounts.Commands.WithdrawMoney;
using Fortuna.Contracts.Accounts;
using Microsoft.AspNetCore.Mvc;

namespace Fortuna.Api.Controllers;

[ApiController]
[Route("api/accounts")]
public sealed class AccountsController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Open(
        [FromBody] OpenBankAccountRequest request,
        [FromServices] OpenBankAccountCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var accountId = await handler.Handle(
            new OpenBankAccountCommand(request.CustomerId, request.AccountNumber, request.AccountName, request.Currency),
            cancellationToken);

        return Ok(accountId);
    }

    [HttpPost("{accountId:guid}/deposit")]
    public async Task<ActionResult<Guid>> Deposit(
        Guid accountId,
        [FromBody] DepositMoneyRequest request,
        [FromServices] DepositMoneyCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            new DepositMoneyCommand(accountId, request.Amount, request.Currency, request.Title),
            cancellationToken);

        return Ok(result);
    }

    [HttpPost("{accountId:guid}/withdraw")]
    public async Task<ActionResult<Guid>> Withdraw(
        Guid accountId,
        [FromBody] WithdrawMoneyRequest request,
        [FromServices] WithdrawMoneyCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            new WithdrawMoneyCommand(accountId, request.Amount, request.Currency, request.Title),
            cancellationToken);

        return Ok(result);
    }
}
