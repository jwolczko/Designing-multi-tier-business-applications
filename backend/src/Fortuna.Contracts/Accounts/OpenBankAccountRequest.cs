namespace Fortuna.Contracts.Accounts;

public sealed record OpenBankAccountRequest(
    Guid CustomerId,
    string AccountNumber,
    string AccountName,
    string Currency);
