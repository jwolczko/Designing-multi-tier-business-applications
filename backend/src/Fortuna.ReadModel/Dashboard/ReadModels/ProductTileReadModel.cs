namespace Fortuna.ReadModel.Dashboard.ReadModels;

public sealed class ProductTileReadModel
{
    public Guid AccountId { get; set; }
    public Guid CustomerId { get; set; }
    public string AccountName { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public string Currency { get; set; } = string.Empty;
}
