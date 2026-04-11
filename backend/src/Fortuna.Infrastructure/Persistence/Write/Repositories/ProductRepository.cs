using Fortuna.Domain.Products;
using Fortuna.Domain.Products.Repositories;

namespace Fortuna.Infrastructure.Persistence.Write.Repositories;

public sealed class ProductRepository : IProductRepository
{
    private readonly WriteDbContext _dbContext;

    public ProductRepository(WriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task AddAsync(Product product, CancellationToken cancellationToken)
        => _dbContext.Products.AddAsync(product, cancellationToken).AsTask();
}
